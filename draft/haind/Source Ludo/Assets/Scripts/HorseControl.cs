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
        var step = speed * Time.deltaTime;

        if (clicked != null)
        {
            anim.enabled = true;
            int nextPosition = PositionControl.GetNextPosition(color, position);
            if (nextPosition == -1)
                StopMoving();
            else
            {
                clicked.transform.position = Vector3.MoveTowards(clicked.transform.position, PositionControl.GetRealPosition(nextPosition), step);

                if (clicked.transform.position == PositionControl.GetRealPosition(nextPosition))
                {
                    i++;
                    position = nextPosition;

                    if (i >= iClick)
                        StopMoving();
                }
            }
        }
    }

    private void StopMoving()
    {
        clicked = null;
        anim.enabled = false;
    }

    private void OnMouseDown()
    {
        if (clicked == null)
        {
            clicked = gameObject;
            i = 0;
        }
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
