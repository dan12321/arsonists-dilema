using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper methods for time
/// </summary>
public class Timers
{
    /// <summary>
    /// Calls the call back with false, waits then calls it with true
    /// </summary>
    /// <param name="time">The amount of time to wait</param>
    /// <param name="callBack">The method to call</param>
    public static IEnumerator BoolTimer(float time, System.Action<bool> callBack)
    {
        callBack(false);
        yield return new WaitForSeconds(time);
        callBack(true);
    }
}
