using Assets;
using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class BotV01 : ChessBot
{
    Func<BoardState, IEnumerable<Move>> GetAllValidMoves;
    Dictionary<BoardState, float> evaluationTable;
    DateTime initTime;
    const int threadCount = 8;

    public BotV01(Func<BoardState, IEnumerable<Move>> getAllValidMoves)
    {
        GetAllValidMoves = getAllValidMoves;
        evaluationTable = new Dictionary<BoardState, float>();
    }

    public override Move GetBestMove(BoardState state, int depth, int maxTime)
    {
        initTime = DateTime.Now;

        var moves = GetAllValidMoves(state);

        Task<(float,Move)>[] tasks = new Task<(float, Move)>[threadCount];
        List<Move>[] dividedMoves = new List<Move>[threadCount];
        
        for (int i = 0; i < threadCount; i++)
        {
            dividedMoves[i] = new List<Move>();
        };

        for (int i = 0; i < moves.Count(); i++)
        {
            int j = i % threadCount;

            dividedMoves[j].Add(moves.ElementAt(i));
        }

        for (int i = 0; i < threadCount; i++)
        {
            tasks[i] = MiniMax(new BoardState(state), dividedMoves[i], depth, state.WhiteMoves, float.MinValue, float.MaxValue, maxTime);
        }

        Task.WaitAll(tasks);

        Debug.Log((DateTime.Now - initTime).TotalMilliseconds.ToString());

        if (state.WhiteMoves)
        {
            float maxValue = tasks.Max(t => t.Result.Item1);
            return tasks.First(t => t.Result.Item1 == maxValue).Result.Item2;
        }
        else
        {
            float minValue = tasks.Min(t => t.Result.Item1);
            return tasks.First(t => t.Result.Item1 == minValue).Result.Item2;
        }
    }

    public async Task<(float, Move)> MiniMax(BoardState state, IEnumerable<Move> moves, int depth, bool maximize, float alpha, float beta, int maxTime)
    {
        Move bestMove = moves.First();

        if (maximize)
        {
            float bestVal = float.MinValue;

            foreach (Move m in moves)
            {
                if((DateTime.Now - initTime).TotalMilliseconds > maxTime) return (bestVal, bestMove);

                var newState = new BoardState(state).UpdateState(m.PieceType, m.Tile1, m.Tile2, m.NewPieceType);
                var value = await MiniMax(newState, depth - 1, false, alpha, beta, maxTime);

                if (value > bestVal)
                {
                    bestVal = value;
                    bestMove = m;
                }

                alpha = MathF.Max(alpha, bestVal);
                if (beta <= alpha)
                {
                    break;
                }
            }
            return (bestVal, bestMove);
        }
        else
        {
            float bestVal = float.MaxValue;
            foreach (Move m in moves)
            {
                if ((DateTime.Now - initTime).TotalMilliseconds > maxTime) return (bestVal, bestMove);

                var newState = new BoardState(state).UpdateState(m.PieceType, m.Tile1, m.Tile2, m.NewPieceType);
                var value = await MiniMax(newState, depth - 1, true, alpha, beta, maxTime);

                if (value < bestVal)
                {
                    bestVal = value;
                    bestMove = m;
                }

                beta = MathF.Min(beta, bestVal);
                if (beta <= alpha)
                {
                    break;
                }
            }
            return (bestVal, bestMove);
        }

    }

    public async Task<float> MiniMax(BoardState state, int depth, bool maximize, float alpha, float beta, int maxTime)
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
                if ((DateTime.Now - initTime).TotalMilliseconds > maxTime) return bestVal;

                if (m == null)
                {
                    Debug.LogError("move was null ??");
                    continue;
                }

                var newState = new BoardState(state).UpdateState(m.PieceType, m.Tile1, m.Tile2, m.NewPieceType);
                var value = await MiniMax(newState, depth - 1, false, alpha, beta, maxTime);

                bestVal = MathF.Max(value, bestVal);
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
                if ((DateTime.Now - initTime).TotalMilliseconds > maxTime) return bestVal;

                var newState = new BoardState(state).UpdateState(m.PieceType, m.Tile1, m.Tile2, m.NewPieceType);
                var value = await MiniMax(newState, depth - 1, true, alpha, beta, maxTime);

                bestVal = MathF.Min(value, bestVal);
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
