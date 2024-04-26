using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public abstract class ChessBot
    {
        public abstract Move GetBestMove(BoardState state, int depth, int maxTime);

    }
}
