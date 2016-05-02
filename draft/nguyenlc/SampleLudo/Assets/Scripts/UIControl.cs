using UnityEngine;
using UnityEngine.UI;
public class UIControl : MonoBehaviour {
    public Text diceValueText;
    public Text currentPlayerText;
    public Button buttonRoll;
    public Button buttonSkip;
    public Button buttonNewGame;
    public Image winnerPanel;
    public Text labelWinner;
    public Text textWinner;
    
    Color GetCorrespondingColor(HorseColor color)
    {
        switch(color)
        {
            case HorseColor.Red: return Color.red;
            case HorseColor.Blue: return Color.blue;
            case HorseColor.Yellow: return Color.yellow;
            case HorseColor.Green: return Color.green;
            default: return Color.white;
        }
    }
    void Start()
    {
        buttonRoll.onClick.AddListener(delegate { ButtonRoll_OnClick(); });
        buttonSkip.onClick.AddListener(delegate { ButtonSkip_OnClick(); });
        buttonNewGame.onClick.AddListener(delegate { ButtonNewGame_OnClick(); });
        HideWinnerPanel();
    }
    void Update()
    {
        currentPlayerText.text = GameState.Instance.CurrentPlayer.ToString();
        currentPlayerText.color = GetCorrespondingColor(GameState.Instance.CurrentPlayer);
        diceValueText.text = GameState.Instance.CurrentDiceValue.ToString();
        buttonRoll.interactable = (GameState.Instance.CurrentDiceValue == 0);
        buttonSkip.interactable = !buttonRoll.interactable;
        if (GameState.Instance.Winner != HorseColor.None)
            DisplayWinnerPanel();
    }
    void DisplayWinnerPanel()
    {
        this.enabled = false;
        winnerPanel.enabled = true;
        labelWinner.enabled = true;
        textWinner.enabled = true;
        buttonRoll.interactable = false;
        buttonSkip.interactable = false;
        textWinner.text = GameState.Instance.Winner.ToString();
        textWinner.color = GetCorrespondingColor(GameState.Instance.Winner);
    }
    void HideWinnerPanel()
    {
        winnerPanel.enabled = false;
        labelWinner.enabled = false;
        textWinner.enabled = false;
        winnerPanel.rectTransform.anchoredPosition = new Vector2(-400, 0);
        this.enabled = true;
    }
    public void ButtonRoll_OnClick()
    {
        GameState.Instance.CurrentDiceValue = Random.Range(1, 7); // Set dice value randomly from the range 0 - 6
    }
    public void ButtonSkip_OnClick()
    {
        GameState.Instance.ResetDiceAndChangePlayer();
    }
    public void ButtonNewGame_OnClick()
    {
        HideWinnerPanel();
        GameState.Instance.ResetGameState();
    }
}