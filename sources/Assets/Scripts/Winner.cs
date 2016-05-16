using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Winner : MonoBehaviour {
    public Button exitGame;
    public Button resetGame;
    public Text winner;
    public Canvas gameOver;
	// Use this for initialization
	void Start () {
        exitGame = exitGame.GetComponent<Button>();
        resetGame = resetGame.GetComponent<Button>();
        winner = winner.GetComponent<Text>();
        gameOver = gameOver.GetComponent<Canvas>();
        gameOver.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (GameState.Instance.Winner != HorseColor.None)
        {
            winner.text = GameState.Instance.Winner + " Wins!";
            gameOver.enabled = true;
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
