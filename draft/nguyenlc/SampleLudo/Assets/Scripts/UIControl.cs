using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIControl : MonoBehaviour {
    public Text diceValueText;
    public Text currentPlayerText;
    public Button buttonRoll;
    public Button buttonSkip;
    public Button buttonNewGame;
    void Start()
    {
        buttonRoll.onClick.AddListener(delegate { ButtonRoll_OnClick(); });
        buttonSkip.onClick.AddListener(delegate { ButtonSkip_OnClick(); });
        buttonNewGame.onClick.AddListener(delegate { ButtonNewGame_OnClick(); });
    }
    void Update()
    {
        currentPlayerText.text = GameState.Instance.CurrentPlayer.ToString();
        switch (GameState.Instance.CurrentPlayer)
        {
            case HorseColor.Red:
                currentPlayerText.color = Color.red;
                break;
            case HorseColor.Blue:
                currentPlayerText.color = Color.blue;
                break;
            case HorseColor.Yellow:
                currentPlayerText.color = Color.yellow;
                break;
            case HorseColor.Green:
                currentPlayerText.color = Color.green;
                break;
        };
        diceValueText.text = GameState.Instance.CurrentDiceValue.ToString();
        
    }
    public void ButtonRoll_OnClick()
    {
        Debug.Log("ButtonRoll clicked!");
    }
    public void ButtonSkip_OnClick()
    {
        Debug.Log("ButtonSkip clicked!");
    }
    public void ButtonNewGame_OnClick()
    {
        Debug.Log("ButtonNewGame clicked!");
    }
}