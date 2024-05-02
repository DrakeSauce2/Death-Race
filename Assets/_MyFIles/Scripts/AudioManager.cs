using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource deathAudioSource;
    [SerializeField] private AudioSource collisionAudioSource;


    [Header("Audio Clips")]
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip collisionSFX;

    bool isEnabled = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Update()
    {
        if(GameManager.Instance.HasGameStarted() == false)
        {
            deathAudioSource.Stop();
            collisionAudioSource.Stop();
        }
    }

    public void PlayDeathSFX()
    {
        if (isEnabled == false) return;

        deathAudioSource.loop = false;
        deathAudioSource.clip = deathSFX;
        deathAudioSource.Play();
    }

    public void PlayCollisionSFX()
    {
        if (isEnabled == false) return;

        collisionAudioSource.loop = false;
        collisionAudioSource.clip = collisionSFX;
        collisionAudioSource.Play();
    }
}
