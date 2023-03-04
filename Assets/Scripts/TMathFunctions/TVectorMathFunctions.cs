using UnityEngine;

namespace TMathFunctions
{
    public struct VMathfs
    {
        public static float SqrDistance(Vector3 a, Vector3 b)
        {
            float num = a.x - b.x;
            float num2 = a.y - b.y;
            float num3 = a.z - b.z;
            return Mathf.Pow(num, 2) + Mathf.Pow(num2, 2) + Mathf.Pow(num3, 2);
        }

        public static bool InRange(Vector3 a, Vector3 b, float dist)
        {
            float num = a.x - b.x;
            float num2 = a.y - b.y;
            float num3 = a.z - b.z;

            return Mathf.Pow(num, 2) + Mathf.Pow(num2, 2) + Mathf.Pow(num3, 2) <= Mathf.Pow(dist, 2);
        }
    }

    public struct V2Mathfs
    {
        public static float SqrDistance(Vector2 a, Vector2 b)
        {
            float num = a.x - b.x;
            float num2 = a.y - b.y;
            return Mathf.Pow(num, 2) + Mathf.Pow(num2, 2);
        }

        public static bool InRange(Vector3 a, Vector3 b, float dist)
        {
            float num = a.x - b.x;
            float num2 = a.y - b.y;

            return Mathf.Pow(num, 2) + Mathf.Pow(num2, 2) <= Mathf.Pow(dist, 2);
        }
    }
}