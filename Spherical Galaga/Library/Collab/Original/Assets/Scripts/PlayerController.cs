using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    FireBullet fireBullet;
    SpherePhysics physics;

    public float Speed = 2f;
    public Transform cameraTransform;

	// Use this for initialization
	void Start () {
        fireBullet = GetComponent<FireBullet>();
        physics = GetComponent<SpherePhysics>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            fireBullet.Fire(Vector3.up);
        }


        var control = Vector2.zero;

        if(Input.GetKey(KeyCode.UpArrow))
        {
            control.y = 1;
        } else if(Input.GetKey(KeyCode.DownArrow))
        {
            control.y = -1;
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            control.x = -1;
        } else if(Input.GetKey(KeyCode.RightArrow))
        {
            control.x = 1;
        }

        physics.velocity = control.normalized * Speed;

        //cameraTransform.position = transform.position + 5*transform.position.normalized;
        //cameraTransform.LookAt(transform);
        transform.LookAt(physics.planet.transform);
    }

    private void OnDestroy() {
        Transform cameraTranform = transform.GetChild(0);
        cameraTranform.parent = null;
        cameraTranform.gameObject.GetComponent<Camera>().enabled = true;
        cameraTranform.gameObject.GetComponent<Skybox>().enabled = true;
        cameraTranform.gameObject.GetComponent<AudioListener>().enabled = true;

        GameObject planet = GameObject.FindGameObjectWithTag("Planet");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            Vector3 explodingVelocity = enemy.transform.position - planet.transform.position;
            Rigidbody rb = enemy.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.velocity = explodingVelocity;
        }

        GameObject.Find("GameOverText").GetComponent<Text>().enabled = true;
        GameObject.Find("PlayAgainText").GetComponent<Text>().enabled = true;
        SceneController.isGameOver = true;
    }
}
