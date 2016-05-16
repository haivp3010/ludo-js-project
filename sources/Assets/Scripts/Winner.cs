using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Winner : MonoBehaviour {
    
    public Canvas gameOver;
    public Animator anim;
    public SpriteRenderer winner;
	
    // Use this for initialization
	void Start () {
        
        gameOver = gameOver.GetComponent<Canvas>();
        anim = anim.GetComponent<Animator>();
        winner = winner.GetComponent<SpriteRenderer>();
        gameOver.enabled = false;
        anim.enabled = false;
        winner.enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (GameState.Instance.Winner != HorseColor.None)
	    {
            gameOver.enabled = true;
            winner.enabled = true;
            anim.enabled = true;

            switch (GameState.Instance.Winner)
            {
                case HorseColor.Red:

                    anim.Play("red_win_animation");
                    break;
                case HorseColor.Green:
                    gameOver.enabled = true;
                    winner.enabled = true;
                    anim.enabled = true;
                    anim.Play("green_win_animation");
                    break;
                case HorseColor.Blue:
                    gameOver.enabled = true;
                    winner.enabled = true;
                    anim.enabled = true;
                    anim.Play("blue_win_animation");
                    break;
                case HorseColor.Yellow:
                    gameOver.enabled = true;
                    winner.enabled = true;
                    anim.enabled = true;
                    anim.Play("yellow_win_animation");
                    break;
            }
	    
	    }
	}
}
