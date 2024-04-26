using Assets.Sprites;

namespace Assets.Pieces
{
    public class BlackRook : Piece
    {
        public static ulong AttackingSquares(BoardState state, ulong piecePosition) => ValidMoves(state, piecePosition);

        public static ulong ValidMoves(BoardState state, ulong piecePosition)
        {
            ulong validMoves = 0;

            if (state == null) return validMoves;

            // Movimientos posibles
            
            // Los movimientos posibles de la torre son la vertical y horizontal en la que se encuentra
            // hasta que se encuentra con una pieza o hasta que llega al borde del tablero

            
            var blackPieces = state.Pieces[0] | state.Pieces[2] | state.Pieces[4] | state.Pieces[6] | state.Pieces[8] | state.Pieces[10];
            var whitePieces = state.Pieces[1] | state.Pieces[3] | state.Pieces[5] | state.Pieces[7] | state.Pieces[9] | state.Pieces[11];
            
            ulong up = piecePosition;
            ulong down = piecePosition;
            ulong left = piecePosition;
            ulong right = piecePosition;

            bool upStopped = false, downStopped = false, leftStopped = false, rightStopped = false;

            if ((up & BoardPositionalInfo.Row8) > 0) upStopped = true;
            if ((down & BoardPositionalInfo.Row1) > 0) downStopped = true;
            if ((left & BoardPositionalInfo.ColA) > 0) leftStopped = true;
            if ((right & BoardPositionalInfo.ColH) > 0) rightStopped = true;
            
            
            while (!upStopped || !downStopped || !leftStopped || !rightStopped)
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

                if (!upStopped) validMoves |= up;
                if (!downStopped) validMoves |= down;
                if (!leftStopped) validMoves |= left;
                if (!rightStopped) validMoves |= right;
            }
            
            return validMoves;
        }

        public override ulong GetValidMoves(BoardState state, ulong piecePosition)
        {
            return ValidMoves(state, piecePosition);
        }

    }
}