using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincible : MonoBehaviour {

    public float blinkDelay = 0.2f;

    private bool isInvincible;
    private SpriteRenderer sr;
    private BoxCollider bc;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider>();
    }

    public void GoInvincible(float duration) {
        isInvincible = true;
        bc.enabled = false;
        Blink();
        Invoke("StopInvincible", duration);
    }

	// Use this for initialization
	void Blink () {
        if (isInvincible) {
            sr.enabled = !sr.enabled;
            Invoke("Blink", blinkDelay);
        }
	}

    void StopInvincible() {
        sr.enabled = true;
        bc.enabled = true;
        isInvincible = false;
    }
	
	
}
