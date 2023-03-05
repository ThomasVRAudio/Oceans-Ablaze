using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class FalloffGenerator
{
    private static Vector2 falloffPower = new();
    public static float[,] GenerateFalloffMap(int size, Vector2 falloffPowers)
    {
        falloffPower = falloffPowers;
        float[,] map = new float[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = i / (float)size * 2 - 1;
                float y = j / (float)size * 2 - 1;

                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                map[i, j] = Evaluate(value);
            }
        }
        return map;
    }

    static float Evaluate(float value)
    {
       // float a = 1;
      //  float b = 2.2f;

        return Mathf.Pow(value, falloffPower.x) / (Mathf.Pow(value, falloffPower.x) + Mathf.Pow(falloffPower.y - falloffPower.y * value, falloffPower.x));
    }
}
