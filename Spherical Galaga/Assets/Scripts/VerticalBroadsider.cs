using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalBroadsider : MonoBehaviour {

    public float thetaRange = 0.1f;
    public float rhoRange = 0.1f;
    public float firingChance = 0.5f;

    SpherePhysics thisSP;
    FireBullet fireBullet;

    private bool canShoot = true;

    // Use this for initialization
    void Start()
    {
        fireBullet = GetComponent<FireBullet>();
        thisSP = GetComponent<SpherePhysics>();
    }

    // Update is called once per frame
    void Update()
    {

        var currentPlayer = GameManager.instance.Player;
   
        if (currentPlayer != null && canShoot && thisSP.isOnSphere)
        {
            var playerSP = currentPlayer.GetComponent<SpherePhysics>();
            float thisRho = Mathf.Repeat(thisSP.onPositiveSide ? thisSP.rho - Mathf.PI : thisSP.rho, 2 * Mathf.PI);
            float thisTheta = Mathf.Repeat(thisSP.onPositiveSide ? Mathf.PI - thisSP.theta : thisSP.theta, Mathf.PI);
            float playerRho = Mathf.Repeat(playerSP.onPositiveSide ? playerSP.rho - Mathf.PI : playerSP.rho, 2 * Mathf.PI);
            float playerTheta = Mathf.Repeat(playerSP.onPositiveSide ? Mathf.PI - playerSP.theta : playerSP.theta, Mathf.PI);
            Debug.Log("this r:t " + thisRho + " " + thisTheta + " player r:t " + playerTheta + " " + playerRho);
            if (playerTheta > (thisTheta - thetaRange) && playerTheta < (thisTheta + thetaRange)) {
                canShoot = false;
                float randomizer = Random.Range(0f, 10f);
                if (randomizer <= firingChance) {
                    if (playerRho < thisRho) {
                        // 0 is right
                        // 1 is left
                        Debug.Log("Shooting right");
                        fireBullet.SetSpawnLocation(transform.GetChild(thisSP.onPositiveSide ? 0 : 1).transform.position);
                        fireBullet.Fire(Vector2.right, thisSP);
                    }
                    else {
                        Debug.Log("Shooting left");
                        fireBullet.SetSpawnLocation(transform.GetChild(thisSP.onPositiveSide ? 1 : 0).transform.position);
                        fireBullet.Fire(Vector2.left, thisSP);
                    }
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
