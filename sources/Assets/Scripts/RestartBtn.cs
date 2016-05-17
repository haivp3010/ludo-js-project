using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RestartBtn : MonoBehaviour
{
    public string Type;
    public Sprite Normal;
    public Sprite OnHover;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D btnCollider;

	// Use this for initialization
	void Start ()
	{
	    spriteRenderer = GetComponent<SpriteRenderer>();
	    btnCollider = GetComponent<CircleCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (GameState.Instance.Winner != HorseColor.None && Type.Equals("In-game"))
        {
            spriteRenderer.enabled = false;
            btnCollider.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
            btnCollider.enabled = true;
        }
	    switch (Type)
	    {
            case "In-game":
	            spriteRenderer.enabled = btnCollider.enabled = GameState.Instance.Winner == HorseColor.None;
	            break;
            case "Winning":
                spriteRenderer.enabled = btnCollider.enabled = GameState.Instance.Winner != HorseColor.None;
	            break;
	    }
    }

    void OnMouseEnter()
    {
        spriteRenderer.sprite = OnHover;
    }

    void OnMouseExit()
    {
        spriteRenderer.sprite = Normal;
    }

    void OnMouseDown()
    {
        GameState.Instance.NUMBER_OF_PLAYERS = 4;
        GameState.Instance.ResetGameState();
        SceneManager.LoadScene("Menu");
    }
}
