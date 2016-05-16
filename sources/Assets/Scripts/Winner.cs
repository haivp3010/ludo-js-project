using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Winner : MonoBehaviour {
    public Button exitGame;
    public Button resetGame;
    //public Text winner;
    public Canvas gameOver;
    public Animator anim;
    public SpriteRenderer winner;
	// Use this for initialization
	void Start () {
        exitGame = exitGame.GetComponent<Button>();
        resetGame = resetGame.GetComponent<Button>();
        //winner = winner.GetComponent<Text>();
        gameOver = gameOver.GetComponent<Canvas>();
        anim = anim.GetComponent<Animator>();
        winner = winner.GetComponent<SpriteRenderer>();
        gameOver.enabled = false;
        anim.enabled = false;
        winner.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (GameState.Instance.Winner == HorseColor.Red)
        {
            gameOver.enabled = true;
            winner.enabled = true;
            anim.enabled = true;
            anim.Play("red_win_animation");
        }
        else if (GameState.Instance.Winner == HorseColor.Green)
        {
            gameOver.enabled = true;
            winner.enabled = true;
            anim.enabled = true;
            anim.Play("green_win_animation");
        }
        else if (GameState.Instance.Winner == HorseColor.Blue)
        {
            gameOver.enabled = true;
            winner.enabled = true;
            anim.enabled = true;
            anim.Play("blue_win_animation");
        }
        else if (GameState.Instance.Winner == HorseColor.Yellow)
        {
            gameOver.enabled = true;
            winner.enabled = true;
            anim.enabled = true;
            anim.Play("yellow_win_animation");
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Reset()
    {
        GameState.Instance.ResetGameState();
        SceneManager.LoadScene("Menu");
    }

    
}
