using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HappyRoomEvent.Tools;

public class CountdownTimer
{
    private CoroutineHandle _timerCoroutine;

    public float RemainingTime
    {
        get;
        set => field = value > 0 ? value : 0;
    }

    public bool IsRunning => _timerCoroutine.IsRunning;

    public CountdownTimer() { }

    public CountdownTimer(float initialTime) => RemainingTime = initialTime;

    public void Start()
    {
        if (IsRunning)
            return;

        _timerCoroutine = Timing.RunCoroutine(Countdown());
    }

    public void Stop()
    {
        RemainingTime = 0f;
        Timing.KillCoroutines(_timerCoroutine);
    }

    private IEnumerator<float> Countdown()
    {
        while (RemainingTime > 0f)
        {
            yield return Timing.WaitForOneFrame;
            RemainingTime -= Time.deltaTime;
        }
    }

    public override string ToString() => TimeSpan.FromSeconds(RemainingTime).ToString();
}
