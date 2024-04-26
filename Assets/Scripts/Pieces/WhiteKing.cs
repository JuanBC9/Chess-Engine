using Assets.Sprites;

namespace Assets.Pieces
{
    public class WhiteKing : Piece
    {
        public static ulong AttackingSquares(BoardState state, ulong piecePosition)
        {
            var attackingSquares = ValidMoves(state, piecePosition);

            attackingSquares &= ~piecePosition >> 2;
            attackingSquares &= ~piecePosition << 2;

            return attackingSquares & ~state.allPieces;
        }

        public static ulong ValidMoves(BoardState state, ulong piecePosition)
        {
            ulong validMoves = 0;

            if (state == null) return validMoves;

            // Movimientos posibles
            
            // Los movimientos posibles de la reina son la vertical, horizontal y diagonales en las que se encuentra
            // hasta que se encuentra con una pieza o hasta que llega al borde del tablero

            ulong up = piecePosition;
            ulong down = piecePosition;
            ulong left = piecePosition;
            ulong right = piecePosition;
            ulong upLeft = piecePosition;
            ulong upRight = piecePosition;
            ulong downLeft = piecePosition;
            ulong downRight = piecePosition;
            ulong castleRight = piecePosition;
            ulong castleLeft = piecePosition;

            bool upStopped = false, downStopped = false, leftStopped = false, rightStopped = false;
            bool upLeftStopped = false, upRightStopped = false, downLeftStopped = false, downRightStopped = false;

            if ((up & BoardPositionalInfo.Row8) > 0) upStopped = true;
            if ((down & BoardPositionalInfo.Row1) > 0) downStopped = true;
            if ((left & BoardPositionalInfo.ColA) > 0) leftStopped = true;
            if ((right & BoardPositionalInfo.ColH) > 0) rightStopped = true;
            if (upStopped || leftStopped) upLeftStopped = true;
            if (upStopped || rightStopped) upRightStopped = true;
            if (downStopped || leftStopped) downLeftStopped = true;
            if (downStopped || rightStopped) downRightStopped = true;

            if (!upStopped)
            {
                up <<= 8;
                
                if ((up & state.whitePieces) > 0) upStopped = true;
                else if ((up & state.blackPieces) > 0 || (up & BoardPositionalInfo.Row8) > 0)
                {
                    validMoves |= up;
                    upStopped = true;
                }
            }

            if (!downStopped)
            {
                down >>= 8;
                
                if ((down & state.whitePieces) > 0) downStopped = true;
                else if ((down & state.blackPieces) > 0 || (down & BoardPositionalInfo.Row1) > 0)
                {
                    validMoves |= down;
                    downStopped = true;
                }
            }

            if (!leftStopped)
            {
                left <<= 1;
                
                if ((left & state.whitePieces) > 0) leftStopped = true;
                else if ((left & state.blackPieces) > 0 || (left & BoardPositionalInfo.ColA) > 0)
                {
                    validMoves |= left;
                    leftStopped = true;
                }
            }

            if (!rightStopped)
            {
                right >>= 1;
                
                if ((right & state.whitePieces) > 0) rightStopped = true;
                else if ((right & state.blackPieces) > 0 || (right & BoardPositionalInfo.ColH) > 0)
                {
                    validMoves |= right;
                    rightStopped = true;
                }
            }

            if (!upLeftStopped)
            {
                upLeft <<= 9;

                if ((upLeft & state.whitePieces) > 0) upLeftStopped = true;
                else if ((upLeft & state.blackPieces) > 0 || (upLeft & BoardPositionalInfo.Row8) > 0 || (upLeft & BoardPositionalInfo.ColA) > 0)
                {
                    validMoves |= upLeft;
                    upLeftStopped = true;
                }
            }
            
            if (!upRightStopped)
            {
                upRight <<= 7;

                if ((upRight & state.whitePieces) > 0) upRightStopped = true;
                else if ((upRight & state.blackPieces) > 0 || (upRight & BoardPositionalInfo.Row8) > 0 || (upRight & BoardPositionalInfo.ColH) > 0)
                {
                    validMoves |= upRight;
                    upRightStopped = true;
                }
            }
            
            if (!downLeftStopped)
            {
                downLeft >>= 7;

                if ((downLeft & state.whitePieces) > 0) downLeftStopped = true;
                else if ((downLeft & state.blackPieces) > 0 || (downLeft & BoardPositionalInfo.Row1) > 0 || (downLeft & BoardPositionalInfo.ColA) > 0)
                {
                    validMoves |= downLeft;
                    downLeftStopped = true;
                }
            }
            
            if (!downRightStopped)
            {
                downRight >>= 9;

                if ((downRight & state.whitePieces) > 0) downRightStopped = true;
                else if ((downRight & state.blackPieces) > 0 || (downRight & BoardPositionalInfo.Row1) > 0 || (downRight & BoardPositionalInfo.ColH) > 0)
                {
                    validMoves |= downRight;
                    downRightStopped = true;
                }
            }

            if (state.CanWhiteCastleRight)
            {
                if (!rightStopped)
                {
                    castleRight >>= 2;

                    if (!((castleRight & state.allPieces) > 0) && !(((castleRight | right) & state.blackAttackingSquares) > 0))
                    {
                        validMoves |= castleRight;
                    }
                }
            }

            if (state.CanWhiteCastleLeft)
            {
                if (!leftStopped)
                {
                    castleLeft <<= 2;

                    if (!((castleLeft & state.allPieces) > 0) && !(((castleLeft | left | (castleLeft << 1)) & state.blackAttackingSquares) > 0))
                    {
                        validMoves |= castleLeft;
                    }
                }
            }

            if (!upStopped) validMoves |= up;
            if (!downStopped) validMoves |= down;
            if (!leftStopped) validMoves |= left;
            if (!rightStopped) validMoves |= right;
            if (!upLeftStopped) validMoves |= upLeft;
            if (!upRightStopped) validMoves |= upRight;
            if (!downLeftStopped) validMoves |= downLeft;
            if (!downRightStopped) validMoves |= downRight;
            
            
            return validMoves & ~state.blackAttackingSquares;
        }

        public override ulong GetValidMoves(BoardState state, ulong piecePosition)
        {
            return ValidMoves(state, piecePosition);
        }
        public override ulong GetAttackingSquares(BoardState state, ulong piecePosition)=> AttackingSquares(state, piecePosition);
    }
}