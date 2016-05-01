using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HorseControl : MonoBehaviour {
    public int horseNumber;
    private int position;
    private Queue<Vector3> frameVectorQueue;
	void Start() {
        //position = PositionControl.GetStartPosition(GameState.GetHorseColor(horseNumber));
        // Test code to initialize position for 2 red horses 0 and 1
        if (horseNumber == 0)
        {
            position = GameState.Instance.HorsePosition[horseNumber] = 0;
        }
        else
        {
            position = GameState.Instance.HorsePosition[horseNumber] = 1;
        }
        gameObject.transform.position = GameState.GetRealPosition(position);
        frameVectorQueue = new Queue<Vector3>();
	}

	void Update() {
        int positionInGameState = GameState.Instance.HorsePosition[horseNumber];
        if (position != positionInGameState)
        {
            List<Vector3> frameVectors = GenerateFrameVectors(position, positionInGameState);
            foreach (Vector3 fv in frameVectors)
                frameVectorQueue.Enqueue(fv);
            position = positionInGameState;
        }
        if (frameVectorQueue.Count > 0)
        {
            gameObject.transform.Translate(frameVectorQueue.Dequeue());
        }
	}
    void OnMouseDown ()
    {
        // Assume the horse will move 3 steps when clicked
        GameState.Instance.HorsePosition[horseNumber] += 3;
    }

    List<Vector3> GenerateFrameVectors(int startPosition, int endPosition)
    {
        // This function is a prototype, it only solve simple case that 0 <= startPosition <= endPosition <= 55
        const int DELTA = 10; // Generate 10 frame vectors for each step
        List<Vector3> frameVectors = new List<Vector3>();
        for (int i = startPosition + 1; i <= endPosition; i++)
            for (int j = 1; j <= DELTA; j++)
                frameVectors.Add((GameState.GetRealPosition(i) - GameState.GetRealPosition(i - 1)) / DELTA);
        return frameVectors;
    }
}
