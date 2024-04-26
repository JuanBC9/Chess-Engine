using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public static event Action<Tile> OnPieceDragged;
    public static event Action<Tile, Piece> OnPieceDropped;

    public Image highLightImg;

    private RectTransform _rectTransform;
    public Piece CurrentPiece => _currentPiece;
    private Piece _currentPiece;

    public ulong Position => _position;
    private ulong _position;

    private bool _canBePlaceable = false;

    private void Awake()
    {
        Piece.OnPieceDrop += DropPiece;
        _rectTransform = GetComponent<RectTransform>();
        
        if (_currentPiece != null)
        {
            UpdateCurrentPiecePosition();
            _currentPiece.OnPieceBeginDrag += DragPiece;
        }
    }

    public void DragPiece(Piece piece)
    {
        _currentPiece.OnPieceBeginDrag -= DragPiece;
        _currentPiece = null;

        OnPieceDragged?.Invoke(this);
    }

    public Piece DragPieceSilent()
    {
        if (_currentPiece == null) return null;

        var piece = _currentPiece;
        _currentPiece.OnPieceBeginDrag -= DragPiece;
        _currentPiece = null;

        return piece;
    }

    public void SetPiece(Piece piece)
    {
        _currentPiece = piece;
        piece.SetTile(this);
        _currentPiece.OnPieceBeginDrag += DragPiece;
        UpdateCurrentPiecePosition();
    }

    public void DropPiece(Piece piece)
    {
        if (!_canBePlaceable) return;

        if (!(piece.transform.position.x >= _rectTransform.transform.position.x - (_rectTransform.sizeDelta.x * .5f) &&
              piece.transform.position.x < _rectTransform.transform.position.x + (_rectTransform.sizeDelta.x * .5f) &&
              piece.transform.position.y >= _rectTransform.transform.position.y - (_rectTransform.sizeDelta.y * .5f) &&
              piece.transform.position.y < _rectTransform.transform.position.y + (_rectTransform.sizeDelta.y * .5f)))
        {
            return;
        }

        SetPiece(piece);

        OnPieceDropped?.Invoke(this, _currentPiece);
    }

    private void UpdateCurrentPiecePosition()
    {
        if (_currentPiece == null) return;

        _currentPiece.transform.position = _rectTransform.position;
    }

    public void Init(int row, int column)
    {
        ulong pos = 1;

        _position = pos << (row * 8 + column);
    }

    public void SetCanBePlaceable(bool canPlace)
    {
        _canBePlaceable = canPlace;
    }

    public void ToggleHighlight(bool v)
    {
        highLightImg.gameObject.SetActive(v);
    }
}
