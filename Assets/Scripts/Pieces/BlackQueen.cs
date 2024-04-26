using Assets.Sprites;

namespace Assets.Pieces
{
    public class BlackQueen : Piece
    {
        public static ulong AttackingSquares(BoardState state, ulong piecePosition) => ValidMoves(state, piecePosition);

        public static ulong ValidMoves(BoardState state, ulong piecePosition)
        {
            ulong validMoves = 0;

            if (state == null) return validMoves;

            // Movimientos posibles
            
            // Los movimientos posibles de la reina son la vertical, horizontal y diagonales en las que se encuentra
            // hasta que se encuentra con una pieza o hasta que llega al borde del tablero

            
            var blackPieces = state.Pieces[0] | state.Pieces[2] | state.Pieces[4] | state.Pieces[6] | state.Pieces[8] | state.Pieces[10];
            var whitePieces = state.Pieces[1] | state.Pieces[3] | state.Pieces[5] | state.Pieces[7] | state.Pieces[9] | state.Pieces[11];
            
            ulong up = piecePosition;
            ulong down = piecePosition;
            ulong left = piecePosition;
            ulong right = piecePosition;
            ulong upLeft = piecePosition;
            ulong upRight = piecePosition;
            ulong downLeft = piecePosition;
            ulong downRight = piecePosition;

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
            
            
            while (!upStopped || !downStopped || !leftStopped || !rightStopped || !upLeftStopped || !upRightStopped || !downLeftStopped || !downRightStopped)
            {
                if (!upStopped)
                {
                    up <<= 8;
                    
                    if ((up & blackPieces) > 0) upStopped = true;
                    else if ((up & whitePieces) > 0 || (up & BoardPositionalInfo.Row8) > 0)
                    {
                        validMoves |= up;
                        upStopped = true;
                    }
                }

                if (!downStopped)
                {
                    down >>= 8;
                    
                    if ((down & blackPieces) > 0) downStopped = true;
                    else if ((down & whitePieces) > 0 || (down & BoardPositionalInfo.Row1) > 0)
                    {
                        validMoves |= down;
                        downStopped = true;
                    }
                }

                if (!leftStopped)
                {
                    left <<= 1;
                    
                    if ((left & blackPieces) > 0) leftStopped = true;
                    else if ((left & whitePieces) > 0 || (left & BoardPositionalInfo.ColA) > 0)
                    {
                        validMoves |= left;
                        leftStopped = true;
                    }
                }

                if (!rightStopped)
                {
                    right >>= 1;
                    
                    if ((right & blackPieces) > 0) rightStopped = true;
                    else if ((right & whitePieces) > 0 || (right & BoardPositionalInfo.ColH) > 0)
                    {
                        validMoves |= right;
                        rightStopped = true;
                    }
                }

                if (!upLeftStopped)
                {
                    upLeft <<= 9;

                    if ((upLeft & blackPieces) > 0) upLeftStopped = true;
                    else if ((upLeft & whitePieces) > 0 || (upLeft & BoardPositionalInfo.Row8) > 0 || (upLeft & BoardPositionalInfo.ColA) > 0)
                    {
                        validMoves |= upLeft;
                        upLeftStopped = true;
                    }
                }
                
                if (!upRightStopped)
                {
                    upRight <<= 7;

                    if ((upRight & blackPieces) > 0) upRightStopped = true;
                    else if ((upRight & whitePieces) > 0 || (upRight & BoardPositionalInfo.Row8) > 0 || (upRight & BoardPositionalInfo.ColH) > 0)
                    {
                        validMoves |= upRight;
                        upRightStopped = true;
                    }
                }
                
                if (!downLeftStopped)
                {
                    downLeft >>= 7;

                    if ((downLeft & blackPieces) > 0) downLeftStopped = true;
                    else if ((downLeft & whitePieces) > 0 || (downLeft & BoardPositionalInfo.Row1) > 0 || (downLeft & BoardPositionalInfo.ColA) > 0)
                    {
                        validMoves |= downLeft;
                        downLeftStopped = true;
                    }
                }
                
                if (!downRightStopped)
                {
                    downRight >>= 9;

                    if ((downRight & blackPieces) > 0) downRightStopped = true;
                    else if ((downRight & whitePieces) > 0 || (downRight & BoardPositionalInfo.Row1) > 0 || (downRight & BoardPositionalInfo.ColH) > 0)
                    {
                        validMoves |= downRight;
                        downRightStopped = true;
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
            }
            
            return validMoves;
        }

        public override ulong GetValidMoves(BoardState state, ulong piecePosition)
        {
            return ValidMoves(state, piecePosition);
        }

    }
}