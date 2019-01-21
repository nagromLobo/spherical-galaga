using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnDeath(Health health);

public class Health : MonoBehaviour {

    public event OnDeath OnDeath;

    public int remainingHealth = 2;
    public AudioClip deathClip;
    public GameObject deathAnimation;

    public float damageAnimationDuration = 0.5f;
    public float damageAnimationPeriod = 0.1f;
    public Color damageAnimationColor = Color.red;

    private SpriteRenderer spriteRenderer;

    private Color origColor;
    private float damageAnimationStartTime;

    public void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        damageAnimationStartTime = -damageAnimationDuration;
    }


    public bool WillThisBulletKillIt(int damage) {
        return remainingHealth <= damage;
   
    }

    public void Damaged(int damageAmount) {
        remainingHealth -= damageAmount;
        if (remainingHealth <= 0) {
            OnHealthDepleted();
        } else {
            StartDamageAnimation();
        }
    }

    private void OnHealthDepleted() {

        if(OnDeath != null) {
            OnDeath(this);
        }

        if (deathClip != null) {
            AudioManager.instance.PlaySoundEffect(deathClip);
        }

        if (deathAnimation) {
            Instantiate(deathAnimation, transform.position, Quaternion.Euler(transform.eulerAngles));
        }

        Destroy(gameObject);
    }

    public void StartDamageAnimation() {
        var wasAlreadyAnimating = IsDamageAnimating;
        damageAnimationStartTime = Time.time;
        if (!wasAlreadyAnimating) {
            origColor = spriteRenderer.color;
            spriteRenderer.color = damageAnimationColor;
            Invoke("UpdateDamageAnimation", damageAnimationPeriod);
        }
    }

    public void UpdateDamageAnimation() {
        if(IsDamageAnimating) {
            spriteRenderer.color = spriteRenderer.color == damageAnimationColor ?
                origColor : 
                damageAnimationColor;
            Invoke("UpdateDamageAnimation", damageAnimationPeriod);
        } else {
            damageAnimationStartTime = 0.0f;
            spriteRenderer.color = origColor;
        }
    }

    protected bool IsDamageAnimating {
        get {
            //Debug.Log("dt: " + (Time.time - damageAnimationStartTime));
            return (Time.time - damageAnimationStartTime) < damageAnimationDuration;
        }
    }
}
