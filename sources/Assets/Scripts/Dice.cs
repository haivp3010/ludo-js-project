using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dice : MonoBehaviour
{
    
    private static int number = 10;
    public int diceIdentity;            //khai báo dice 1 hoặc dice 2
    public Animator anim;
    private SpriteRenderer spriteRenderer;
    private bool updateOn;
    public Sprite[] dice1;              //khai báo các sprite của dice để add
    private int common;
    public int Number
    {
        get { return number; }
        set { number = value; }
    }



    // Use this for initialization
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Random chance = new Random();
        anim.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (GameState.AnimatingDice == true)            //nếu animatingdice = true
        {
            anim.enabled = true;                        //thì enable animation cho tất cả object đc gắn script(dice 1 và 2)

        }
        else
        {
            anim.enabled = false;
        }
        if (updateOn != true)
        {

            if (diceIdentity == 1)
            {
                spriteRenderer.sprite = dice1[GameState.Dice1];         //hiển thị giá trị cho dice 1

            }
            else if (diceIdentity == 2)
            {
                spriteRenderer.sprite = dice1[GameState.Dice2];         //hiển thị giá trị cho dice 2
            }

        }


    }
    private void OnMouseDown()
    {
        StartCoroutine(updateOff());                //khởi tạo coroutine, khi gọi hàm updateoff sẽ tắt animation trong 2 giây
        GameState.AnimatingDice = true;             //dùng GameState làm hàm trung gian, khi AnimatingDIce = true sẽ enable animation trong hàm update
    }



    IEnumerator updateOff()                     //hàm updateoff() đã nói ở trên
    {
        Debug.Log(Time.time);
        yield return new WaitForSeconds(2.0f);      //tắt animation sau 2s
        Debug.Log(Time.time);
        updateOn = false;
        RollingDice();                                 //gọi hàm rollingdice để random animation tạo dice value
        Debug.Log("dice1 value = " + (GameState.Dice1 + 1));
        Debug.Log("dice2 value = " + (GameState.Dice2 + 1));
    }

    private void RollingDice()                      //random giá trị
    {
        common = Random.Range(1, 6);                //1/5 khả năng 2 xx cùng giá tri
        if (common == 1)
        {
            GameState.Dice1 = Random.Range(0, 6);
            GameState.Dice2 = GameState.Dice1;

        }
        else                                        //random xx
        {
            GameState.Dice1 = Random.Range(0, 6);
            GameState.Dice2 = Random.Range(0, 6);
        }

        GameState.Instance.DiceRolled = true;
        GameState.Instance.UpdateMovable();
        if (GameState.Instance.NoHorseCanMove())
            if (!(GameState.Dice1 == GameState.Dice2 || (GameState.Dice1 == 0 && GameState.Dice2 == 5) || (GameState.Dice1 == 5 && GameState.Dice2 == 0)))
            {
                GameState.Instance.NextPlayer();
                GameState.Instance.DiceRolled = false;
            }

        anim.enabled = false;
        GameState.AnimatingDice = false;
        Debug.Log("common = " + common);
    }


}
