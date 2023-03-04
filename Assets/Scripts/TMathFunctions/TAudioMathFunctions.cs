using UnityEngine;

namespace TMathFunctions
{
    public struct AMathfs
    {
        #region Audio Functions
        public static float LinearToDecibel(float linear)
        {
            return linear == 0 ? -144.0f : 20.0f * Mathf.Log(linear);
        }

        public static float DecibelToLinear(float dB)
        {
            return Mathf.Pow(10.0f, dB / 20.0f);
        }
        #endregion
        
    }

}