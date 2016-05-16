using UnityEngine;
using System.Collections;

public class HelpBtn : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    public Sprite Normal;
    public Sprite OnHover;

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
        GameState.Instance.InGameHelp = !GameState.Instance.InGameHelp;
    }

}
