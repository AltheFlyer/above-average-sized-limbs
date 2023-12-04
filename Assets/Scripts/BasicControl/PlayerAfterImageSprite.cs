using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    public float activeTime = 0.1f;
    private float timeActivated;
    private float alpha;

    public float alphaSet = 0.8f;
    private float alphaMultiplier = 0.85f;

    private Transform player;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer playerSpriteRenderer;
    private Color color;

    private void OnEnable()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;

        spriteRenderer.sprite = playerSpriteRenderer.sprite;

        transform.position = player.position;
        transform.rotation = player.rotation;
        timeActivated = Time.time;
    }

    public void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(1f, 1f, 1f, alpha);
        // color = Color.HSVToRGB(Random.Range(0f, 1f), 1, 1);
        // color.a = alpha;
        spriteRenderer.color = color;

        if (Time.time >= (timeActivated + activeTime))
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }

}
