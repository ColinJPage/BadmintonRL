using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForSecondsRealtimePauseable : CustomYieldInstruction
{
    private float secondsRemaining;
    public override bool keepWaiting
    {
        get
        {
            if (Time.timeScale > 0f) secondsRemaining -= Time.unscaledDeltaTime;

            return secondsRemaining > 0f;
        }
    }

    public WaitForSecondsRealtimePauseable(float seconds)
    {
        secondsRemaining = seconds;
    }
}
