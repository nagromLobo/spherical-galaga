using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public static bool isGameOver = false;

    void Update() {
        if (isGameOver) {
            if (Input.GetKeyDown(KeyCode.Return)) {
                SceneManager.LoadScene("Main");
            }
        }
    }

    public void LoadGame() {
        SceneManager.LoadScene("Main");
    }
}
