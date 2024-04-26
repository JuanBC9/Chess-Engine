using Assets.Sprites;

namespace Assets.Pieces
{
    public class WhiteHorse : Piece
    {
        public static ulong AttackingSquares(BoardState state, ulong piecePosition) => ValidMoves(state, piecePosition);

        public static ulong ValidMoves(BoardState state, ulong piecePosition)
        {
            ulong validMoves = 0;

            if (state == null) return validMoves;
            
            // Movimientos posibles
            
            // piecePosition << 17 (2 alante, 1 izquierda)
            //      - Si no hay una pieza negra en esa posición
            //      - Si no estamos en la fila mas izquierda
            
            // piecePosition << 15 (2 alante, 1 derecha)
            //      - Si no hay una pieza negra en esa posición
            //      - Si no estamos en la fila mas derecha
            
            // piecePosition >> 17 (2 atras, 1 izquierda)
            //      - Si no hay una pieza negra en esa posición
            //      - Si no estamos en la fila mas izquierda
            
            // piecePosition >> 15 (2 atras, 1 derecha)
            //      - Si no hay una pieza negra en esa posición
            //      - Si no estamos en la fila mas derecha
            
            
            // piecePosition << 10 (1 alante, 2 izquierda)
            //      - Si no hay una pieza negra en esa posición
            //      - Si no estamos en la fila mas izquierda
            //      - Si no estamos en la segunda fila mas izquierda
            
            // piecePosition << 6 (1 alante, 2 derecha)
            //      - Si no hay una pieza negra en esa posición
            //      - Si no estamos en la fila mas derecha
            //      - Si no estamos en la segunda fila mas derecha

            // piecePosition >> 6 (1 atras, 2 izquierda)
            //      - Si no hay una pieza negra en esa posición
            //      - Si no estamos en la fila mas izquierda
            //      - Si no estamos en la segunda fila mas izquierda
            
            // piecePosition >> 10 (1 atras, 2 derecha)
            //      - Si no hay una pieza negra en esa posición
            //      - Si no estamos en la fila mas derecha
            //      - Si no estamos en la segunda fila mas derecha

            ulong f2i1 = piecePosition << 17;
            ulong f2d1 = piecePosition << 15;
            ulong b2i1 = piecePosition >> 15;
            ulong b2d1 = piecePosition >> 17;

            ulong f1i2 = piecePosition << 10;
            ulong f1d2 = piecePosition << 6;
            ulong b1i2 = piecePosition >> 6;
            ulong b1d2 = piecePosition >> 10;

            bool canf2i1 = true;
            bool canf2d1 = true;
            bool canb2i1 = true;
            bool canb2d1 = true;
            
            bool canf1i2 = true;
            bool canf1d2 = true;
            bool canb1i2 = true;
            bool canb1d2 = true;
            
            //Si estamos en la columna más a la izquierda
            if ((piecePosition & BoardPositionalInfo.ColA) > 0)
            {
                canf2i1 = false;
                canb2i1 = false;
                canf1i2 = false;
                canb1i2 = false;
            }
            
            //Si estamos en la columna más a la derecha
            if ((piecePosition & BoardPositionalInfo.ColH) > 0)
            {
                canf2d1 = false;
                canb2d1 = false;
                canf1d2 = false;
                canb1d2 = false;
            }
            
            //Si estamos en la segunda columna más a la izquierda
            if ((piecePosition & BoardPositionalInfo.ColB) > 0)
            {
                canf1i2 = false;
                canb1i2 = false;
            }
            
            //Si estamos en la segunda columna más a la derecha
            if ((piecePosition & BoardPositionalInfo.ColG) > 0)
            {
                canf1d2 = false;
                canb1d2 = false;
            }

            //Si estamos en la primera fila
            if ((piecePosition & BoardPositionalInfo.Row1) > 0)
            {
                canb2i1 = false;
                canb2d1 = false;
                canb1i2 = false;
                canb1d2 = false;
            }
            
            //Si estamos en la última fila
            if ((piecePosition & BoardPositionalInfo.Row8) > 0)
            {
                canf2i1 = false;
                canf2d1 = false;
                canf1i2 = false;
                canf1d2 = false;
            }
            
            //Si estamos en la segunda fila
            if ((piecePosition & BoardPositionalInfo.Row2) > 0)
            {
                canb2i1 = false;
                canb2d1 = false;
            }
            
            //Si estamos en la penúltima fila
            if ((piecePosition & BoardPositionalInfo.Row7) > 0)
            {
                canf2i1 = false;
                canf2d1 = false;
            }

            for (int i = 0; i < state.Pieces.Length; i++)
            {
                var pieces = state.Pieces[i];
                
                //white pieces
                if (i % 2 != 0)
                {
                    if (canf2i1 && (pieces & f2i1) > 0) canf2i1 = false;
                    if (canf2d1 && (pieces & f2d1) > 0) canf2d1 = false;
                    if (canb2i1 && (pieces & b2i1) > 0) canb2i1 = false;
                    if (canb2d1 && (pieces & b2d1) > 0) canb2d1 = false;
                    
                    if (canf1i2 && (pieces & f1i2) > 0) canf1i2 = false;
                    if (canf1d2 && (pieces & f1d2) > 0) canf1d2 = false;
                    if (canb1i2 && (pieces & b1i2) > 0) canb1i2 = false;
                    if (canb1d2 && (pieces & b1d2) > 0) canb1d2 = false;
                }
            }
            
            if (canf2i1) { validMoves |= f2i1; }
            if (canf2d1) { validMoves |= f2d1; }
            if (canb2i1) { validMoves |= b2i1; }
            if (canb2d1) { validMoves |= b2d1; }
            
            if (canf1i2) { validMoves |= f1i2; }
            if (canf1d2) { validMoves |= f1d2; }
            if (canb1i2) { validMoves |= b1i2; }
            if (canb1d2) { validMoves |= b1d2; }

            return validMoves;
        }

        public override ulong GetValidMoves(BoardState state, ulong piecePosition)
        {
            return ValidMoves(state, piecePosition);
        }
    }
}