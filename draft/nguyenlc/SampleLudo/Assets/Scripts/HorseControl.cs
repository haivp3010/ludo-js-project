using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HorseControl : MonoBehaviour {
    public int horseNumber;
    private int position;
    private Queue<Vector3> frameVectorQueue;
	void Start()
    {
        position = GameState.Instance.HorsePosition[horseNumber];
        gameObject.transform.position = GameState.GetRealPosition(position);
        frameVectorQueue = new Queue<Vector3>();
	}

	void Update()
    {
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
        GameState.Instance.ProcessDice(horseNumber);
    }
    List<Vector3> GenerateFrameVectors(int startPosition, int endPosition)
    {
        const int DELTA = 10; // Generate 10 frame vectors for each step
        List<int> positionSeries = GameState.GetPositionSeries(horseNumber, startPosition, endPosition);
        List<Vector3> frameVectors = new List<Vector3>();
        for (int i = 1; i < positionSeries.Count; i++)
            for (int j = 1; j <= DELTA; j++)
                frameVectors.Add((GameState.GetRealPosition(positionSeries[i]) - GameState.GetRealPosition(positionSeries[i - 1])) / DELTA);
        return frameVectors;
    }
}
