using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepMaker : MonoBehaviour
{
    public bool playerTurn = true;
    public int stepsToFinish = 17;
    private int currentStep = 1;

    public Text turnOrder;

    private Vector3 currentPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTurn)
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                MakeStep();

                EndTurn();
                return;
            }

            if (Input.GetKey(KeyCode.Alpha2))
            {
                MakeStep(); 
                MakeStep();

                EndTurn();
                return;
            }
        }

        else
        {

            StartCoroutine(SlowDown());

        }
    }

    private void MakeStep()
    {
        this.transform.position = currentPosition + new Vector3(1, 0, 0);
    }

    private void EndTurn()
    {
        currentPosition = transform.position;
        playerTurn = false;
    }

    IEnumerator SlowDown()
    {
        yield return new WaitForSeconds(3);
    }

    private void ChangeTurnOrder(bool isPlayer)
    {
        if (isPlayer)
        {
            turnOrder.text = "Your Turn!";
            turnOrder.color = Color.green;
        }
        else 
        { 
            turnOrder.text = "Enemy Turn!";
            turnOrder.color = Color.red;
        }
        
    }
}
