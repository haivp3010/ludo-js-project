using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HorseControl : MonoBehaviour
{
    public Animator Anim;
    public float Speed;
    public int horseNumber; // 0 - 15
    private int horsePosition;
    private HorseColor horseColor;
    private bool updateOn = true;
    void Start()
    {
        // Get references
        Anim = gameObject.GetComponent<Animator>();

        // Get horse properties
        horseColor = GameState.GetHorseColor(horseNumber);
        horsePosition = PositionControl.GetSpawnPosition(horseNumber);
        gameObject.transform.position = PositionControl.GetRealPosition(horsePosition);

        // Initialize GameState
        GameState.Instance.HorsePosition[horseNumber] = horsePosition;
    }

    void Update()
    {
        // Only current player can click on horses
        
        if(updateOn == false)
        {
            Anim.enabled = true;
            Anim.Play("attack");
            //GameState.AttackingHorse = true;
        }
        else
        {
            if (horseColor != GameState.Instance.CurrentPlayer)
                gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            else
                gameObject.GetComponent<PolygonCollider2D>().enabled = true;
            if (GameState.Instance.Winner != HorseColor.None && horsePosition == GameState.Instance.HorsePosition[horseNumber])
            {
                gameObject.GetComponent<PolygonCollider2D>().enabled = false;

            }
            else
            {
                var step = Speed * Time.deltaTime;

                if (horsePosition != GameState.Instance.HorsePosition[horseNumber])
                {
                    if (horsePosition >= 900 || GameState.Instance.HorsePosition[horseNumber] >= 900)
                    {
                        if (GameState.Instance.HorsePosition[horseNumber] >= 900)
                        {
                            if (GameState.AttackingHorse==true)
                            {
                                horsePosition = GameState.Instance.HorsePosition[horseNumber];
                                gameObject.transform.position = PositionControl.GetRealPosition(horsePosition);
                                GameState.AttackingHorse = false;
                            }
                        }
                        else if(horsePosition >= 900)
                        {
                            if (GameState.Instance.Movable[horseNumber] == MoveCase.Attackable)
                                GameState.AttackingHorse = true;
                            horsePosition = GameState.Instance.HorsePosition[horseNumber];
                            gameObject.transform.position = PositionControl.GetRealPosition(horsePosition);                            
                        }
                        // Reset dice roll
                        
                        GameState.Instance.DiceRolled = false;
                    }

                    else
                    {
                        Anim.enabled = true;
                        int nextPosition = PositionControl.GetNextPosition(horseColor, horsePosition);
                        if (GameState.Instance.Movable[horseNumber] == MoveCase.Attackable && (gameObject.transform.position == PositionControl.GetRealPosition(horsePosition)))
                        {
                            if (PositionControl.GetNextPosition(horseColor, horsePosition) == GameState.Instance.HorsePosition[horseNumber])
                            //if (horsePosition == GameState.Instance.HorsePosition[horseNumber] - 1)
                            {
                                StartCoroutine(updateOff());
                            }
                        }
                        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, PositionControl.GetRealPosition(nextPosition), step);
                        
                        if (gameObject.transform.position == PositionControl.GetRealPosition(nextPosition))
                        {
                            horsePosition = nextPosition;
                            if (horsePosition == 12 || horsePosition == 17 || horsePosition == 23 || horsePosition == 35 || horsePosition == 41 || horsePosition == 46)
                            {
                                gameObject.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
                            }
                            //if (GameState.Instance.Movable[horseNumber] == MoveCase.Attackable)
                            //{
                            //    if (PositionControl.GetNextPosition(horseColor, horsePosition) == GameState.Instance.HorsePosition[horseNumber])
                            //    //if (horsePosition == GameState.Instance.HorsePosition[horseNumber] - 1)
                            //    {
                                    
                            //        StartCoroutine(updateOff());                            
                            //    }
                            //}
                            Anim.enabled = false;


                            // If horse comes to target, change turn
                            if (horsePosition == GameState.Instance.HorsePosition[horseNumber])
                            {


                                if (!((GameState.Dice1 == GameState.Dice2) || (GameState.Dice1 == 0 && GameState.Dice2 == 5) || (GameState.Dice1 == 5 && GameState.Dice2 == 0)))
                                    GameState.Instance.NextPlayer();

                                // Reset dice roll
                                GameState.Instance.DiceRolled = false;
                            }
                        }
                    }
                }
                else
                    GameState.Instance.HorseMoving = false;

                GameState.Instance.CheckWinner();
            }
        }

    }
 


    IEnumerator updateOff()
    {       
        updateOn = false;        
        GameState.AttackingHorse = false;
        yield return new WaitForSeconds(1.5f);
        GameState.AttackingHorse = true;

        //GameState.Instance.KillHorse(GameState.Instance.FindHorseAt(GameState.Instance.HorsePosition[horseNumber]));
        updateOn = true;        
        Anim.Play("walk");
        
    }

    private void OnMouseDown()
    {
        if (!GameState.Instance.HorseMoving)
        {
            GameState.Instance.HorseMoving = true;
            GameState.Instance.ProcessDice(horseNumber);
        }
    }



    private void OnMouseEnter()
    {
        Anim.enabled = true;
    }

    private void OnMouseExit()
    {
        Anim.enabled = false;
    }
}