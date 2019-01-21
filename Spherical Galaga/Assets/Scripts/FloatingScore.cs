using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingScore : MonoBehaviour {

    private TextMesh tm;
    private Vector3 rotationAngle;

    private void Awake()
    {
        tm = GetComponent<TextMesh>();
    }

    // Use this for initialization
    void Start () {
        rotationAngle = new Vector3(0, 0, -20);
        float delay = 3.0f;
        StartCoroutine(RotateToAngle(rotationAngle, delay));
        StartCoroutine(ChangeAlphaValue(0, delay));
        Invoke("DestroyThis", delay);
    }

    public void SetPoints(int points) {
        tm.text = points.ToString();
    }

    IEnumerator RotateToAngle(Vector3 newAngle, float time) {
        float elapsedTime = 0;
        Vector3 startingAngle = transform.localEulerAngles;
        while (elapsedTime < time) {
            transform.localEulerAngles = Vector3.Lerp(startingAngle, newAngle, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator ChangeAlphaValue(float newAlpha, float time) {
        float elapsedTime = 0;
        float startingAlpha = tm.color.a;
        while (elapsedTime < time) {
            tm.color = new Color(tm.color.r, tm.color.g, tm.color.b, Mathf.Lerp(startingAlpha, newAlpha, (elapsedTime / time)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    void DestroyThis() {
        Destroy(gameObject);
    }
}
