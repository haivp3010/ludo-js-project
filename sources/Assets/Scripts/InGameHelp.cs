using UnityEngine;
using System.Collections;

public class InGameHelp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    GetComponent<Canvas>().enabled = GameState.Instance.InGameHelp;
	}

    public void BackBtn()
    {
        GameState.Instance.InGameHelp = false;
    }
}
