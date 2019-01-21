using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public int pointsWorth = 0;
    public GameObject floatingScorePrefab;
    public bool canFire;
    public bool isFiring;
    public float delayBetweenBullets;
    FireBullet fireBullet;

	// Use this for initialization
	void Start () {
        fireBullet = GetComponent<FireBullet>();

        var health = GetComponent<Health>();
        health.OnDeath += OnDeath;
    }

    void Update() {
        if (canFire && !isFiring) {
            FireProjectile();
            Invoke("FiringDelay", delayBetweenBullets);
        }

        if (Vector3.Distance(transform.position, Planet.instance.transform.position) > 50) {
            pointsWorth = 0;
            Destroy(gameObject);
        }
    }

    void FiringDelay() {
        isFiring = false;
    }

    void FireProjectile() {
        isFiring = true;
        fireBullet.Fire(Vector3.down, GetComponent<SpherePhysics>());
    }

    public void GetHit(GameObject bullet) {
        Health health = GetComponent<Health>();
        if (bullet.tag == "EnemyBullet") {
            if (health.WillThisBulletKillIt(1)) {
                pointsWorth = 0;
            }
        }
        health.Damaged(1);
    }

    private void OnDeath(Health health) {
        if (GameManager.instance != null && pointsWorth != 0) {
            GameManager.instance.GetComponent<ScoreKeeper>().IncreaseScore(pointsWorth);
            GameObject newFloatingScore = Instantiate(floatingScorePrefab, transform.position, Quaternion.Euler(transform.rotation.eulerAngles));
            newFloatingScore.GetComponent<FloatingScore>().SetPoints(pointsWorth);
        }
    }

    void OnCollisionEnter(Collision collision) {
        GameObject otherObject = collision.gameObject;
        Health health = otherObject.GetComponent<Health>();
        if (health && otherObject.tag == "Player") {
            health.Damaged(1);
        }
    }
}
