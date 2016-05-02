using UnityEngine;
using System.Collections;

public class HorseControl : MonoBehaviour
{
    public Animator Anim;
    public float Speed;
    public int horseNumber; // 0 - 15
    private int horsePosition;
    private HorseColor horseColor;
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
        var step = Speed * Time.deltaTime;

        if (horsePosition != GameState.Instance.HorsePosition[horseNumber])
        {
            if (horsePosition >= 900 || GameState.Instance.HorsePosition[horseNumber] >= 900)
            {
                horsePosition = GameState.Instance.HorsePosition[horseNumber];
                gameObject.transform.position = PositionControl.GetRealPosition(horsePosition);
            }
            else
            {
                Anim.enabled = true;
                int nextPosition = PositionControl.GetNextPosition(horseColor, horsePosition);
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, PositionControl.GetRealPosition(nextPosition), step);
                if (gameObject.transform.position == PositionControl.GetRealPosition(nextPosition))
                    horsePosition = nextPosition;
            }
        }
    }

    private void OnMouseDown()
    {
        //GameState.HorseMoving = true;
        GameState.Instance.ProcessDice(horseNumber);
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
