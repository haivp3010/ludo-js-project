using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlowManager : MonoBehaviour {

    public List<Renderer> horseRenderers = new List<Renderer>();
    public List<Renderer> diceRenderers = new List<Renderer>();
    private OutlineEffect outlineEffect;
    public float Count = 0.0f;
    public float GlowingSpeed = 13;
    private bool increasing = true;

    // Use this for initialization
    void Start () {
        outlineEffect = GetComponent<OutlineEffect>();
	}
	
	// Update is called once per frame
	void Update () {
        //
        // If dice are not rolled, glow at dice
        if (!GameState.Instance.DiceRolled && !GameState.Instance.HorseMoving)
        {
            outlineEffect.outlineRenderers.Clear();
            outlineEffect.outlineRenderers.Capacity = 0;
            outlineEffect.outlineRenderers.Add(diceRenderers[0]);
            outlineEffect.outlineRenderers.Add(diceRenderers[1]);
        }
        else if (!GameState.Instance.HorseMoving)
        {
            outlineEffect.outlineRenderers.Clear();
            outlineEffect.outlineRenderers.Capacity = 0;
            for (int i = 0; i <= 15; i++)
            {
                if (GameState.Instance.Movable[i] != MoveCase.Immovable)
                {
                    outlineEffect.outlineRenderers.Add(horseRenderers[i]);
                }
            }
        }
        else
        {
            outlineEffect.outlineRenderers.Clear();
            outlineEffect.outlineRenderers.Capacity = 0;
        }

        GlowingEffect();
    }

    void GlowingEffect()
    {
        if (Count >= 5.0f)
            increasing = false;

        if (Count <= 0.0f)
            increasing = true;

        if (increasing)
            Count += Time.deltaTime * GlowingSpeed;
        else
            Count -= Time.deltaTime * GlowingSpeed;

        outlineEffect.lineThickness = Count;
    }
}
