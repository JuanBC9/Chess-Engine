using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public Board Board;
    private ChessBot _bot;
    private GameState _gameState;

    private void Start()
    {
        Board.Init();        
        _bot = new BotV02(Board.GetAllValidMoves);

        _gameState = new GameState()
        {
            WhiteScore = 0,
            BlackScore = 0,
            GameFinished = false,
            FinishValue = 0,
        };


        StartCoroutine(Game());
    }

    private IEnumerator Game()
    {
        while (Application.isPlaying)
        {
            //Check game state
            Board.GetGameState(ref _gameState);

            Debug.Log($"Score: White {_gameState.WhiteScore}, Black {_gameState.BlackScore}");

            if (_gameState.GameFinished)
            {
                if (_gameState.FinishValue == 1) Debug.Log($"Game Finished!!, White Won");
                if (_gameState.FinishValue == 0) Debug.Log($"Game Finished!!, Draw");
                if (_gameState.FinishValue == -1) Debug.Log($"Game Finished!!, Black Won");
                break;
            }

            //White Turn
            //var move = _bot2.GetBestMove(Board.BoardState, 4, 10000);
            //Board.MakeMove(move);


            yield return new WaitUntil(() => !Board.BoardState.WhiteMoves);
            yield return new WaitForSeconds(.5f);

            //Check game state
            Board.GetGameState(ref _gameState);

            if (_gameState.GameFinished)
            {
                Debug.Log($"Score: White {_gameState.WhiteScore}, Black {_gameState.BlackScore}");
                if (_gameState.FinishValue == 1) Debug.Log($"Game Finished!!, White Won");
                if (_gameState.FinishValue == 0) Debug.Log($"Game Finished!!, Draw");
                if (_gameState.FinishValue == -1) Debug.Log($"Game Finished!!, Black Won");
                break;
            }


            //Black Turn
            var move1 = _bot.GetBestMove(Board.BoardState, 4, 10000);
            Board.MakeMove(move1);

            yield return new WaitUntil(() => Board.BoardState.WhiteMoves);
        }

        SceneManager.LoadScene(0);
    }
}

public class GameState
{
    public float WhiteScore;
    public float BlackScore;
    public bool GameFinished;
    public int FinishValue; // 0 = Draw, 1 = White won, -1 = black won
}
