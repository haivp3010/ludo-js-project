using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HorseControl : MonoBehaviour
{
    public Animator Anim;
    public int horseNumber; // 0 - 15
    private int horsePosition;
    private HorseColor horseColor;
    private bool updateOn = true;
    private PolygonCollider2D horseCollider;

    void Start()
    {

        // Get references
        Anim = GetComponent<Animator>();
        horseCollider = GetComponent<PolygonCollider2D>();

        if (!IsPlayer())
        {
            horseCollider.enabled = false;
            Anim.enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            enabled = false;
        }
        else
        {
            // Get horse properties
            horseColor = GameState.Instance.GetHorseColor(horseNumber);
            horsePosition = PositionControl.GetSpawnPosition(horseNumber);
            


            /* DEBUG */
            //switch (horseNumber)
            //{
            //    case 0:
            //        horsePosition = 106;
            //        break;
            //    case 1:
            //        horsePosition = 105;
            //        break;
            //    case 2:
            //        horsePosition = 104;
            //        break;
            //    case 3:
            //        horsePosition = 102;
            //        break;
            //    case 4:
            //        horsePosition = 206;
            //        break;
            //    case 5:
            //        horsePosition = 205;
            //        break;
            //    case 6:
            //        horsePosition = 204;
            //        break;
            //    case 7:
            //        horsePosition = 202;
            //        break;
            //    case 8:
            //        horsePosition = 306;
            //        break;
            //    case 9:
            //        horsePosition = 305;
            //        break;
            //    case 10:
            //        horsePosition = 304;
            //        break;
            //    case 11:
            //        horsePosition = 302;
            //        break;
            //    case 12:
            //        horsePosition = 406;
            //        break;
            //    case 13:
            //        horsePosition = 405;
            //        break;
            //    case 14:
            //        horsePosition = 404;
            //        break;
            //    case 15:
            //        horsePosition = 402;
            //        break;
            //}
            // End of debug
            
            gameObject.transform.position = PositionControl.GetRealPosition(horsePosition);

            // Initialize GameState
            GameState.Instance.HorsePosition[horseNumber] = horsePosition;
        }
    }

    void Update()
    {
        SortingOrder();
        FlipHorses();
        // Only current player can click on horses

        if (updateOn == false)
        {
            Anim.enabled = true;
            Anim.Play("attack");
            //GameState.AttackingHorse = true;
        }
        else
        {
            ColliderSetup();

            // Bot processing
            if (GameState.Instance.IsBotTurn() && horseColor == GameState.Instance.CurrentPlayer && !GameState.Instance.HorseMoving && !GameState.AttackingHorse)
            {
                horseCollider.enabled = false;

                if (GameState.Instance.BotChoice() == horseNumber)
                {
                    StartCoroutine(Wait());
                }
            }

            // End of Bot processing
                

            if (GameState.Instance.Winner != HorseColor.None && horsePosition == GameState.Instance.HorsePosition[horseNumber])
            {
                horseCollider.enabled = false;

            }
            else
            {
                var step = GameState.Instance.HorseSpeed * Time.deltaTime;

                if (horsePosition != GameState.Instance.HorsePosition[horseNumber])
                {
                    if (horsePosition >= 900 || GameState.Instance.HorsePosition[horseNumber] >= 900)
                    {
                        if (GameState.Instance.HorsePosition[horseNumber] >= 900)
                        {
                            if (GameState.AttackingHorse)
                            {
                                horsePosition = GameState.Instance.HorsePosition[horseNumber];
                                gameObject.transform.position = PositionControl.GetRealPosition(horsePosition);
                                GameState.AttackingHorse = false;
                            }
                        }
                        else if (horsePosition >= 900)
                        {
                            if (GameState.Instance.Movable[horseNumber] == MoveCase.Attackable)
                                GameState.AttackingHorse = true;
                            horsePosition = GameState.Instance.HorsePosition[horseNumber];
                            gameObject.transform.position = PositionControl.GetRealPosition(horsePosition);
                            GameState.Instance.HorseMoving = false;
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
                            {
                                StartCoroutine(updateOff());
                            }
                        }
                        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, PositionControl.GetRealPosition(nextPosition), step);

                        if (gameObject.transform.position == PositionControl.GetRealPosition(nextPosition))
                        {
                            horsePosition = nextPosition;
                            Anim.enabled = false;

                            // If horse comes to target, change turn
                            if (horsePosition == GameState.Instance.HorsePosition[horseNumber])
                            {
                                GameState.Instance.HorseMoving = false;
                                if (!((GameState.Dice1 == GameState.Dice2) || (GameState.Dice1 == 0 && GameState.Dice2 == 5) || (GameState.Dice1 == 5 && GameState.Dice2 == 0)))
                                    GameState.Instance.NextPlayer();

                                // Reset dice roll
                                GameState.Instance.DiceRolled = false;
                            }
                        }
                    }
                }

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
        updateOn = true;
        Anim.Play("walk");

    }

    private void OnMouseDown()
    {
        if (horseColor == GameState.Instance.CurrentPlayer && !GameState.Instance.HorseMoving && GameState.Instance.DiceRolled && GameState.Instance.Movable[horseNumber] != MoveCase.Immovable && GameState.Instance.Message.Equals(""))
        {
            GameState.Instance.HorseMoving = true;
            GameState.Instance.ProcessDice(horseNumber);
        }
    }

    private void FlipHorses()
    {
        if ((horsePosition >= 0 && horsePosition <= 11) || (horsePosition >= 17 && horsePosition <= 21) || (horsePosition >= 35 && horsePosition <= 40) || horsePosition == 46 || horsePosition == 47 || (horsePosition >= 900 && horsePosition <= 903) || (horsePosition >= 912 && horsePosition <= 915) || (horsePosition >= 101 && horsePosition <= 106) || (horsePosition >= 401 && horsePosition <= 406))
            GetComponent<SpriteRenderer>().flipX = true;
        else
            GetComponent<SpriteRenderer>().flipX = false;
    }

    private void SortingOrder()
    {
        GameState.Instance.CurrentYValues[horseNumber] = transform.position.y;
        GameState.Instance.UpdateSortingOrder();
        GetComponent<Renderer>().sortingOrder = GameState.Instance.SortingOrder[horseNumber];
    }

    private void ColliderSetup()
    {
        horseCollider.enabled = horseColor == GameState.Instance.CurrentPlayer;
    }

    private bool IsPlayer()
    {
        return horseNumber <= ((GameState.Instance.NUMBER_OF_PLAYERS) * 4 - 1);
    }

    private void OnMouseEnter()
    {
        Anim.enabled = true;
    }

    private void OnMouseExit()
    {
        Anim.enabled = false;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        OnMouseDown();
    }
}