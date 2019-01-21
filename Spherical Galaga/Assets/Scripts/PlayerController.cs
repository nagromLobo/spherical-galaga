using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    FireBullet fireBullet;
    SpherePhysics physics;
    private Transform cameraTransform;
    private bool canFire = true;

    public float Speed = 2f;
    public float firingDelay = 0.2f;

    // Use this for initialization
    void Start () {
        cameraTransform = Camera.main.transform;
        fireBullet = GetComponent<FireBullet>();
        physics = GetComponent<SpherePhysics>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (canFire) {
                canFire = false;
                fireBullet.SetSpawnLocation(transform.GetChild(0).position);
                fireBullet.Fire(Vector2.up, physics);
                Invoke("ReloadAmmo", firingDelay);
            }
        }


        var control = Vector3.zero;

        if(Input.GetKey(KeyCode.UpArrow)) {
            control.y = 1;
        } else if(Input.GetKey(KeyCode.DownArrow)) {
            control.y = -1;
        }

        if(Input.GetKey(KeyCode.LeftArrow)) {
            control.x = -1;
        } else if(Input.GetKey(KeyCode.RightArrow)) {
            control.x = 1;
        }
        control = control.normalized * Speed;
        control.x = physics.IsNearPole() ? control.x/10f : control.x;


        physics.velocity = new Vector3(
            control.x,
            control.y,
            physics.velocity.z
        );

        //camera stuff
        cameraTransform.position = transform.position + 5*transform.position.normalized;
        cameraTransform.LookAt(transform.position, transform.up);
    }

    public void GetHit(GameObject bullet) {
        Health health = GetComponent<Health>();
        health.Damaged(1);
    }

    private void OnDestroy() {
        GameManager.instance.GetComponent<PlayerLives>().TakeALife();
    }

    private void ReloadAmmo() {
        canFire = true;
    }
}
