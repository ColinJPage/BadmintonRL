using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*!This class limits the frequency that something can happen.
 * Call IsReady to check if cooldown is over, or constantly call Try() and the class will automatically call the action when cooldown finishes. 
 */
[System.Serializable]
public class Cooldown
{
    [Tooltip("When this thing was last triggered")]
    private double lastTime = 0;
    [Tooltip("Minimum amount of time before it can be triggered again")]
    public double cooldownTime {
        get
        {
            if(CooldownTime != null)
                return CooldownTime();
            return 0f;
        }
    }

    public delegate double GetCooldownTime();
    private GetCooldownTime CooldownTime;

    public void SetCooldownTime(float newCooldownTime)
    {
        CooldownTime = () => newCooldownTime;
    }

    public Cooldown(float _cooldownTime = 0f)
    {
        SetCooldownTime(_cooldownTime);
    }
    public Cooldown() : this(1f)
    {

    }
    public Cooldown(GetCooldownTime ct)
    {
        CooldownTime = ct;
    }

    public double TimeSinceTrigger => CurrentTime - lastTime;
    public bool IsAfterTime => TimeSinceTrigger >= cooldownTime;
    public bool IsBeforeTime => TimeSinceTrigger < cooldownTime;
    public double Progress => Mathf.Clamp((float)(TimeSinceTrigger / cooldownTime), 0f, 1f);

    public void Do()
    {
        Restart();
    }

    public bool TryDoIfAfterTime()
    {
        if (IsAfterTime)
        {
            Do();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Set the timer to the current time
    /// </summary>
    public void Restart()
    {
        lastTime = CurrentTime;
    }

    /// <summary>
    /// Reset the timer immediately so that IsAfterTime will be true on its next call
    /// </summary>
    public void Reset()
    {
        lastTime = float.MinValue;
    }


    protected virtual double CurrentTime => Time.realtimeSinceStartupAsDouble;
}

[System.Serializable]
public class CooldownScaledTime : Cooldown
{
    public CooldownScaledTime(float _cooldownTime = 0f) : base(_cooldownTime)
    {
        
    }
    public CooldownScaledTime(GetCooldownTime lambda) : base(lambda)
    {

    }
    protected override double CurrentTime => Time.timeAsDouble;
}