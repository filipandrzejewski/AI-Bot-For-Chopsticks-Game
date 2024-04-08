using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public int state = 0;
    void Start()
    {
        state = 1;
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
}
