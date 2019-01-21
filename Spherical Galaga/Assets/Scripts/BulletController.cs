using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float lifeTime = 10.0f;
    public int damage = 1;

	// Use this for initialization
	void Start () {
        Invoke("DestroyBullet", lifeTime);
	}
	
	// Update is called once per frame
	void OnCollisionEnter (Collision collision) {
        GameObject otherObject = collision.gameObject;
        if (otherObject.tag == "Enemy") {
            EnemyController ec = otherObject.GetComponent<EnemyController>();
            ec.GetHit(gameObject);
        }
        else if (otherObject.tag == "Player") {
            PlayerController pc = otherObject.GetComponent<PlayerController>();
            pc.GetHit(gameObject);
        }
        DestroyBullet();
	}

    void DestroyBullet() {
        Destroy(gameObject);
    }
}
