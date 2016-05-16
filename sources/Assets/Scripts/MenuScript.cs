using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuScript : MonoBehaviour
{

    public Button playButton;
    public Button quitButton;
    public Button helpButton;
    public Button backButton;
    public Button decreasePlayerButton;
    public Button increasePlayerButton;
    public Button decreaseAIButton;
    public Button increaseAIButton;
    public Canvas helpMenu;
    public Canvas playMenu;
    public Text numberPlayer;
    public Text numberAI;
    private int numberP = 4;
    private int numberA = 0;
    // Use this for initialization
    void Start()
    {
        numberPlayer = numberPlayer.GetComponent<Text>();
        numberAI = numberAI.GetComponent<Text>();
        playButton = playButton.GetComponent<Button>();
        quitButton = quitButton.GetComponent<Button>();
        helpButton = helpButton.GetComponent<Button>();
        decreasePlayerButton = decreasePlayerButton.GetComponent<Button>();
        increasePlayerButton = increasePlayerButton.GetComponent<Button>();
        decreaseAIButton = decreaseAIButton.GetComponent<Button>();
        increaseAIButton = increaseAIButton.GetComponent<Button>();
        helpMenu = helpMenu.GetComponent<Canvas>();
        playMenu = playMenu.GetComponent<Canvas>();
        helpMenu.enabled = false;
        playMenu.enabled = false;
        numberAI.text = " " + numberA.ToString();
        numberPlayer.text = " " + numberP.ToString();
    }

    // Update is called once per frame
    public void PlayMenu()
    {
        playMenu.enabled = true;
    }

    public void StartGame()
    {
        GameState.Instance.ResetGameState();
        SceneManager.LoadScene("ludo");
    }
    public void Quit()
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
    public void DecreasePlayer()
    {
        if (numberP > 2 && numberP <= 4 && numberP > (numberA + 1))
        {
            numberP--;
            decreasePlayerButton.enabled = true;
            GameState.Instance.NUMBER_OF_PLAYERS = numberP;
        }
        //else decreasePlayerButton.enabled = false;
        numberPlayer.text = " " + numberP.ToString();
    }
    public void IncreasePlayer()
    {
        if (numberP < 4 && numberP >= 2)
        {
            numberP++;
            increasePlayerButton.enabled = true;
            GameState.Instance.NUMBER_OF_PLAYERS = numberP;
        }
        //else increasePlayerButton.enabled = false;
        numberPlayer.text = " " + numberP.ToString();
    }
    public void IncreaseAI()
    {
        if (numberA < (numberP - 1) && numberA >= 0)        //at least 1 human player
        {
            numberA++;
            increaseAIButton.enabled = true;
            GameState.Instance.NUMBER_OF_BOTS = numberA;
        }
        numberAI.text = " " + numberA.ToString();
    }
    public void DecreaseAI()
    {
        if (numberA > 0 && numberA < numberP)
        {
            numberA--;
            decreaseAIButton.enabled = true;
            GameState.Instance.NUMBER_OF_BOTS = numberA;
        }
        numberAI.text = " " + numberA.ToString();
    }
}
