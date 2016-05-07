using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    public Sprite AudioOn_Normal;
    public Sprite AudioOff_Normal;
    public Sprite AudioOn_Hover;
    public Sprite AudioOff_Hover;
    private Sprite currentSprite;
    private AudioListener audioListener;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioListener = GetComponent<AudioListener>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        currentSprite = GameState.Instance.Audio ? AudioOn_Normal : AudioOff_Normal;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.Instance.Audio)
        {
            audioSource.enabled = true;
            audioListener.enabled = true;
        }
        else
        {
            audioSource.enabled = false;
            audioListener.enabled = false;
        }
        spriteRenderer.sprite = currentSprite;
    }

    void OnMouseEnter()
    {
        currentSprite = GameState.Instance.Audio ? AudioOn_Hover : AudioOff_Hover;
    }

    void OnMouseExit()
    {
        currentSprite = GameState.Instance.Audio ? AudioOn_Normal : AudioOff_Normal;
    }

    void OnMouseDown()
    {
        currentSprite = (currentSprite == AudioOn_Hover) ? AudioOff_Hover : AudioOn_Hover;
        GameState.Instance.Audio = !GameState.Instance.Audio;
    }

}
