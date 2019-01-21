using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnDeath(Health health);

public class Health : MonoBehaviour {

    public event OnDeath OnDeath;

    public int remainingHealth = 2;
    public AudioClip deathClip;
    public GameObject deathAnimation;

    public bool WillThisBulletKillIt(int damage) {
        return remainingHealth <= damage;
    }

    public void Damaged(int damageAmount) {
        remainingHealth -= damageAmount;
        if (remainingHealth <= 0) {
            OnHealthDepleted();
        }
    }

    private void OnHealthDepleted()
    {
        if(OnDeath != null)
        {
            OnDeath(this);
        }

        if (deathClip != null)
        {
            AudioManager.instance.PlaySoundEffect(deathClip);
        }

        if (deathAnimation)
        {
            Instantiate(deathAnimation, transform.position, Quaternion.Euler(transform.eulerAngles));
        }

        Destroy(gameObject);
    }
}
