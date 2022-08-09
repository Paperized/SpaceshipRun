using UnityEngine;

public static class EulerAngleExtension
{
    public static float GetZEulerAngle(this Quaternion quaternion)
    {
        return (quaternion.eulerAngles.z > 180) ? quaternion.eulerAngles.z - 360 : quaternion.eulerAngles.z;
    }
}
