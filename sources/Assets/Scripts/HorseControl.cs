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
                            if (GameState.AttackingHorse == true)
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
        if (!GameState.Instance.HorseMoving && GameState.Instance.DiceRolled && GameState.Instance.Movable[horseNumber] != MoveCase.Immovable && GameState.Instance.Message.Equals(""))
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
        if (horseColor != GameState.Instance.CurrentPlayer)
            horseCollider.enabled = false;
        else
            horseCollider.enabled = true;
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
}