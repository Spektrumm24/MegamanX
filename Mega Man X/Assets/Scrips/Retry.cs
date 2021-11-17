using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    [SerializeField] AudioClip sfxClick;
    public void LoadScene (string escena)
    {
        SceneManager.LoadScene(escena);
    }
    public void Musica(AudioClip audio)
    {
        AudioSource.PlayClipAtPoint(audio, Camera.main.transform.position);
    }
}
