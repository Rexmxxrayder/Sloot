using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationCurveExtensions {
    public static float GetDuration(this AnimationCurve curve) {
        return curve.keys[^1].time - curve.keys[0].time;
    }

    public static int GetHigherKey(this AnimationCurve curve) {
        int higherKey = 0;
        for (int i = 0; i < curve.keys.Length; i++) {
            if(curve.keys[0].value < curve.keys[i].value) {
                higherKey = i;
            }
        }

        return higherKey;
    }

    public static void ChangeDuration(this AnimationCurve curve, float newDuration) {
        Keyframe[] keys = curve.keys;
        for (int i = 0; i < curve.length; i++) {
            keys[i].time /= GetDuration(curve);
            keys[i].time *= newDuration;
        }
        curve.keys = keys;
    }

    public static void ChangeHeight(AnimationCurve curve, float newHeight) {
        Keyframe[] keys = curve.keys;
        for (int i = 0; i < curve.length; i++) {
            keys[i].value /= GetHigherKey(curve);
            keys[i].time *= newHeight;
        }
        curve.keys = keys;
    }
}

public static class Vector3Extensions {
    public static Vector3 PlanXZ(this Vector3 curve) {
        return new Vector3(curve.x, 0, curve.z);
    }
}