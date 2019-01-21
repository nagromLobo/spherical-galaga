using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalBroadsider : MonoBehaviour {

    public float thetaRange = 0.1f;
    public float rhoRange = 0.1f;

    SpherePhysics thisSP;
    SpherePhysics playerSP;
    FireBullet fireBullet;

    private bool canShoot = true;

	// Use this for initialization
	void Start () {
        fireBullet = GetComponent<FireBullet>();
        thisSP = GetComponent<SpherePhysics>();
        playerSP = GameManager.instance.GetComponent<PlayerLives>().currentPlayer.GetComponent<SpherePhysics>();
	}
	
	// Update is called once per frame
	void Update () {

        var currentPlayer = GameManager.instance.Player;
      
        if (currentPlayer != null && canShoot && thisSP.isOnSphere) {
            playerSP = currentPlayer.GetComponent<SpherePhysics>();
            float thisRho = Mathf.Repeat(thisSP.rho, 2 * Mathf.PI);
            float thisTheta = Mathf.Repeat(thisSP.theta, Mathf.PI);
            float playerRho = Mathf.Repeat(playerSP.rho, 2 * Mathf.PI);
            float playerTheta = Mathf.Repeat(playerSP.theta, Mathf.PI);
            if (playerRho > (thisRho - rhoRange) && playerRho < (thisRho + rhoRange)) {
                if (playerTheta > thisTheta) {
                    canShoot = false;
                    Debug.Log("Shooting up");
                    fireBullet.SetSpawnLocation(transform.GetChild(0).transform.position);
                    fireBullet.Fire(Vector2.up, thisSP);
                }
                else {
                    Debug.Log("Shooting down");
                    canShoot = false;
                    fireBullet.SetSpawnLocation(transform.GetChild(1).transform.position);
                    fireBullet.Fire(Vector2.down, thisSP);
                }
                Invoke("Reload", 1f);
            }
        }
        
	}

    void Reload()
    {
        canShoot = true;
    }
}
