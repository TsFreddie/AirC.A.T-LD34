using UnityEngine;
using System.Collections;

public class MenuAudio : MonoBehaviour {

    public AudioClip _selectSound;

    public AudioSource audio;
    
    public void PlaySelectSound()
    {
        audio.PlayOneShot(_selectSound);
    }
    
    public void GotoScene(int id)
    {
        ResourceManager.Instance.LoadScene(id);
    }
}
