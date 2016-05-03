using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Winner : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (GameState.Instance.Winner != HorseColor.None)
            gameObject.GetComponent<Text>().text = GameState.Instance.Winner + " Wins!";
	}
}
