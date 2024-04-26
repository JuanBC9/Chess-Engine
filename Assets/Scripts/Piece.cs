using Assets;
using Assets.Pieces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Piece : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public static event Action<Piece> OnCancelMove;
    public static event Action<Piece> OnPieceDrop;
    public event Action<Piece> OnPieceBeginDrag;

    public static int[] PieceValues =
    {
        1,1,3,3,3,3,5,5,9,9, 1000, 1000
    };

    public PieceType type;

    private RectTransform _rectTransform;
    private Tile _currentTile;
    private Tile _previousTile;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SetTile(Tile tile)
    {
        if (_currentTile != null)
        {
            _previousTile = _currentTile;
        }

        _currentTile = tile;
    }

    public bool CanMove(BoardState state) { return !state.WhiteMoves && ((int)type % 2 == 0) ||
                                                    state.WhiteMoves && ((int)type % 2 != 0); }

    public virtual ulong GetValidMoves(BoardState state, ulong piecePosition) { return 0; }

    public virtual ulong GetAttackingSquares(BoardState state, ulong piecePosition)
    {
        var validMoves = GetValidMoves(state, piecePosition);
        return validMoves & ~state.allPieces;
    }


    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _previousTile = _currentTile;
        _currentTile = null;

        OnPieceBeginDrag?.Invoke(this);
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        OnPieceDrop?.Invoke(this);

        if (_currentTile == null)
        {
            if (_previousTile != null)
            {
                OnCancelMove?.Invoke(this);
            }
        }
    }

    public static bool CanMove(PieceType pieceType, BoardState state)
    {
        return !state.WhiteMoves && ((int)pieceType % 2 == 0) ||
                state.WhiteMoves && ((int)pieceType % 2 != 0);
    }

    public static ulong ValidMoves(PieceType pieceType, BoardState state, ulong piecePosition)
    {
        return pieceType switch
        {
            PieceType.BlackPawn => BlackPawn.ValidMoves(state, piecePosition),
            PieceType.WhitePawn => WhitePawn.ValidMoves(state, piecePosition),
            PieceType.BlackBishop => BlackBishop.ValidMoves(state, piecePosition),
            PieceType.WhiteBishop => WhiteBishop.ValidMoves(state, piecePosition),
            PieceType.BlackHorse => BlackHorse.ValidMoves(state, piecePosition),
            PieceType.WhiteHorse => WhiteHorse.ValidMoves(state, piecePosition),
            PieceType.BlackRook => BlackRook.ValidMoves(state, piecePosition),
            PieceType.WhiteRook => WhiteRook.ValidMoves(state, piecePosition),
            PieceType.BlackQueen => BlackQueen.ValidMoves(state, piecePosition),
            PieceType.WhiteQueen => WhiteQueen.ValidMoves(state, piecePosition),
            PieceType.BlackKing => BlackKing.ValidMoves(state, piecePosition),
            PieceType.WhiteKing => WhiteKing.ValidMoves(state, piecePosition),
            _ => 0,
        };
    }

    public static ulong AttackingSquares(PieceType pieceType, BoardState state, ulong piecePosition)
    {
        return pieceType switch
        {
            PieceType.BlackPawn => BlackPawn.AttackingSquares(state, piecePosition) & ~state.allPieces,
            PieceType.WhitePawn => WhitePawn.AttackingSquares(state, piecePosition) & ~state.allPieces,
            PieceType.BlackBishop => BlackBishop.AttackingSquares(state, piecePosition) & ~state.allPieces,
            PieceType.WhiteBishop => WhiteBishop.AttackingSquares(state, piecePosition) & ~state.allPieces,
            PieceType.BlackHorse => BlackHorse.AttackingSquares(state, piecePosition) & ~state.allPieces,
            PieceType.WhiteHorse => WhiteHorse.AttackingSquares(state, piecePosition) & ~state.allPieces,
            PieceType.BlackRook => BlackRook.AttackingSquares(state, piecePosition) & ~state.allPieces,
            PieceType.WhiteRook => WhiteRook.AttackingSquares(state, piecePosition) & ~state.allPieces,
            PieceType.BlackQueen => BlackQueen.AttackingSquares(state, piecePosition) & ~state.allPieces,
            PieceType.WhiteQueen => WhiteQueen.AttackingSquares(state, piecePosition) & ~state.allPieces,
            PieceType.BlackKing => BlackKing.AttackingSquares(state, piecePosition) & ~state.allPieces,
            PieceType.WhiteKing => WhiteKing.AttackingSquares(state, piecePosition) & ~state.allPieces,
            _ => 0,
        };
    }
}

public enum PieceType
{
    BlackPawn, // 0
    WhitePawn,
    BlackBishop, // 2
    WhiteBishop,
    BlackHorse, // 4
    WhiteHorse,
    BlackRook, // 6
    WhiteRook,
    BlackQueen, // 8
    WhiteQueen,
    BlackKing, // 10
    WhiteKing
}
