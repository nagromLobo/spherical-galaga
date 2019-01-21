using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance = null;
    public AudioSource musicSource;
    public AudioSource soundEffectSource;

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
	
	public void PlayMusic(AudioClip clip) {
        soundEffectSource.clip = clip;
        soundEffectSource.Play();
    }

    public void PlaySoundEffect(AudioClip clip) {
        soundEffectSource.clip = clip;
        soundEffectSource.Play();
    }
}
