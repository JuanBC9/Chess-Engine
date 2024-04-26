using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromotionSelector : MonoBehaviour
{
    public event Action<PieceType> OnPieceSelected;

    [SerializeField] Button BishopButton;
    [SerializeField] Button HorseButton;
    [SerializeField] Button RookButton;
    [SerializeField] Button QueenButton;
    [SerializeField] bool WhitePieces;

    private void Awake()
    {
        if (WhitePieces)
        {
            BishopButton.onClick.AddListener(() => SelectPiece(PieceType.WhiteBishop));
            HorseButton.onClick.AddListener(() => SelectPiece(PieceType.WhiteHorse));
            RookButton.onClick.AddListener(() => SelectPiece(PieceType.WhiteRook));
            QueenButton.onClick.AddListener(() => SelectPiece(PieceType.WhiteQueen));
        }
        else
        {
            BishopButton.onClick.AddListener(() => SelectPiece(PieceType.BlackBishop));
            HorseButton.onClick.AddListener(() => SelectPiece(PieceType.BlackHorse));
            RookButton.onClick.AddListener(() => SelectPiece(PieceType.BlackRook));
            QueenButton.onClick.AddListener(() => SelectPiece(PieceType.BlackQueen));
        }
    }

    private void SelectPiece(PieceType type)
    {
        OnPieceSelected?.Invoke(type);
        Destroy(gameObject);
    }
}
