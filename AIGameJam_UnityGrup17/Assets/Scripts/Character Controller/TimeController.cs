using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static scr_Models;

public class TimeController : MonoBehaviour
{
    public float slowDownFactor = 0.5f;
    public float speedUpFactor = 2f;


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
    }

    void SpeedUpTime()
    {
        Time.timeScale = speedUpFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    void ResetTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}
