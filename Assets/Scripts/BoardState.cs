using Assets.Sprites;
using System.Linq;

namespace Assets
{
    public class BoardState
    {
        //Piece Placement
        public ulong[] Pieces;

        //Turn
        public bool WhiteMoves;

        //Castle Rights (https://www.chessprogramming.org/Castling_Rights)
        public bool CanWhiteCastleRight;
        public bool CanWhiteCastleLeft;
        public bool CanBlackCastleRight;
        public bool CanBlackCastleLeft;

        //En passant target square (https://www.chessprogramming.org/En_passant)
        public ulong EnPassantSquare;

        //HalfMove clock (https://www.chessprogramming.org/Halfmove_Clock)
        public int HalfMoveCount;

        public ulong whiteAttackingSquares;
        public ulong blackAttackingSquares;

        public int BlackScore;
        public int WhiteScore;

        
        public ulong blackPieces => Pieces[0] | Pieces[2] | Pieces[4] | Pieces[6] | Pieces[8] | Pieces[10];
        public ulong whitePieces => Pieces[1] | Pieces[3] | Pieces[5] | Pieces[7] | Pieces[9] | Pieces[11];
        public ulong allPieces => blackPieces | whitePieces;
        
        public BoardState(BoardState state)
        {
            Pieces = new ulong[12]
            {
                state.Pieces[0],
                state.Pieces[1],
                state.Pieces[2],
                state.Pieces[3],
                state.Pieces[4],
                state.Pieces[5],
                state.Pieces[6],
                state.Pieces[7],
                state.Pieces[8],
                state.Pieces[9],
                state.Pieces[10],
                state.Pieces[11],
            };

            WhiteMoves = state.WhiteMoves;

            CanWhiteCastleRight = state.CanWhiteCastleRight;
            CanWhiteCastleLeft = state.CanWhiteCastleLeft;
            CanBlackCastleRight = state.CanBlackCastleRight;
            CanBlackCastleLeft = state.CanBlackCastleLeft;

            EnPassantSquare = state.EnPassantSquare;
            HalfMoveCount = state.HalfMoveCount;

            whiteAttackingSquares = state.whiteAttackingSquares;
            blackAttackingSquares = state.blackAttackingSquares;

            BlackScore = state.BlackScore;
            WhiteScore = state.WhiteScore;
        }

        public BoardState()
        {
            Pieces = new ulong[12]
            {
                71776119061217280,
                65280,
                2594073385365405696,
                36,
                4755801206503243776,
                66,
                9295429630892703744,
                129,
                1152921504606846976,
                16,
                576460752303423488,
                8
            };

            WhiteMoves = true;

            CanWhiteCastleRight = true;
            CanWhiteCastleLeft = true;
            CanBlackCastleRight = true;
            CanBlackCastleLeft = true;

            EnPassantSquare = 0;

            HalfMoveCount = 0;

            whiteAttackingSquares = BoardPositionalInfo.Row2;
            blackAttackingSquares = BoardPositionalInfo.Row7;

            BlackScore = 0;
            WhiteScore = 0;
        }

        public void DeletePiece(PieceType pieceType, ulong tilePosition)
        {
            Pieces[(int)pieceType] &= ~tilePosition;
        }

        public void AddPiece(PieceType pieceType, ulong tilePosition)
        {
            Pieces[(int)pieceType] |= tilePosition;
        }

        public void UpdatePiecePosition(PieceType piece, ulong prevTilePosition, ulong newTilePosition)
        {
            DeletePiece(piece, prevTilePosition);
            AddPiece(piece, newTilePosition);
        }

        public BoardState UpdateState(Move move)
        {
            return UpdateState(move.PieceType, move.Tile1, move.Tile2, move.NewPieceType);
        }

        public BoardState UpdateState(PieceType piece, ulong prevTilePosition, ulong newTilePosition, PieceType newPieceType)
        {
            if (prevTilePosition == newTilePosition) return this;

            if ((newTilePosition & allPieces) > 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    if ((Pieces[i] & newTilePosition) > 0)
                    {
                        DeletePiece((PieceType)i, newTilePosition);
                        if (i % 2 == 0) // BlackPieces
                        {
                            WhiteScore += Piece.PieceValues[i];
                        }
                        else
                        {
                            BlackScore += Piece.PieceValues[i];
                        }

                        break;
                    }
                }
            }

            UpdatePiecePosition(piece, prevTilePosition, newTilePosition);

            //Check Castle
            if (piece == PieceType.BlackKing)
            {
                CanBlackCastleLeft = false;
                CanBlackCastleRight = false;

                if (newTilePosition == (prevTilePosition << 2)) //castle left
                {
                    UpdatePiecePosition(PieceType.BlackRook, newTilePosition << 2, newTilePosition >> 1);
                }
                else if (newTilePosition == (prevTilePosition >> 2)) //castle right
                {
                    UpdatePiecePosition(PieceType.BlackRook, newTilePosition >> 1, newTilePosition << 1);
                }
            }

            if (piece == PieceType.WhiteKing)
            {
                CanWhiteCastleLeft = false;
                CanWhiteCastleRight = false;

                if (newTilePosition == (prevTilePosition << 2)) //castle left
                {
                    UpdatePiecePosition(PieceType.WhiteRook, newTilePosition << 2, newTilePosition >> 1);
                }
                else if (newTilePosition == (prevTilePosition >> 2)) //castle right
                {
                    UpdatePiecePosition(PieceType.WhiteRook, newTilePosition >> 1, newTilePosition << 1);
                }
            }

            if (piece == PieceType.BlackRook)
            {
                if (prevTilePosition == (BoardPositionalInfo.Row8 & BoardPositionalInfo.ColA) && CanBlackCastleLeft) // leftRook
                {
                    CanBlackCastleLeft = false;
                }
                else if (prevTilePosition == (BoardPositionalInfo.Row8 & BoardPositionalInfo.ColH) && CanBlackCastleRight) //rightRook
                {
                    CanBlackCastleRight = false;
                }
            }

            if (piece == PieceType.WhiteRook)
            {
                if (prevTilePosition == (BoardPositionalInfo.Row1 & BoardPositionalInfo.ColA) && CanWhiteCastleLeft) // leftRook
                {
                    CanWhiteCastleLeft = false;
                }
                else if (prevTilePosition == (BoardPositionalInfo.Row1 & BoardPositionalInfo.ColH) && CanWhiteCastleRight) //rightRook
                {
                    CanWhiteCastleRight = false;
                }
            }

            //Check on passant
            if (piece == PieceType.BlackPawn)
            {
                // if prevTile is in seventh rank and has moved two rows down
                if ((prevTilePosition & BoardPositionalInfo.Row7) > 0 && newTilePosition == prevTilePosition >> 16)
                {
                    EnPassantSquare = prevTilePosition >> 8;
                }
                else
                {
                    EnPassantSquare = 0;
                }
            }
            else if (piece == PieceType.WhitePawn)
            {
                // if prevTile is in second rank and has moved two rows up
                if ((prevTilePosition & BoardPositionalInfo.Row2) > 0 && newTilePosition == prevTilePosition << 16)
                {
                    EnPassantSquare = prevTilePosition << 8;
                }
                else
                {
                    EnPassantSquare = 0;
                }
            }

            //Check Promotion
            if (newPieceType != piece)
            {
                DeletePiece(piece, newTilePosition);
                AddPiece(newPieceType, newTilePosition);

                if ((int)piece % 2 == 0)
                {
                    BlackScore -= Piece.PieceValues[(int)piece];
                    BlackScore += Piece.PieceValues[(int)newPieceType];
                }
                else
                {
                    WhiteScore -= Piece.PieceValues[(int)piece];
                    WhiteScore += Piece.PieceValues[(int)newPieceType];
                }

            }

            //Check halfmove clock
            // TODO

            //Update turn
            WhiteMoves = !WhiteMoves;

            return this;
        }

        public override string ToString()
        {
            var str = "";
            var piecesPositions = new int[8, 8];

            // white pieces
            var whitePieces = Pieces[1] | Pieces[3] | Pieces[5] | Pieces[7] | Pieces[9] | Pieces[11];
            var row = 1;
            var col = 8;
            for (ulong i = 1; i > 0; i<<=1)
            {
                piecesPositions[row - 1, col - 1] = (i & whitePieces) > 0 ? 1 : piecesPositions[row - 1, col - 1];

                col--;
                if (col <= 0)
                {
                    col = 8;
                    row++;
                }
            }
            
            // black pieces
            var blackPieces = Pieces[0] | Pieces[2] | Pieces[4] | Pieces[6] | Pieces[8] | Pieces[10];
            row = 1;
            col = 8;
            for (ulong i = 1; i > 0; i<<=1)
            {
                piecesPositions[row - 1, col - 1] = (i & blackPieces) > 0 ? 2 : piecesPositions[row - 1, col - 1];

                col--;
                if (col <= 0)
                {
                    col = 8;
                    row++;
                }
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    str += $" {piecesPositions[j, i].ToString()} ";
                }

                str += "\n";
            }
            
            return str;
        }
    }
}
