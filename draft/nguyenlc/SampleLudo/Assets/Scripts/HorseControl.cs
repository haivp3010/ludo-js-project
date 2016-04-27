using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HorseControl : MonoBehaviour {
    public int horseNumber;
    private int position;
    private Queue<Vector3> frameVectors;
	void Start() {
        position = PositionControl.GetStartPosition(GameState.GetHorseColor(horseNumber));
        frameVectors = new Queue<Vector3>();
	}
	void Update() {
        if (position != GameState.Instance.HorsePosition[horseNumber])
        {
            // Generate frame vectors
            // Add vectors to queue
            position = GameState.Instance.HorsePosition[horseNumber];
        }

        if (frameVectors.Count > 0)
        {
            // Process the first frame vectors
        }
	}
    void OnMouseDown ()
    {
        GameState.Instance.MoveHorseForward(horseNumber, GameState.Instance.CurrentDiceValue);
    }
}
