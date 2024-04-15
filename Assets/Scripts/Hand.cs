using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hand : MonoBehaviour
{
    public int state = 0;
    public bool isPlayer;

    private bool isDragging = false;
    private Collider2D collider;
    private Vector3 ogPosition;
    private Vector3 offset;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    void Start()
    {
        state = 1;
        UpdateFingers();
        ogPosition = transform.position;
    }
    
    public void UpdateFingers() 
    {

        
        for (int i = 0; i < 4;  i++) 
        {
            if (i < state) 
            {
                transform.GetChild(i).gameObject.SetActive(true); ; 
            }
            else 
            { 
                transform.GetChild(i).gameObject.SetActive(false); 
            }
            
        }
    
    }
    void Update()
    {
        
    }

    public int Attack(int fingers, bool player)
    {
        if (player == isPlayer)
        {
            Debug.Log("Attempting to revive other hand.");
            if (state == 0)
            {
                if (fingers % 2 == 0)
                {
                    state += fingers / 2;
                    UpdateFingers();
                    Debug.Log("Other hand revived with: " + fingers / 2 + " fingers.");
                    return fingers / 2;
                }
                Debug.Log("Odd number of fingers can not be split to revive other hand.");
                return -1;
            }
            Debug.Log("Can not revive hand that hasn't been knocked out.");
            return -1;
        }
        else
        {
            Debug.Log("Attempting to attack opponent's hand.");
            state += fingers;
            if (state > 4)
            {
                state -= 5;
            }
            UpdateFingers();
            Debug.Log("Attack added " + fingers + " fingers");
            return 0;
        }
    }

    public void OnMouseDown()
    {
        if (state > 0)
        {
            offset = transform.position - MouseWorldPosition();
            isDragging = true;
        }
    }
    private void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = MouseWorldPosition() + offset;
        }
    }

    public void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;
            SetCollider2D(false);


            var rayOrigin = Camera.main.transform.position;
            var rayDirection = MouseWorldPosition();
            RaycastHit2D target;
            if (target = Physics2D.Raycast(rayOrigin, rayDirection))
            {
                if (target.transform.tag == "Hand")
                {
                    int effect = target.transform.GetComponent<Hand>().Attack(this.state, this.isPlayer);

                    if (effect >= 0)
                    {
                        if (effect > 0)
                        {
                            state -= effect;
                            UpdateFingers();
                        }

                        GameManager.instance.EndPlayerTurn();
                    }
                }
            }

            transform.position = ogPosition;
            SetCollider2D(true);
        }
    }

    public void SetCollider2D(bool set)
    {
        collider.enabled = set;
    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }
}
