using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class timer : MonoBehaviour
{
    public TMP_Text timerText;
    public int secondsToLive = 5;
    private DateTime start;
    private int timeLeft=1; //Set to 1 to bypass the conditional in Update loop
    private bool started = false;
    public void startTimer()
    {
        started = true;
        Debug.Log("Started");
    }
    public void stopTimer()
    {
        started = false;
        Debug.Log("Stopped!");
    }
    public void resetTimer()
    {
        timerText.text = secondsToLive.ToString();
        timeLeft = 1;
    }
    private void OnEnable()
    {
        Debug.Log("Enabling timer");
        start = DateTime.Now;
        startTimer();
    }
    // Update is called once per frame
    void Update()
    {
        if (started) {
            if (timeLeft <= 0)
            {
                timerText.text = "Time's Up!";
                stopTimer();
            }
            else
            {
                timeLeft = (secondsToLive - DateTime.Now.Subtract(start).Seconds);
                timerText.text = timeLeft.ToString();
            }
        }
    }
}
