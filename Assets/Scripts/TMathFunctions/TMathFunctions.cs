using UnityEngine;

namespace TMathFunctions
{

    public struct Mathfs
    {
        public const float TAU = 2.0f * Mathf.PI;

        public static float Remap(float min, float max, float new_min, float new_max, float val)
        {
            return new_min + (val - min) * (new_max - new_min) / (max - min);
        }

        #region Easing Functions
        public static float EaseInOut(float time, float power)
        {
            if (time < 0.5f)
                return 0.5f * Mathf.Pow(2 * time, power);
            else
                return 0.5f * (1 - Mathf.Pow(1 - (2 * time - 1), power)) + 0.5f;
        }

        public static float EaseIn(float time, float power)
        {
            return Mathf.Pow(time, power);
        }

        public static float EaseOut(float time, float power)
        {
            return 1 - Mathf.Pow(1 - time, power);
        }
        #endregion
        
    }

}