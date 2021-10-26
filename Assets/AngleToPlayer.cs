using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class AngleToPlayer
{
    public static bool IsAngleInPlayerDirection(Vector3 nextPosition, Transform target, Transform transform) {
        if (Vector3.Distance(nextPosition, target.position) > Vector3.Distance(transform.position, target.position))
            return true;
        else return false;
    }
}
