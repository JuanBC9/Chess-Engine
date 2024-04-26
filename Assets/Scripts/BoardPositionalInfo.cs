using UnityEngine;

namespace Assets.Sprites
{
    public static class BoardPositionalInfo
    {
        #region Rows

        /*
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        1  1  1  1  1  1  1  1
        */
        public static ulong Row1 = 255;
        
        /*
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        1  1  1  1  1  1  1  1
        0  0  0  0  0  0  0  0
        */
        public static ulong Row2 = 65280;

        /*
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        1  1  1  1  1  1  1  1
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        */
        public static ulong Row3 = 16711680;
        
        /*
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        1  1  1  1  1  1  1  1
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        */
        public static ulong Row4 = 4278190080;
        
        /*
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        1  1  1  1  1  1  1  1
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        */
        public static ulong Row5 = 1095216660480;
        
        /*
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        1  1  1  1  1  1  1  1
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        */
        public static ulong Row6 = 280375465082880;
        
        /*
        0  0  0  0  0  0  0  0
        1  1  1  1  1  1  1  1
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        */
        public static ulong Row7 = 71776119061217280;
        
        /*
        1  1  1  1  1  1  1  1
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        0  0  0  0  0  0  0  0
        */
        public static ulong Row8 = 18374686479671623680;

        #endregion
        
        #region Columns

        /*
        1  0  0  0  0  0  0  0
        1  0  0  0  0  0  0  0
        1  0  0  0  0  0  0  0
        1  0  0  0  0  0  0  0
        1  0  0  0  0  0  0  0
        1  0  0  0  0  0  0  0
        1  0  0  0  0  0  0  0
        1  0  0  0  0  0  0  0
        */
        public static ulong ColA = 9259542123273814144;

        /*
        0  1  0  0  0  0  0  0
        0  1  0  0  0  0  0  0
        0  1  0  0  0  0  0  0
        0  1  0  0  0  0  0  0
        0  1  0  0  0  0  0  0
        0  1  0  0  0  0  0  0
        0  1  0  0  0  0  0  0
        0  1  0  0  0  0  0  0
        */
        public static ulong ColB = 4629771061636907072;

        /*
        0  0  1  0  0  0  0  0
        0  0  1  0  0  0  0  0
        0  0  1  0  0  0  0  0
        0  0  1  0  0  0  0  0
        0  0  1  0  0  0  0  0
        0  0  1  0  0  0  0  0
        0  0  1  0  0  0  0  0
        0  0  1  0  0  0  0  0
        */
        public static ulong ColC = 2314885530818453536;

        /*
        0  0  0  1  0  0  0  0
        0  0  0  1  0  0  0  0
        0  0  0  1  0  0  0  0
        0  0  0  1  0  0  0  0
        0  0  0  1  0  0  0  0
        0  0  0  1  0  0  0  0
        0  0  0  1  0  0  0  0
        0  0  0  1  0  0  0  0
        */
        public static ulong ColD = 1157442765409226768;

        /*
        0  0  0  0  1  0  0  0
        0  0  0  0  1  0  0  0
        0  0  0  0  1  0  0  0
        0  0  0  0  1  0  0  0
        0  0  0  0  1  0  0  0
        0  0  0  0  1  0  0  0
        0  0  0  0  1  0  0  0
        0  0  0  0  1  0  0  0
        */
        public static ulong ColE = 578721382704613384;

        /*
        0  0  0  0  0  1  0  0
        0  0  0  0  0  1  0  0
        0  0  0  0  0  1  0  0
        0  0  0  0  0  1  0  0
        0  0  0  0  0  1  0  0
        0  0  0  0  0  1  0  0
        0  0  0  0  0  1  0  0
        0  0  0  0  0  1  0  0
        */
        public static ulong ColF = 289360691352306692;

        /*
        0  0  0  0  0  0  1  0
        0  0  0  0  0  0  1  0
        0  0  0  0  0  0  1  0
        0  0  0  0  0  0  1  0
        0  0  0  0  0  0  1  0
        0  0  0  0  0  0  1  0
        0  0  0  0  0  0  1  0
        0  0  0  0  0  0  1  0
        */
        public static ulong ColG = 144680345676153346;

        /*
        0  0  0  0  0  0  0  1
        0  0  0  0  0  0  0  1
        0  0  0  0  0  0  0  1
        0  0  0  0  0  0  0  1
        0  0  0  0  0  0  0  1
        0  0  0  0  0  0  0  1
        0  0  0  0  0  0  0  1
        0  0  0  0  0  0  0  1
        */
        public static ulong ColH = 72340172838076673;

        #endregion

        public static ulong OuterEdge => Row1 | Row8 | ColA | ColH;
        public static ulong SecondOuterEdge => Row2 | Row7 | ColB | ColG;

        public static ulong GetRow(int row)
        {
            return row switch
            {
                1 => Row1,
                2 => Row2,
                3 => Row3,
                4 => Row4,
                5 => Row5,
                6 => Row6,
                7 => Row7,
                8 => Row8,
                _ => 0
            };
        }
        
        public static ulong GetCol(int col)
        {
            return col switch
            {
                1 => ColA,
                2 => ColB,
                3 => ColC,
                4 => ColD,
                5 => ColE,
                6 => ColF,
                7 => ColG,
                8 => ColH,
                _ => 0
            };
        }

        public static Vector2Int GetCoords(ulong position)
        {
            int row = 0;
            int col = 0;

            for (int i = 1; i <= 8; i++)
            {
                if ((GetRow(i) & position) > 0) row = i;
                if ((GetCol(i) & position) > 0) col = i;

                if (row > 0 & col > 0) break;
            }

            return new Vector2Int(row - 1, col - 1);
        }

    }
}