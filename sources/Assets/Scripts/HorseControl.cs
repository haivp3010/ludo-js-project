using UnityEngine;
using System.Collections;

public class HorseControl : MonoBehaviour
{
    public Animator anim;
    private GameObject clicked;
    public float speed;
    public static Dice dice1 = new Dice();
    private int i = 0;
    private int iClick = dice1.Number;
    public int horseNumber; // 0 - 15
    private int position;
    private HorseColor color;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        color = GameLogic.GetHorseColor(horseNumber);
        position = PositionControl.GetStartPosition(color);
    }
    void Update()
    {
        //gameObject.transform.position = PositionControl.GetRealPosition(position);
        var step = speed * Time.deltaTime;

        if (clicked != null)
        {
            anim.enabled = true;
            int nextPosition = position + 1;
            clicked.transform.position = Vector3.MoveTowards(transform.position, PositionControl.GetRealPosition(nextPosition), step);

            if (clicked.transform.position == PositionControl.GetRealPosition(nextPosition))
            {
                i++;
                position++;

                if (i > iClick)
                {
                    clicked = null;

                    anim.enabled = false;
                }

                /*
                if (i == 48)
                {
                    i = 0;
                    iClick -= 48;
                }
                if (i == 36)
                {
                    i = 101;
                    iClick += 65;
                }
                */

            }
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Sprite Clicked!");
        if (clicked == null)
            Debug.Log("null");
        clicked = gameObject;
        i = 0;
        Debug.Log(clicked.ToString());
    }

    private void OnMouseEnter()
    {
        anim.enabled = true;

        Debug.Log("MouseEnter");
    }

    private void OnMouseExit()
    {
        anim.enabled = false;
        Debug.Log("MouseExit");
    }
}
