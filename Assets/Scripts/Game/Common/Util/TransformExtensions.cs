using UnityEngine;

namespace Game.Common.Util
{
    public static class TransformExtensions
    {
        public static Vector2 toXZ(this Vector3 vec)
        {
            return new Vector2(vec.x, vec.z);
        }
        public static Vector3 toSimulationPlane(this Vector3 vec)
        {
            return new Vector3(vec.x, vec.z, vec.y);
        }
    }
}