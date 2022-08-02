using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod
{
    public static bool IsFacingTarget(this Transform transform, Transform target)
    {
        const float dotThresshold = 0.5f;
        var vectorToTarget = target.position - transform.position;
        vectorToTarget.Normalize();
        float dot = Vector3.Dot(transform.forward, vectorToTarget); //(对方朝向，自己朝向)
        return dot >= dotThresshold;
    }
}