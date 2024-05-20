using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static scr_Models;

public class TimeController : MonoBehaviour
{
    public float slowDownFactor = 0.5f;
    public float speedUpFactor = 2f;
    public AudioSource audioSource;

    private void Awake()
    {
        // Eðer AudioSource ayný GameObject'te ise, bileþeni al
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SlowDownTime();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpeedUpTime();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetTime();
        }
    }

    void SlowDownTime()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        UpdateAudioPitch();
    }

    void SpeedUpTime()
    {
        Time.timeScale = speedUpFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        UpdateAudioPitch();
    }

    void ResetTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        UpdateAudioPitch();
    }

    void UpdateAudioPitch()
    {
        if (audioSource != null)
        {
            audioSource.pitch = Time.timeScale;
        }
    }
}
