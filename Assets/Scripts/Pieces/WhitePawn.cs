using Assets.Sprites;

namespace Assets.Pieces
{
    public class WhitePawn : Piece
    {
        public static ulong AttackingSquares(BoardState state, ulong piecePosition)
        {
            ulong attackingSquares = 0;

            if ((piecePosition & BoardPositionalInfo.Row1) > 0) return 0;

            if ((piecePosition & BoardPositionalInfo.ColA) <= 0) // Si no estoy en el extremo izquierdo
            {
                var topLeft = piecePosition << 9;
                attackingSquares |= topLeft;
            }
            
            if ((piecePosition & BoardPositionalInfo.ColH) <= 0) // Si no estoy en el extremo derecho
            {
                var topRight = piecePosition << 7;
                attackingSquares |= topRight;
            }
            
            attackingSquares &= ~state.allPieces;
            
            return attackingSquares;
        }

        public static ulong ValidMoves(BoardState state, ulong piecePosition)
        {
            ulong validMoves = 0;

            if (state == null) return validMoves;

            // Movimientos posibles

            // - piecePosition << 8 (Avanzar hacia alante una fila)
            //      - Solo si no hay una ficha en piecePosition << 8

            // - piecePosition << 16 (Avanzar hacia alante 2 filas)
            //      - Si esta en la segunda fila
            //      - Si no hay una ficha en piecePosition << 8
            //      - Si no hay una ficha en piecePosition << 16

            // - piecePosition << 9 (Comer hacia alante izquierda)
            //      - Si hay una ficha negra en piecePosition << 9
            //      - Si no estamos en la columna izquierda
            //      - Si hay enpassant en piecePosition << 9

            // - piecePosition << 7 (Comer hacia alante derecha)
            //      - Si hay una ficha negra en piecePosition << 7
            //      - Si no estamos en la columna derecha
            //      - Si hay enpassant en piecePosition << 7

            ulong advance = piecePosition << 8;
            ulong advance2 = piecePosition << 16;
            ulong takeLeft = piecePosition << 9;
            ulong takeRight = piecePosition << 7;

            bool canAdvance = true,
                 canAdvance2 = true,
                 canTakeLeft = false,
                 canTakeRight = false;


            if (state.EnPassantSquare == takeLeft && (state.EnPassantSquare & BoardPositionalInfo.Row6) > 0) canTakeLeft = true;
            if (state.EnPassantSquare == takeRight && (state.EnPassantSquare & BoardPositionalInfo.Row6) > 0) canTakeRight = true;
            if ((piecePosition & BoardPositionalInfo.Row2) == 0) canAdvance2 = false;

            for (int i = 0; i < state.Pieces.Length; i++)
            {
                var pieces = state.Pieces[i];

                if (canAdvance && ((pieces & advance) > 0))
                {
                    canAdvance = false;
                    canAdvance2 = false;
                }


                if (canAdvance2 && ((pieces & advance) > 0 || (pieces & advance2) > 0))
                {
                    canAdvance2 = false;
                }


                if (i % 2 == 0) // blackPieces 
                {
                    if (!canTakeLeft && (pieces & takeLeft) > 0)
                    {
                        canTakeLeft = true;
                    }

                    if (!canTakeRight && (pieces & takeRight) > 0)
                    {
                        canTakeRight = true;
                    }
                }
            }

            if ((piecePosition & BoardPositionalInfo.ColH) > 0) canTakeRight = false;
            if ((piecePosition & BoardPositionalInfo.ColA) > 0) canTakeLeft = false;

            if (canAdvance) { validMoves |= advance; }
            if (canAdvance2) { validMoves |= advance2; }
            if (canTakeLeft) { validMoves |= takeLeft; }
            if (canTakeRight) {  validMoves |= takeRight; }


            return validMoves;
        }

        public override ulong GetValidMoves(BoardState state, ulong piecePosition)
        {
            return ValidMoves(state, piecePosition);
        }
        public override ulong GetAttackingSquares(BoardState state, ulong piecePosition) => AttackingSquares(state, piecePosition);
    }
}
