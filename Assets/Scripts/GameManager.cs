using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameState
{
    public bool playerTurn;
    public bool ended = false;
    public int[] state;
    public int score = 0;

    public GameState(bool _playerTurn, int[] _state) 
    {
        if (_state[0] ==  0 && _state[1] == 0)
        {
            ended = true;
        }
        else if (_state[2] == 0 && _state[3] == 0)
        {
            ended = true;
        }

        playerTurn = _playerTurn;
        state = _state;
        this.LoopTo5();
        score = GradeScore(this);
    }

    public List<GameState> GetMoves()
    {
        return GetMoves(this);
    }

    private List<GameState> GetMoves(GameState o)
    {
        GameState n = o.InvertGameState();

        List<GameState> output = new List<GameState>();
        if (n.state[0] != 0)
        {
            if (n.state[2] != 0)
            {
                output.Add(new GameState(!n.playerTurn, new int[]
                {
                    n.state[0], n.state[1], n.state[2] + n.state[0], n.state[3]
                }
                ));
            }
            if (n.state[3] != 0)
            {
                output.Add(new GameState(!n.playerTurn, new int[]
                {
                    n.state[0], n.state[1], n.state[2], n.state[3] + n.state[0]
                }
                ));
            }
        }
        else if (n.state[1] % 2 == 0) // case n.state[0] == 0
        {
            output.Add(new GameState(!n.playerTurn, new int[]
            {
                n.state[1]/2, n.state[1]/2, n.state[2], n.state[3]
            }
            ));
        }

        if (n.state[1] != 0)
        {
            if (n.state[1] != n.state[0]) // decreases redundancy and overlaps (attacking with the same numbers will result in repeated states)
            {
                if (n.state[2] != 0)
                {
                    output.Add(new GameState(!n.playerTurn, new int[]
                    {
                    n.state[0], n.state[1], n.state[2] + n.state[1], n.state[3]
                    }
                    ));
                }
                if (n.state[3] != 0)
                {
                    output.Add(new GameState(!n.playerTurn, new int[]
                    {
                    n.state[0], n.state[1], n.state[2], n.state[3] + n.state[1]
                    }
                    ));
                }
            }

        }
        else if (n.state[0] % 2 == 0) // case n.state[1] == 0
        {
            output.Add(new GameState(!n.playerTurn, new int[]
            {
                n.state[0]/2, n.state[0]/2, n.state[2], n.state[3]
            }
            ));
        }

        return output;
    }

    public float[] GetMovesRecursive(int depth)
    {
        
        List<GameState> possibleMoves = GetMoves(this);
        float[] moveEvaluation = new float[possibleMoves.Count];

        for (int i = 0; i < possibleMoves.Count; i++)
        {
            moveEvaluation[i] = 0;
            moveEvaluation[i] += GetMovesValueRecursive(depth, possibleMoves[i]);
        }

        return moveEvaluation;
    }

    public float GetMovesValueRecursive(int depth, GameState o, float acummulated = 0)
    {
        float acumulatedScore = acummulated;

        if (depth <= 0) // depth reached -> return falt score and do not calculate further
        {
            return o.score;
        }
        else if (ended) // game ended -> return score weighted by depth and do not calculate further
        {
            float sign = Mathf.Sign(o.score);
            float weightedScroe = Mathf.Pow(o.score, depth + 1);
            if (depth % 2 != 0)
            {
                weightedScroe *= sign;
            }

            return weightedScroe;
        }
        else // calculate further moves and repeat for each of them
        {
            List<GameState> possibleMoves = GetMoves(o);
            for (int i = 0; i < possibleMoves.Count; i++)
            {
                acumulatedScore += GetMovesValueRecursive(depth - 1, possibleMoves[i], acumulatedScore);

                float sign = Mathf.Sign(o.score);
                float weightedScroe = Mathf.Pow(o.score, depth + 1);
                if (depth % 2 != 0)
                {
                    weightedScroe *= sign;
                }

                acumulatedScore += weightedScroe;
            }
        }
        return acumulatedScore;
    }

    public void GradeScore()
    {
        score = GradeScore(this);
    }

    public int GradeScore(GameState state)
    {
        if (ended)
        {
            if (playerTurn)
            {
                return -12;
            }
            else
            {
                return +12;
            }
        }
        int score = 0;
        if (state.state[0] + state.state[2] == 5)
        {
            if (state.state[3] == 0)
            {
                score = 12;
            }
            else if (state.state[3] == state.state[2])
            {
                score = 4;
            }
            else
            {
                score = 8;
            }
        }
        else if (state.state[0] + state.state[3] == 5)
        {
            if (state.state[2] == 0)
            {
                score = 12;
            }
            else if (state.state[2] == state.state[3])
            {
                score = 4;
            }
            else
            {
                score = 8;
            }
        }

        if (state.state[1] + state.state[2] == 5)
        {
            if (state.state[3] == 0)
            {
                score = 12;
            }
            else if (state.state[3] == state.state[2])
            {
                score = 4;
            }
            else
            {
                score = 8;
            }
        }
        else if (state.state[1] + state.state[3] == 5)
        {
            if (state.state[2] == 0)
            {
                score = 12;
            }
            else if (state.state[2] == state.state[3])
            {
                score = 4;
            }
            else
            {
                score = 8;
            }
        }

        if (state.state[0] == 0) { score += 4; }
        if (state.state[1] == 0) { score += 4; }
        if (state.state[2] == 0) { score -= 4; }
        if (state.state[3] == 0) { score -= 4; }

        if (playerTurn)
        {
            return -score;
        }
        else
        {
            return score;
        }

    }

    public void LoopTo5()
    {
        if (state[0] > 4) {state[0] -= 5;}
        if (state[1] > 4) { state[1] -= 5; }
        if (state[2] > 4) { state[2] -= 5; }
        if (state[3] > 4) { state[3] -= 5; }
    }

    public GameState InvertGameState()
    {
        return new GameState(playerTurn, new int[] { state[2], state[3], state[0], state[1] });
    }




    public override string ToString()
    {
        return $"{state[0]}, {state[1]}, {state[2]}, {state[3]} - score: {score}, player turn = {playerTurn}, ended = {ended}";
    }
}

public class GameManager : MonoBehaviour
{
    public Hand[] playerHands;
    public Hand[] opponentHands;
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
        GameState test = new GameState(false, new int[] { 2, 1, 3, 1 });
        Debug.Log(test.score);
        StartPlayerTurn();
        
    }

    void Update()
    {
        
    }

    public void StartPlayerTurn()
    {
        gameState.playerTurn = true;
        for (int i = 0; i < playerHands.Length; i++)
        {
            playerHands[i].SetCollider2D(true);
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
        gameState.GradeScore();
    }

    public void SetGameState(GameState setState)
    {
        gameState = setState;
        int index = 0;
        for (int i = 0; i < playerHands.Length; i++, index++)
        {
            playerHands[i].state = setState.state[index];
            playerHands[i].UpdateFingers();
        }
        for (int i = 0; i < opponentHands.Length; i++, index++)
        {
            opponentHands[i].state = setState.state[index];
            opponentHands[i].UpdateFingers();
        }
    }

    public void EndPlayerTurn()
    {
        gameState.playerTurn = false;

        for (int i = 0; i < playerHands.Length; i++)
        {
            playerHands[i].SetCollider2D(false);
        }

        UpdateGameState();

        Debug.Log("Player turn ended with state: " + gameState.ToString());

        StartOpponentTurn();
    }

    public void StartOpponentTurn()
    {
        
        float[] potentialChoicesEvaluation = gameState.GetMovesRecursive(2);

        float highestValue = potentialChoicesEvaluation[0];
        int highestValuedChoice = 0;
        for (int i = 0; i < potentialChoicesEvaluation.Length; i++)
        {
            if (potentialChoicesEvaluation[i] > highestValue)
            {
                Debug.Log("Comparing marked choice of value: " + highestValue + " with new condender: " + potentialChoicesEvaluation[i]);
                highestValuedChoice = i;
                highestValue = potentialChoicesEvaluation[i];
            }
            Debug.Log("Move to state " + gameState.GetMoves()[i] + " | graded as worth: " + potentialChoicesEvaluation[i]);
        }
        Debug.Log("Highest value was " + highestValue);

        GameState favoredGameState = gameState.GetMoves()[highestValuedChoice];
        favoredGameState = favoredGameState.InvertGameState();
        SetGameState(favoredGameState);

        Debug.Log("Enemy turn ended with state: " + gameState.ToString());

        StartPlayerTurn();


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
