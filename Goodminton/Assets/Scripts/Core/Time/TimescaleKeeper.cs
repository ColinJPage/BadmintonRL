using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the ONLY CLASS that is allowed to set Time.timescale!
/// </summary>
public class TimescaleKeeper : MonoBehaviour
{
    public BoolReference isPaused;
    public FloatModVariable timeScale = new FloatModVariable(1f);

    private FloatMultiplier pausedModifier = new FloatMultiplier(1f);

    private static float defaultFixedDeltaTime = 1 / 50f;

    public static TimescaleKeeper instance;
    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        //timeScale.Subscribe(OnTimeScaleChanged);
        timeScale.AddModifier(pausedModifier);
        isPaused.Subscribe(OnPauseChange);
        isPaused.Value = false;
    }

    private void OnDisable()
    {
        //timeScale.Unsubscribe(OnTimeScaleChanged);
        timeScale.RemoveModifier(pausedModifier);
        isPaused.Unsubscribe(OnPauseChange);
        isPaused.Value = false;
        SetTimeScale(1f);
    }

    private void Update() // weird that this is in Update(). But maybe fine?
    {
        OnTimeScaleChanged(timeScale);
    }

    void OnTimeScaleChanged(FloatModVariable timeScale)
    {
        SetTimeScale(timeScale.GetValue());
    }

    private void SetTimeScale(float v)
    {
        Time.timeScale = v;

        if (v >= 0.01f)
        {
            // Colin: We adjust fixed update speed when timeScale is less than 1.
            // I don't understand why, but things get jittery while (timescale < 1) if we don't do this.
            // Some people online say this is bad practice and may change the results of physics calculations,
            // and that rigidbody interpolation solves the jittering. That doesn't work in our case, for some reason.
            Time.fixedDeltaTime = Mathf.Max(0.001f, defaultFixedDeltaTime * Mathf.Min(1f, v));
        }
    }

    void OnPauseChange(bool newPause)
    {
        pausedModifier.SetFactor(newPause ? 0f : 1f);
    }
}
