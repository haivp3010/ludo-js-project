using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class moving_ludo : MonoBehaviour {
    private Vector3[] movingPosition = new Vector3[12];         //routes coordinations
    private Vector3[] defaultRedPosition = new Vector3[4];      //starting coordinations
    public GameObject clicked;
    float speedMove = 5f;
    public int dice1, dice2;                                    //xuc xac
    bool move = false;
    private int i=0;
    private int check;
    public Animator anim;

    // Use this for initialization 
    void Start () {
        anim = GetComponent<Animator>();
        movingPosition[0] = new Vector3(-6.9f, -2.75f);
        movingPosition[1] = new Vector3(-5.75f, -2.1f);
        movingPosition[2] = new Vector3(-4.4f, -1.4f);
        movingPosition[3] = new Vector3(-3f, -0.7f);
        movingPosition[4] = new Vector3(-1.8f, -0.1f);
        movingPosition[5] = new Vector3(-0.4f, 0.3f);
        movingPosition[6] = new Vector3(0.9f, 0.1f);
        movingPosition[7] = new Vector3(2.2f, -0.6f);
        movingPosition[8] = new Vector3(3.6f, -1.4f);
        movingPosition[9] = new Vector3(4.8f, -2f);
        movingPosition[10] = new Vector3(6f, -2.8f);
        movingPosition[11] = new Vector3(9.2f, -2.3f);
        defaultRedPosition[0] = new Vector3(-2.1f,-4.1f);
        defaultRedPosition[1] = new Vector3(-0.5f, -3.6f);
        defaultRedPosition[2] = new Vector3(-1.1f, -4.8f);
        defaultRedPosition[3] = new Vector3(0.1f, -4.2f);
    }

    

    // Update is called once per frame
    void Update()
    {

        if (i < (dice1 + dice2))                                     //moving function
        {
            if (move)
            {
                clicked.transform.position = Vector3.MoveTowards(clicked.transform.position, movingPosition[i], Time.deltaTime * speedMove);
                if (clicked.transform.position == movingPosition[i]) i++;
            }
        }
        else
        {
            move = false;
            anim.Play("New Animation2");
            
        }

        
    }
    

    void OnMouseDown()                                              //selecting ludo to move
    {
        anim.Play("New Animation");
        clicked = gameObject;
        move = true;
    }

  
}
