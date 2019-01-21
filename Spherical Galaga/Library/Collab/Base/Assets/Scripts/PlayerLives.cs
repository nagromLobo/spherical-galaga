using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLives : MonoBehaviour {

    public GameObject lifePrefab;
    public GameObject playerPrefab;
    public GameObject planet;
    public int numberOfLives = 3;
    public float invincibleDuration = 1.0f;
    public AudioClip playerSpawnClip;

    private GameObject[] playerLives;

	// Use this for initialization
	void Start () {
        playerLives = new GameObject[numberOfLives];
        Vector3 firtLifePosition = new Vector3(5, -2.6f, 5);
        Vector3 deltaPosition = new Vector3(0.5f, 0, 0);
        for (int i = 0; i < numberOfLives; ++i) {
            Transform cameraTransform = Camera.main.transform;
            GameObject newLife = Instantiate(lifePrefab, cameraTransform);
            newLife.transform.localPosition = firtLifePosition - i * deltaPosition;
            playerLives[i] = newLife;
        }

        numberOfLives -= 1;
        MovePlayerLifeToCenter();
    }
	
	public void TakeALife () {
        if (numberOfLives == 0) { 
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies) {
                enemy.GetComponent<SpherePhysics>().enabled = false;
                Vector3 explodingVelocity = enemy.transform.position - planet.transform.position;
                Rigidbody rb = enemy.GetComponent<Rigidbody>();
                rb.useGravity = true;
                rb.velocity = explodingVelocity;
            }

            GameObject.Find("GameOverText").GetComponent<Text>().enabled = true;
            GameObject.Find("PlayAgainText").GetComponent<Text>().enabled = true;
            SceneController.isGameOver = true;
        }
        else {
            Camera.main.GetComponent<CameraShake>().enabled = true;
            Camera.main.GetComponent<CameraShake>().shakeAmount = 0.5f;
            Camera.main.GetComponent<CameraShake>().shakeDuration = 0.5f;
            numberOfLives -= 1;
            Invoke("MovePlayerLifeToCenter", 2f);
        }
    }

    private void MovePlayerLifeToCenter() {
        float duration = 1.5f;
        StartCoroutine(MoveToPosition(new Vector3(0, 0, 3), duration));
        Invoke("MovePlayerLifeToPlanet", duration);
    }

    private void MovePlayerLifeToPlanet() {
        float duration = 0.5f;
        StartCoroutine(MoveToPosition(new Vector3(0, 0, 5), duration));
        Invoke("SpawnNewPlayer", duration);
    }

    IEnumerator MoveToPosition(Vector3 newPosition, float time) {
        float elapsedTime = 0;
        Vector3 startingPos = playerLives[numberOfLives].transform.localPosition;
        while (elapsedTime < time) {
            playerLives[numberOfLives].transform.localPosition = Vector3.Lerp(startingPos, newPosition, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void SpawnNewPlayer() {
        GameObject newPlayer = Instantiate(playerPrefab, playerLives[numberOfLives].transform.position, Quaternion.identity);
        playerLives[numberOfLives].GetComponent<SpriteRenderer>().enabled = false;
        SpherePhysics playerSpherePhysics = newPlayer.GetComponent<SpherePhysics>();
        playerSpherePhysics.gravity = -1f;
        Invincible invincible = newPlayer.GetComponent<Invincible>();
        invincible.GoInvincible(invincibleDuration);

        if (playerSpawnClip) {
            AudioManager.instance.PlaySoundEffect(playerSpawnClip);
        }
    }
    
}
