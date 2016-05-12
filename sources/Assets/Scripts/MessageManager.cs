using UnityEngine;
using System.Collections;

public class MessageManager : MonoBehaviour {

    private Animator Anim;

	// Use this for initialization
	void Start () {
        Anim = GetComponent<Animator>();
	    Anim.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!GameState.Instance.Message.Equals(""))
        {
            Anim.enabled = true;
            Anim.Play(GameState.Instance.Message, 0, 0f);
            GameState.Instance.Message = "";
        }
	}
}
