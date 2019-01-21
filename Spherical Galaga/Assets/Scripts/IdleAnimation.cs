using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimation : MonoBehaviour {

    public float animationInterval = 0.1f;
    public Sprite[] sprites;

    private SpriteRenderer sr;
    private int currentSpriteIndex = 0;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        Invoke("ChangeSprite", animationInterval);
	}
	
	void ChangeSprite() {
        currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Length;
        sr.sprite = sprites[currentSpriteIndex];
        Invoke("ChangeSprite", animationInterval);
    }
}
