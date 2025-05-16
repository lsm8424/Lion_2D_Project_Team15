using UnityEngine;

public static class EventFunctionTracker
{
    public static bool IsEventRunning { get; private set; }

    public static void BeginEvent() => IsEventRunning = true;

    public static void EndEvent() => IsEventRunning = false;
}
