using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform[] playerHands;
    public Transform[] opponentHands;
    public int[] gameState;

    public bool playerTurn = true;

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
        gameState = new int[] { 1, 1, 1, 1 };
        StartPlayerTurn();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTurn == false)
        {
            StartPlayerTurn();
        }
        
    }

    public void StartPlayerTurn()
    {
        playerTurn = true;
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
            gameState[index] = playerHands[i].GetComponent<Hand>().state;
        }
        for (int i = 0; i < opponentHands.Length; i++, index++)
        {
            gameState[index] = opponentHands[i].GetComponent<Hand>().state;
        }

        PrintGameState();
    }

    public void EndPlayerTurn()
    {
        playerTurn = false;

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
        string print = "";
        for (int i = 0; i < gameState.Length; i++)
        {

            print += gameState[i];
        }

        Debug.Log(print);
    }
}
