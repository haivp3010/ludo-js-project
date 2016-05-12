using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuScript : MonoBehaviour {
    
    public Button playButton;
    public Button quitButton;
    public Button helpButton;
    public Button backButton;
    public Button decreaseButton;
    public Button increaseButton;
    public Canvas helpMenu;
    public Canvas playMenu;
    public Text numberPlayer;
    private int number = 4;
	// Use this for initialization
	void Start () {
        numberPlayer = numberPlayer.GetComponent<Text>();
        playButton = playButton.GetComponent<Button>();
        quitButton = quitButton.GetComponent<Button>();
        helpButton = helpButton.GetComponent<Button>();
        decreaseButton = decreaseButton.GetComponent<Button>();
        increaseButton = increaseButton.GetComponent<Button>();
        helpMenu = helpMenu.GetComponent<Canvas>();
        playMenu = playMenu.GetComponent<Canvas>();
        helpMenu.enabled = false;
        playMenu.enabled = false;
        
        numberPlayer.text = " " +  number.ToString();
    }
	
	// Update is called once per frame
	public void PlayMenu () {
        //SceneManager.LoadScene("ludo");
        playMenu.enabled = true;
    }

    public void StartGame()
    {
        GameState.Instance.ResetGameState();
        SceneManager.LoadScene("ludo");
    }
    public void Quit ()
    {
        Application.Quit();
    }

    public void Help()
    {
        helpMenu.enabled = true;
    }
    public void Back()
    {
        helpMenu.enabled = false;
    }
    public void Decrease()
    {
        if (number > 2 && number <=4)
        {
            number--;
            decreaseButton.enabled = true;
            GameState.Instance.NUMBER_OF_PLAYERS = number;
        }
        //else decreaseButton.enabled = false;
        numberPlayer.text = " " + number.ToString();
    }
    public void Increase()
    {
        if (number < 4 && number >=2)
        {
            number++;
            increaseButton.enabled = true;
            GameState.Instance.NUMBER_OF_PLAYERS = number;
        }
        //else increaseButton.enabled = false;
        numberPlayer.text = " " + number.ToString();
    }
}
