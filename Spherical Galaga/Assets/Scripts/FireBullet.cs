using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour {

    public GameObject bulletObject;
    public Sprite bulletSprite;
    public AudioClip bulletClip;
    public int speedMagnitude = 1;

    private Vector3 spawnLocation;

    public void SetSpawnLocation(Vector3 location) {
        spawnLocation = location;
    }

    // Fire a bullet in the north direction
    public void Fire(Vector2 direction, SpherePhysics shooterSP) {
        bulletObject.GetComponent<SpriteRenderer>().sprite = bulletSprite;

        if (bulletClip) {
            AudioManager.instance.PlaySoundEffect(bulletClip);
        }

        var control = direction * speedMagnitude;
        Quaternion bulletOrientation = Quaternion.identity;
        GameObject newBullet = Instantiate(bulletObject, spawnLocation, bulletOrientation);
        SpherePhysics sp = newBullet.GetComponent<SpherePhysics>();
        if (direction == Vector2.right || direction == Vector2.left) {
            sp.rho = shooterSP.rho + direction.x / 10f;
        }
        else {
            sp.rho = shooterSP.rho;
        }

        if (direction == Vector2.up || direction == Vector2.down) {
            sp.theta = shooterSP.theta + direction.y / 10f;
        }
        else {
            sp.theta = shooterSP.theta;
        }
        
        sp.velocity = control;
    }
}
