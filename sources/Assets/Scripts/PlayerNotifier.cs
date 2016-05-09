using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerNotifier : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Animator>().Play(GameState.Instance.CurrentPlayer.ToString());
	}
}
