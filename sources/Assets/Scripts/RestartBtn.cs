using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RestartBtn : MonoBehaviour
{

    public Sprite Normal;
    public Sprite OnHover;
    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start ()
	{
	    spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

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
        GameState.Instance.ResetGameState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
