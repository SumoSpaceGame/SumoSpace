using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vec3Util {
    public static Vector3 Vec2ToXZ(Vector2 xyVec) {
        return new Vector3(xyVec.x, 0f, xyVec.y);
    }

    public static Vector3 Vec2ToVec3Z(Vector2 xyVec, float z) {
        return new Vector3(xyVec.x, xyVec.y, z);
    }
}
