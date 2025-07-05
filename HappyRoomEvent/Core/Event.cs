using MEC;
using System.Collections.Generic;

namespace HappyRoomEvent.Core;

public abstract class Event
{
    protected CoroutineHandle WaitForEndCoroutine;
    protected CoroutineHandle LogicUpdateCoroutine;

    public virtual bool IsActive => WaitForEndCoroutine.IsRunning || LogicUpdateCoroutine.IsRunning;

    public abstract bool AreEndConditionsCompleted { get; }

    public virtual void StartEvent()
    {
        WaitForEndCoroutine = Timing.RunCoroutine(WaitForEnd());
        LogicUpdateCoroutine = Timing.RunCoroutine(Update());
    }

    public virtual void EndEvent() =>
        Timing.KillCoroutines(WaitForEndCoroutine, LogicUpdateCoroutine);

    protected abstract IEnumerator<float> Update();

    protected IEnumerator<float> WaitForEnd()
    {
        yield return Timing.WaitForOneFrame;

        while (!AreEndConditionsCompleted)
            yield return Timing.WaitForSeconds(0.3f);

        EndEvent();
    }
}
