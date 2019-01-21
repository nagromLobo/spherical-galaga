using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    private PlayerLives playerLives;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
        playerLives = GetComponent<PlayerLives>();
    }

    public GameObject Player {
        get {
            return playerLives.currentPlayer;
        }
    }
}
