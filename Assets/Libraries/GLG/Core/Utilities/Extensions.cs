using System.Collections.Generic;
using UnityEngine;

public static class GLGExtensions
{
    public static T GetRandom<T>(this T[] array)
    {
        return array.Length > 0 ? array[Random.Range(0, array.Length)] : default;
    }

    public static T GetRandom<T>(this List<T> array)
    {
        return array.Count > 0 ? array[Random.Range(0, array.Count)] : default;
    }

    public static bool Approximately(this Quaternion quatA, Quaternion value, float acceptableRange)
    {
        return 1f - Mathf.Abs(Quaternion.Dot(quatA, value)) < acceptableRange;
    }

    public static bool IsCloserThan(this Vector3 self, Vector3 point, float distance)
    {
        return (self - point).sqrMagnitude < distance * distance;
    }

    public static bool IsCloserThan(this Vector2 self, Vector2 point, float distance)
    {
        return (self - point).sqrMagnitude < distance * distance;
    }

    public static bool IsCloserThan(this Vector4 self, Vector4 point, float distance)
    {
        return (self - point).sqrMagnitude < distance * distance;
    }

    public static Vector3 Right(this Vector3 self)
    {
        return Vector3.Cross(self, Vector3.up).normalized;
    }

    public static Vector3 Left(this Vector3 self)
    {
        return -Vector3.Cross(self, Vector3.up).normalized;
    }

    public static Vector3 Up(this Vector3 self)
    {
        return -Vector3.Cross(self, self.Right()).normalized;
    }

    public static Vector3 Down(this Vector3 self)
    {
        return -Vector3.Cross(self, self.Right()).normalized;
    }
}