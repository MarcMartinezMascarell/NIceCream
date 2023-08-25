using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    //Play an specific sound effect when the player interacts with an item
    public void PlayItemSoundEffect(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
