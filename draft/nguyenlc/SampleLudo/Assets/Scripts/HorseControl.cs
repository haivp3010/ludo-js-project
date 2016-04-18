using UnityEngine;
using System.Collections;

public class HorseControl : MonoBehaviour {
    public int horseNumber; // 0 - 15
    private int position;
    private HorseColor color;
	void Start() {
        color = GameLogic.GetHorseColor(horseNumber);
        position = PositionControl.GetStartPosition(color);
	}
	void Update() {
        gameObject.transform.position = PositionControl.GetRealPosition(position);
	}

    void OnMouseDown ()
    {
        position++;
    }
}
