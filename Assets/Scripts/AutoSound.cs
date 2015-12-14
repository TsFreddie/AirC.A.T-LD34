using UnityEngine;
using System.Collections;

public class AutoSound : MonoBehaviour {
    public AudioClip _startMusic;
    private AudioSource audio;

	void Start () {
        audio = GetComponent<AudioSource>();
        audio.PlayOneShot(_startMusic);
        audio.PlayDelayed(8.599f);
    }
}
