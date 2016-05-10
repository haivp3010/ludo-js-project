using UnityEngine;
using System.Collections;

public class MessageManager : MonoBehaviour {

    private Animator Anim;

	// Use this for initialization
	void Start () {
        Anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!GameState.Instance.Message.Equals(""))
        {
            GetComponent<SpriteRenderer>().enabled = true;
            Anim.enabled = true;
            Anim.Play(GameState.Instance.Message);
            StartCoroutine(ResetMessage());
        }
	}

    IEnumerator ResetMessage()
    {
        yield return new WaitForSeconds(Anim.GetCurrentAnimatorStateInfo(0).length);
        Anim.enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GameState.Instance.Message = "";
    }
}
