using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour {

    public Sprite[] sprites;
    public float animationInterval;

    private SpriteRenderer sr;
    private int currentSpriteIndex;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        Invoke("ChangeSprite", animationInterval);
	}
	
	void ChangeSprite() {
        currentSpriteIndex += 1;
        if (currentSpriteIndex >= sprites.Length - 1) {
            Destroy(gameObject);
        }
        sr.sprite = sprites[currentSpriteIndex];
        Invoke("ChangeSprite", animationInterval);
    }
}
