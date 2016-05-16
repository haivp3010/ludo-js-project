using UnityEngine;
using System.Collections;

public class WinningExit : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private CircleCollider2D btnCollider;
    public Sprite Normal;
    public Sprite OnHover;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        btnCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.enabled = btnCollider.enabled = GameState.Instance.Winner != HorseColor.None;
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
        
        Application.Quit();
    }
}
