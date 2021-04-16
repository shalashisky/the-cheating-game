using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip[] menuSounds;
    // 0 - options menu select
    // 1 - wrong buzzer
    // 2 - Screen Transition


    public void PlaySound(int id)
    {
        audioSource.PlayOneShot(menuSounds[id], 1f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
