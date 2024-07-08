using UnityEngine;

namespace SpacetimeDB.Types
{
    public static class StdbQuaternionExtensions
    {
        public static Quaternion ToUnityQuaternion(this StdbQuaternion stdbQuat)
        {
            return new Quaternion(stdbQuat.X, stdbQuat.Y, stdbQuat.Z, stdbQuat.W);
        }
    }
}
