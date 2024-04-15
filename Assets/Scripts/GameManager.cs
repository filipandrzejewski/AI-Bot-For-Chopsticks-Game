using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameState
{
    public bool playerTurn;
    public int[] state;
    public int score = 0;

    public GameState(bool _playerTurn, int[] _state) 
    {
        playerTurn = _playerTurn;
        state = _state;
    }

    // Turn this function into recursive. Be sure to stop it at certain depth because it could go forever.
    // It maybe faster to check for a loop at some point, maybe it will extend the depth it will be able to look into.
    public List<GameState> GetMoves() 
    {
        if (!playerTurn) 
        {
            state = new int[] { state[2], state[3], state[0], state[1] };
        }
        List<GameState> output = new List<GameState>();
        if (state[0] != 0)
        {
            if (state[2] != 0) 
            {
                output.Add(new GameState(!playerTurn, new int[]
                {
                    state[0], state[1], state[2] + state[0], state[3]
                }
                ));
            }
            if (state[3] != 0) 
            {
                output.Add(new GameState(!playerTurn, new int[]
                {
                    state[0], state[1], state[2], state[3] + state[0]
                }
                ));
            }
        }
        else if (state[1] % 2 == 0)
        {
            output.Add(new GameState(!playerTurn, new int[]
            {
                state[1]/2, state[1]/2, state[2], state[3]
            }
            ));
        }

        if (state[1] != 0 & state[1] != state[0]) 
        {
            if (state[2] != 0)
            {
                output.Add(new GameState(!playerTurn, new int[]
                {
                    state[0], state[1], state[2] + state[1], state[3]
                }
                ));
            }
            if (state[3] != 0)
            {
                output.Add(new GameState(!playerTurn, new int[]
                {
                    state[0], state[1], state[2], state[3] + state[1]
                }
                ));
            }
        }
        else if (state[0] % 2 == 0)
        {
            output.Add(new GameState(!playerTurn, new int[]
            {
                state[0]/2, state[0]/2, state[2], state[3]
            }
            ));
        }

        return output;
    }

    public (List<GameState>, int) GetMovesRecursive(int depth)
    {
        this.score = GradeScore();
        if (depth <= 0)
        {
            return (null, score);
        }


        if (!playerTurn)
        {
            state = new int[] { state[2], state[3], state[0], state[1] };
        }

        List<GameState> output = new List<GameState>();
        if (state[0] != 0)
        {
            if (state[2] != 0)
            {
                output.Add(new GameState(!playerTurn, new int[]
                {
                    state[0], state[1], state[2] + state[0], state[3]
                }
                ));
            }
            if (state[3] != 0)
            {
                output.Add(new GameState(!playerTurn, new int[]
                {
                    state[0], state[1], state[2], state[3] + state[0]
                }
                ));
            }
        }
        else if (state[1] % 2 == 0)
        {
            output.Add(new GameState(!playerTurn, new int[]
            {
                state[1]/2, state[1]/2, state[2], state[3]
            }
            ));
        }

        if (state[1] != 0 & state[1] != state[0])
        {
            if (state[2] != 0)
            {
                output.Add(new GameState(!playerTurn, new int[]
                {
                    state[0], state[1], state[2] + state[1], state[3]
                }
                ));
            }
            if (state[3] != 0)
            {
                output.Add(new GameState(!playerTurn, new int[]
                {
                    state[0], state[1], state[2], state[3] + state[1]
                }
                ));
            }
        }
        else if (state[0] % 2 == 0)
        {
            output.Add(new GameState(!playerTurn, new int[]
            {
                state[0]/2, state[0]/2, state[2], state[3]
            }
            ));
        }
        

        for (int i = 0; i < output.Count; i ++ )
        {
            output[i].score
        }
        return output;
    }

    public int GradeScore() 
    {
        return 0;
    }


    public override string ToString()
    {
        return $"{state[0]}, {state[1]}, {state[2]}, {state[3]} - score: {score}";
    }
}

public class GameManager : MonoBehaviour
{
    public Transform[] playerHands;
    public Transform[] opponentHands;
    public GameState gameState;

    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        
    }

    void Start()
    {
        gameState = new GameState(true, new int[] { 1, 1, 1, 1 });
        StartPlayerTurn();

        List<GameState> possibleGameStates = gameState.GetMoves();
        for (int i = 0; i < possibleGameStates.Count; i++)
        {
            Debug.Log(possibleGameStates[i].ToString());
        }
        
    }

    void RecursiveTest(int depth)
    {
        if (depth == 0)
        {
            return;
        }
        

    }

    void Update()
    {
        if (gameState.playerTurn == false)
        {
            StartPlayerTurn();
        }
        
    }

    public void StartPlayerTurn()
    {
        gameState.playerTurn = true;
        for (int i = 0; i < playerHands.Length; i++)
        {
            playerHands[i].GetComponent<Hand>().SetCollider2D(true);
        }

        UpdateGameState();
    }

    public void UpdateGameState()
    {
        int index = 0;
        for (int i = 0; i < playerHands.Length; i++, index ++)
        {
            gameState.state[index] = playerHands[i].GetComponent<Hand>().state;
        }
        for (int i = 0; i < opponentHands.Length; i++, index++)
        {
            gameState.state[index] = opponentHands[i].GetComponent<Hand>().state;
        }

        PrintGameState();

        List<GameState> possibleGameStates = gameState.GetMoves();
        for (int i = 0; i < possibleGameStates.Count; i++)
        {
            Debug.Log(possibleGameStates[i].ToString());
        }

    }

    /*
    public int[,] CalculatePossibleStates(int[] currentState)
    {
        int[,] possibilities;
        for (int i = 0; i < currentState.Length; i++)
        {
            possibilities +=
        }
    }
    */

    public void EndPlayerTurn()
    {
        gameState.playerTurn = false;

        for (int i = 0; i < playerHands.Length; i++)
        {
            playerHands[i].GetComponent<Hand>().SetCollider2D(false);
        }

        UpdateGameState();

        StartOpponentTurn();
    }

    public void StartOpponentTurn()
    {

    }

    public void PrintGameState()
    {
        string print = "Turn ended. Current State: ";
        for (int i = 0; i < gameState.state.Length; i++)
        {

            print += gameState.state[i];
        }

        Debug.Log(print);
    }

}
