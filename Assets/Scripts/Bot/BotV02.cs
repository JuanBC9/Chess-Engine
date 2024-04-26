using Assets;
using Assets.Scripts;
using Assets.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class BotV02 : ChessBot
{
    Func<BoardState, IEnumerable<Move>> GetAllValidMoves;
    Dictionary<BoardState, IEnumerable<Move>> evaluationTable;
    DateTime initTime;
    const int threadCount = 8;

    public BotV02(Func<BoardState, IEnumerable<Move>> getAllValidMoves)
    {
        GetAllValidMoves = getAllValidMoves;
        evaluationTable = new Dictionary<BoardState, IEnumerable<Move>>();
    }

    private BoardState CreateBoardState(BoardState prevState, Move move)
    {
        var newBoardState = new BoardState(prevState).UpdateState(move);

        if (!evaluationTable.ContainsKey(newBoardState))
        {
            evaluationTable.Add(newBoardState, GetAllValidMoves(newBoardState));
        }

        return newBoardState;
    }

    public override Move GetBestMove(BoardState state, int depth, int maxTime)
    {
        initTime = DateTime.Now;

        var moves = GetAllValidMoves(state);

        Task<Move>[] tasks = new Task<Move>[threadCount];
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
            tasks[i] = MiniMax(state, dividedMoves[i], depth, state.WhiteMoves, float.MinValue, float.MaxValue, maxTime);
        }

        Task.WaitAll(tasks);

        Debug.Log((DateTime.Now - initTime).TotalMilliseconds.ToString());

        if (state.WhiteMoves)
        {
            float maxValue = tasks.Max(t => t.Result.Evaluation);
            return tasks.First(t => t.Result.Evaluation == maxValue).Result;
        }
        else
        {
            float minValue = tasks.Min(t => t.Result.Evaluation);
            return tasks.First(t => t.Result.Evaluation == minValue).Result;
        }
    }

    public async Task<Move> MiniMax(BoardState state, IEnumerable<Move> moves, int depth, bool maximize, float alpha, float beta, int maxTime)
    {
        Move bestMove = moves.First();

        if (maximize)
        {
            float bestVal = float.MinValue;

            foreach (Move m in moves)
            {
                if((DateTime.Now - initTime).TotalMilliseconds > maxTime) return bestMove;


                var newState = CreateBoardState(state, m);
                m.Evaluation = await MiniMax(newState, depth - 1, false, alpha, beta, maxTime);

                if (m.Evaluation > bestVal)
                {
                    bestVal = m.Evaluation;
                    bestMove = m;
                }

                alpha = MathF.Max(alpha, bestVal);
                if (beta <= alpha)
                {
                    break;
                }
            }
            return bestMove;
        }
        else
        {
            float bestVal = float.MaxValue;
            foreach (Move m in moves)
            {
                if ((DateTime.Now - initTime).TotalMilliseconds > maxTime) return bestMove;

                var newState = CreateBoardState(state, m);
                m.Evaluation = await MiniMax(newState, depth - 1, true, alpha, beta, maxTime);

                if (m.Evaluation < bestVal)
                {
                    bestVal = m.Evaluation;
                    bestMove = m;
                }

                beta = MathF.Min(beta, bestVal);
                if (beta <= alpha)
                {
                    break;
                }
            }
            return bestMove;
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

        if (state.BlackScore >= Piece.PieceValues[(int)PieceType.WhiteKing] ||
            state.WhiteScore >= Piece.PieceValues[(int)PieceType.BlackKing])
        {
            return EvaluateState(state);
        }

        if (maximize)
        {


            float bestVal = float.MinValue;

            foreach (Move m in evaluationTable[state])
            {
                if ((DateTime.Now - initTime).TotalMilliseconds > maxTime) return bestVal;

                if (m == null)
                {
                    Debug.LogError("move was null ??");
                    continue;
                }

                var newState = CreateBoardState(state, m);
                m.Evaluation = await MiniMax(newState, depth - 1, false, alpha, beta, maxTime);

                bestVal = MathF.Max(m.Evaluation, bestVal);
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
            foreach (Move m in evaluationTable[state])
            {
                if ((DateTime.Now - initTime).TotalMilliseconds > maxTime) return bestVal;

                var newState = CreateBoardState(state, m);
                m.Evaluation = await MiniMax(newState, depth - 1, true, alpha, beta, maxTime);

                bestVal = MathF.Min(m.Evaluation, bestVal);
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
        ulong blackKingPosition = state.Pieces[(int)PieceType.BlackKing];
        Vector2Int coords = BoardPositionalInfo.GetCoords(blackKingPosition);


        return ((blackKingPosition > 0) ? BlackKingPositionTable[coords.x, coords.y] : 0) + (state.WhiteScore - state.BlackScore) + UnityEngine.Random.Range(-1f, 1f);
    }

    private static float[,] BlackKingPositionTable = 
    {
        { 4, 4, 0, 0, -2, 0, 4, 4},
        { 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0},
    };
}
