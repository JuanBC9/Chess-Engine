using Assets.Sprites;

namespace Assets.Pieces
{
    public class BlackBishop : Piece
    {
        public static ulong AttackingSquares(BoardState state, ulong piecePosition) => ValidMoves(state, piecePosition);
        public static ulong ValidMoves(BoardState state, ulong piecePosition)
        {
            ulong validMoves = 0;

            if (state == null) return validMoves;

            // Movimientos posibles

            // Los movimientos posibles del alfil son basicamente las diagonales de la posición en la que se encuentra 
            // hasta que se encuentra con una pieza o hasta que llega al borde del tablero

            ulong topLeft = piecePosition << 9;
            ulong topRight = piecePosition << 7;
            ulong bottomLeft = piecePosition >> 7;
            ulong bottomRight = piecePosition >> 9;


            bool topLeftStopped = (piecePosition & BoardPositionalInfo.ColA) > 0 || (piecePosition & BoardPositionalInfo.Row8) > 0;
            bool topRightStopped = (piecePosition & BoardPositionalInfo.ColH) > 0 || (piecePosition & BoardPositionalInfo.Row8) > 0;
            bool bottomLeftStopped = (piecePosition & BoardPositionalInfo.ColA) > 0 || (piecePosition & BoardPositionalInfo.Row1) > 0;
            bool bottomRightStopped = (piecePosition & BoardPositionalInfo.ColH) > 0 || (piecePosition & BoardPositionalInfo.Row1) > 0;

            while (!topLeftStopped || !topRightStopped || !bottomLeftStopped || !bottomRightStopped)
            {
                //Check if there is a piece
                for (int i = 0; i < state.Pieces.Length; i++)
                {
                    var pieces = state.Pieces[i];

                    if (i % 2 == 0) //Black pieces
                    {
                        if (!topLeftStopped && (topLeft & pieces) > 0) topLeftStopped = true;
                        if (!topRightStopped && (topRight & pieces) > 0) topRightStopped = true;
                        if (!bottomLeftStopped && (bottomLeft & pieces) > 0) bottomLeftStopped = true;
                        if (!bottomRightStopped && (bottomRight & pieces) > 0) bottomRightStopped = true;
                    }
                    else //white pieces
                    {
                        if (!topLeftStopped && (topLeft & pieces) > 0)
                        {
                            validMoves |= topLeft;
                            topLeftStopped = true;
                        }

                        if (!topRightStopped && (topRight & pieces) > 0)
                        {
                            validMoves |= topRight;
                            topRightStopped = true;
                        }

                        if (!bottomLeftStopped && (bottomLeft & pieces) > 0)
                        {
                            validMoves |= bottomLeft;
                            bottomLeftStopped = true;
                        }

                        if (!bottomRightStopped && (bottomRight & pieces) > 0)
                        {
                            validMoves |= bottomRight;
                            bottomRightStopped = true;
                        }
                    }
                }

                //Check if we are in the edge
                if (!topLeftStopped && (topLeft & BoardPositionalInfo.OuterEdge) > 0)
                {
                    validMoves |= topLeft;
                    topLeftStopped = true;
                }

                if (!topRightStopped && (topRight & BoardPositionalInfo.OuterEdge) > 0)
                {
                    validMoves |= topRight;
                    topRightStopped = true;
                }

                if (!bottomLeftStopped && (bottomLeft & BoardPositionalInfo.OuterEdge) > 0)
                {
                    validMoves |= bottomLeft;
                    bottomLeftStopped = true;
                }

                if (!bottomRightStopped && (bottomRight & BoardPositionalInfo.OuterEdge) > 0)
                {
                    validMoves |= bottomRight;
                    bottomRightStopped = true;
                }

                if (!topLeftStopped)
                {
                    validMoves |= topLeft;
                    topLeft <<= 9;
                }

                if (!topRightStopped)
                {
                    validMoves |= topRight;
                    topRight <<= 7;
                }

                if (!bottomLeftStopped)
                {
                    validMoves |= bottomLeft;
                    bottomLeft >>= 7;
                }

                if (!bottomRightStopped)
                {
                    validMoves |= bottomRight;
                    bottomRight >>= 9;
                }
            }


            return validMoves;
        }
        public override ulong GetValidMoves(BoardState state, ulong piecePosition)
        {
            return ValidMoves(state, piecePosition);
        }
    }
}