using Assets;
using System.Collections.Generic;
using Assets.Sprites;
using UnityEngine;
using System.Linq;
using UnityEditor;
using System;
using System.Threading.Tasks;

public class Board : MonoBehaviour
{
    [SerializeField] private Tile[] _tiles;
    [SerializeField] private Piece[] _piecePrefabs;
    [SerializeField] private PromotionSelector _promotionSelectorPrefab;
    [SerializeField] private List<PieceType> _takenPieces;

    private Dictionary<Tile, Piece> _boardInfo;
    private List<Piece> _pieces;

    public BoardState BoardState => _boardState;
    private BoardState _boardState;

    private Tile _prevTile;

    private void Awake()
    {
        _boardState = new BoardState();
        _boardInfo = new Dictionary<Tile, Piece>();
        _pieces = new List<Piece>();
        
        Tile.OnPieceDropped += ManagePieceDropped;
        Tile.OnPieceDragged += ManagePieceDragged;
        Piece.OnCancelMove += ManagePieceMovementCancel;
    }

    private void OnDestroy()
    {
        Tile.OnPieceDropped -= ManagePieceDropped;
        Tile.OnPieceDragged -= ManagePieceDragged;
        Piece.OnCancelMove -= ManagePieceMovementCancel;
    }

    public void Init()
    {
        Canvas.ForceUpdateCanvases();

        UpdateBoard(_boardState);
        
        CalculateAttackingSquares();
    }

    private void UpdateBoard(BoardState state)
    {
        ClearBoard();
        DrawState(state);
    }

    private void ClearBoard()
    {
        foreach (var tile in _tiles)
        {
            tile.DragPieceSilent();
        }

        for (int i = 0; i < _pieces.Count; i++)
        {
            Destroy(_pieces[i].gameObject);
        }

        _pieces.Clear();
        _boardInfo.Clear();
    }

    private void DrawState(BoardState state)
    {
        for (var i = 0; i < 8; i++) //row
        {
            for (var j = 0; j < 8; j++)//column
            {
                var tile = _tiles[j + i * 8];
                _boardInfo.Add(tile, null);
                tile.Init(i, j);
            }
        }

        foreach (var t in _tiles)
        {
            for (var i = 0; i < state.Pieces.Length; i++)
            {
                var pieces = state.Pieces[i];

                if ((t.Position & pieces) <= 0) continue; // continuo si no hay una pieza en esta tile

                //instanciar pieza en la tile
                var piece = Instantiate(_piecePrefabs[i], t.transform.position, Quaternion.identity, transform.parent);
                t.SetPiece(piece);
                _boardInfo[t] = piece;
                _pieces.Add(piece);

                break;
            }
        }
    }

    private void CalculateAttackingSquares()
    {
        _boardState.blackAttackingSquares = 0;
        _boardState.whiteAttackingSquares = 0;

        foreach (var pieceInfo in _boardInfo.Where(p => p.Value != null))
        {
            if ((int)pieceInfo.Value.type % 2 == 0) //Black pieces
            {
                _boardState.blackAttackingSquares |= pieceInfo.Value.GetAttackingSquares(_boardState, pieceInfo.Key.Position);
            }
            else //White pieces
            {
                _boardState.whiteAttackingSquares |= pieceInfo.Value.GetAttackingSquares(_boardState, pieceInfo.Key.Position);
            }
        }
    }

    private void ManagePieceMovementCancel(Piece piece)
    {
        _prevTile.SetPiece(piece);
        
        //hide valid squares
        ClearHighlightedSquares();
        
        _boardInfo[_prevTile] = piece;
        
        _prevTile = null;
    }

    private void ManagePieceDragged(Tile tile)
    {
        ulong validSquares = 0;

        //Get valid squares
        if (_boardInfo[tile].CanMove(_boardState)) 
        { 
            validSquares = _boardInfo[tile].GetValidMoves(_boardState, tile.Position);
        }
        
        //set prevPiecePosition
        _prevTile = tile;

        //highLight and enable valid squares
        _boardInfo[tile].transform.SetAsLastSibling();
        HighlightSquares(validSquares);

        //Update board info
        _boardInfo[tile] = null;
    }

    public IEnumerable<Move> GetAllPieceValidMoves(PieceType pieceType, ulong tilePosition, BoardState state)
    {
        List<Move> result = new List<Move>();
        var piece = _piecePrefabs[(int)pieceType];

        if (piece.CanMove(state)) return result;

        ulong validMoves = piece.GetValidMoves(state, tilePosition);

        if (validMoves == 0) return result;

        for (ulong i = 1; i <= 9223372036854775808; i <<= 1)
        {
            if (i == 0) break;
            if ((i & validMoves) > 0)
            {
                ulong endTile = i;

                result.Add(new Move
                {
                    PieceType = pieceType,
                    Tile1 = tilePosition,
                    Tile2 = endTile
                });
            }
        }

        return result;
    }

    public IEnumerable<Move> GetAllValidMoves(BoardState state)
    {
        List<Move> result = new List<Move>();
        object lockObj = new object();

        Parallel.For(0, 12, (i) =>
        {
            PieceType pieceType = (PieceType)i;

            if (!Piece.CanMove(pieceType, state)) return;

            ulong piecePositions = state.Pieces[i];

            for (ulong init = 1; init <= 9223372036854775808; init <<= 1)
            {
                if (init == 0) break;

                if ((init & piecePositions) <= 0) continue;

                ulong validMoves = Piece.ValidMoves(pieceType, state, init);

                if (validMoves == 0) continue;

                for (ulong end = 1; end <= 9223372036854775808; end <<= 1)
                {
                    if (end == 0) break;

                    if ((end & validMoves) <= 0) continue;

                    if (pieceType == PieceType.BlackPawn && (end & BoardPositionalInfo.Row1) > 0 ||
                        pieceType == PieceType.WhitePawn && (end & BoardPositionalInfo.Row8) > 0)
                    {
                        lock (lockObj)
                        {
                            result.Add(new Move
                            {
                                PieceType = pieceType,
                                Tile1 = init,
                                Tile2 = end,
                                NewPieceType = pieceType == PieceType.BlackPawn ? PieceType.BlackBishop : PieceType.WhiteBishop,
                                Evaluation = 0
                            });
                            result.Add(new Move
                            {
                                PieceType = pieceType,
                                Tile1 = init,
                                Tile2 = end,
                                NewPieceType = pieceType == PieceType.BlackPawn ? PieceType.BlackHorse : PieceType.WhiteHorse,
                                Evaluation = 0
                            });
                            result.Add(new Move
                            {
                                PieceType = pieceType,
                                Tile1 = init,
                                Tile2 = end,
                                NewPieceType = pieceType == PieceType.BlackPawn ? PieceType.BlackRook : PieceType.WhiteRook,
                                Evaluation = 0
                            });
                            result.Add(new Move
                            {
                                PieceType = pieceType,
                                Tile1 = init,
                                Tile2 = end,
                                NewPieceType = pieceType== PieceType.BlackPawn ? PieceType.BlackQueen : PieceType.WhiteQueen,
                                Evaluation = 0
                            });
                        }
                    }
                    else
                    {
                        lock (lockObj)
                        {
                            result.Add(new Move
                            {
                                PieceType = pieceType,
                                Tile1 = init,
                                Tile2 = end,
                                NewPieceType = pieceType,
                                Evaluation = 0
                            });
                        }
                    }

                }
            }
        });

        return result;
    }

    private async void ManagePieceDropped(Tile tile, Piece piece)
    {
        //hide valid squares
        ClearHighlightedSquares();

        //Update board Info
        _boardInfo[tile] = piece;

        //Check pawn promotion
        if (piece.type == PieceType.BlackPawn && (tile.Position & BoardPositionalInfo.Row1) > 0 ||
            piece.type == PieceType.WhitePawn && (tile.Position & BoardPositionalInfo.Row8) > 0)
        {
            bool pieceSelected = false;
            PieceType promotionPieceSelected = piece.type;
            var promotionSelector = Instantiate(_promotionSelectorPrefab, tile.transform.position, Quaternion.identity, transform.parent);

            promotionSelector.OnPieceSelected += SelectType;

            while (!pieceSelected)
            {
                await Task.Delay(100);
            }

            //update board State
            UpdateBoardState(piece.type, _prevTile.Position, tile.Position, promotionPieceSelected);

            void SelectType(PieceType type)
            {
                promotionSelector.OnPieceSelected -= SelectType;
                pieceSelected = true;
                promotionPieceSelected = type;
            }
        }
        else
        {
            //update board State
            UpdateBoardState(piece.type, _prevTile.Position, tile.Position, piece.type);
        }



        UpdateBoard(_boardState);
        CalculateAttackingSquares();
    }

    [ContextMenu("Highlight black attacking squares")]
    private void ShowBlackAttackingSquares() => HighlightSquares(_boardState.blackAttackingSquares, false);
    
    [ContextMenu("Highlight white attacking squares")]
    private void ShowWhiteAttackingSquares() => HighlightSquares(_boardState.whiteAttackingSquares, false);

    [ContextMenu("Clear Attacking Squares")]
    private void ClearAttackingSquares() => HighlightSquares(0, false);

    private void ClearHighlightedSquares() => HighlightSquares(0);
    
    private void HighlightSquares(ulong validSquares, bool enablePlaceable = true)
    {
        foreach (var t in _tiles)
        {
            t.ToggleHighlight((t.Position & validSquares) > 0);
            if (enablePlaceable) t.SetCanBePlaceable((t.Position & validSquares) > 0);
        }
    }

    private void UpdateBoardState(PieceType piece, ulong prevTile, ulong newTile, PieceType newPieceType)
    {
        if (prevTile == newTile) return;

        _boardState.UpdateState(piece, prevTile, newTile, newPieceType);
    }

    public void MakeMove(Move move)
    {
        //update board State
        UpdateBoardState(move.PieceType, move.Tile1, move.Tile2, move.NewPieceType);


        UpdateBoard(_boardState);
        CalculateAttackingSquares();
    }

    public void GetGameState(ref GameState gs)
    {
        gs.WhiteScore = _boardState.WhiteScore;
        gs.BlackScore = _boardState.BlackScore;

        if (!_pieces.Exists(p => p.type is PieceType.BlackKing))
        {
            gs.GameFinished = true;
            gs.FinishValue = 1;
        }
        else if (!_pieces.Exists(p => p.type is PieceType.WhiteKing))
        {
            gs.GameFinished = true;
            gs.FinishValue = -1;
        }
    }
}
