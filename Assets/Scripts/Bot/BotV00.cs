using Assets;
using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class BotV00 : ChessBot
{
    Func<BoardState, IEnumerable<Move>> GetAllValidMoves;

    private Move BestMove;

    public BotV00(Func<BoardState, IEnumerable<Move>> getAllValidMoves)
    {
        GetAllValidMoves = getAllValidMoves;
    }


    public override Move GetBestMove(BoardState state, int depth, int maxTime)
    {
        Task <float> t = MiniMax(new BoardState(state), depth, state.WhiteMoves, float.MinValue, float.MaxValue, true);

        Task.WaitAll(t);

        return BestMove;
    }

    public async Task<float> MiniMax(BoardState state, int depth, bool maximize, float alpha, float beta, bool asignMove)
    {
        if (!Application.isPlaying)
        {
            return 0;
        }

        if (depth == 0)
        {
            return EvaluateState(state);
        }

        if (maximize)
        {
            float bestVal = float.MinValue;

            foreach (Move m in GetAllValidMoves(state))
            {
                var newState = new BoardState(state).UpdateState(m.PieceType, m.Tile1, m.Tile2, m.NewPieceType);
                var value = await MiniMax(newState, depth - 1, false, alpha, beta, false);

                if (value > bestVal)
                {
                    bestVal = value;
                    if (asignMove)
                        BestMove = m;
                }

                alpha = MathF.Max(alpha, bestVal);
                if (beta <= alpha)
                {
                    break;
                }
            }
            return bestVal;
        }
        else
        {
            float bestVal = float.MaxValue;
            foreach (Move m in GetAllValidMoves(state))
            {
                var newState = new BoardState(state).UpdateState(m.PieceType, m.Tile1, m.Tile2, m.NewPieceType);
                var value = await MiniMax(newState, depth - 1, true, alpha, beta, false);

                if (value < bestVal)
                {
                    bestVal = value;
                    if (asignMove) 
                        BestMove = m;
                }

                beta = MathF.Min(beta, bestVal);
                if (beta <= alpha)
                {
                    break;
                }
            }
            return bestVal;
        }
    }


    private float EvaluateState(BoardState state)
    {
        return (state.WhiteScore - state.BlackScore) + UnityEngine.Random.Range(-.5f, .5f);
    }
}

public class Move
{
    public PieceType PieceType;
    public ulong Tile1;
    public ulong Tile2;
    public PieceType NewPieceType;
    public float Evaluation;
}
