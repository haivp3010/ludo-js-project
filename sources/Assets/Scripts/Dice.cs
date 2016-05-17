using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dice : MonoBehaviour
{
    
    public int diceIdentity;            //khai báo dice 1 hoặc dice 2
    public Animator anim;
    private SpriteRenderer spriteRenderer;
    private bool updateOn;
    public Sprite[] dice1;              //khai báo các sprite của dice để add
    private int common;
    
    // Use this for initialization
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        anim.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Disable dice when one player wins
        if (GameState.Instance.Winner != HorseColor.None)
        {
            enabled = false;
        }
        else
        {
            GetComponent<PolygonCollider2D>().enabled = !GameState.Instance.IsBotTurn();

            // Process Bot's turn
            if (GameState.Instance.IsBotTurn() && !GameState.Instance.AnimatingDice && !GameState.Instance.HorseMoving && !GameState.Instance.AttackingHorse)
            {
                StartCoroutine(!GameState.Instance.Message.Equals("") ? Wait(2.25f) : Wait(1f));
            }
            
            // End of Bot processing

            anim.enabled = GameState.Instance.AnimatingDice;
            if (updateOn != true)
            {

                if (diceIdentity == 1)
                {
                    spriteRenderer.sprite = dice1[GameState.Instance.Dice1];         //hiển thị giá trị cho dice 1

                }
                else if (diceIdentity == 2)
                {
                    spriteRenderer.sprite = dice1[GameState.Instance.Dice2];         //hiển thị giá trị cho dice 2
                }

            }
        }
    }
    private void OnMouseDown()
    {
        if (!GameState.Instance.HorseMoving && !GameState.Instance.AttackingHorse && !GameState.Instance.AnimatingDice && !GameState.Instance.DiceRolled && GameState.Instance.Message.Equals("") && GameState.Instance.Winner == HorseColor.None)
        {
            StartCoroutine(updateOff());                //khởi tạo coroutine, khi gọi hàm updateoff sẽ tắt animation trong 2 giây
            GameState.Instance.AnimatingDice = true;             //dùng GameState làm hàm trung gian, khi AnimatingDIce = true sẽ enable animation trong hàm update
        }
    }



    IEnumerator updateOff()                     //hàm updateoff() đã nói ở trên
    {        
        yield return new WaitForSeconds(1.0f);      //tắt animation sau 2s
        updateOn = false;
        RollingDice();                                 //gọi hàm rollingdice để random animation tạo dice value
    }

    private void RollingDice()                      //random giá trị
    {
        common = Random.Range(1, 6);                //1/5 khả năng 2 xx cùng giá tri
        if (common == 1)
        {
            GameState.Instance.Dice1 = Random.Range(0, 5);
            GameState.Instance.Dice2 = GameState.Instance.Dice1;

        }
        else                                        //random xx
        {
            GameState.Instance.Dice1 = Random.Range(0, 5);
            GameState.Instance.Dice2 = Random.Range(0, 5);
        }

        GameState.Instance.DiceRolled = true;
        GameState.Instance.UpdateMovable();

        if ((GameState.Instance.Dice1 == GameState.Instance.Dice2 || (GameState.Instance.Dice1 == 0 && GameState.Instance.Dice2 == 5) || (GameState.Instance.Dice1 == 5 && GameState.Instance.Dice2 == 0)))
            GameState.Instance.Message = "Them Luot";

        if (GameState.Instance.NoHorseCanMove())
        {
            if (!(GameState.Instance.Dice1 == GameState.Instance.Dice2 || (GameState.Instance.Dice1 == 0 && GameState.Instance.Dice2 == 5) || (GameState.Instance.Dice1 == 5 && GameState.Instance.Dice2 == 0)))
            {
                GameState.Instance.Message = "Mat Luot";
                GameState.Instance.NextPlayer();
            }

            GameState.Instance.DiceRolled = false;
        }

        anim.enabled = false;
        GameState.Instance.AnimatingDice = false;
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        OnMouseDown();
    }
}
