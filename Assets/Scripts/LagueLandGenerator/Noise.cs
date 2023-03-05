﻿using UnityEngine;
using System.Collections;

public static class Noise {

	public enum NormalizeMode { Local, Global};
	public static NoiseMap GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, NormalizeMode normalizeMode) {
		float[,] noiseMap = new float[mapWidth,mapHeight];

		System.Random prng = new System.Random (seed);
		Vector2[] octaveOffsets = new Vector2[octaves];

        float amplitude = 1;
        float frequency = 1;
        float noiseHeight = 0;

        float maxPossibleHeight = 0;

		for (int i = 0; i < octaves; i++) {
			float offsetX = prng.Next (-100000, 100000) + offset.x;
			float offsetY = prng.Next (-100000, 100000) - offset.y;
			octaveOffsets [i] = new Vector2 (offsetX, offsetY);

			maxPossibleHeight += amplitude;
			amplitude *= persistance;
		}

		if (scale <= 0) {
			scale = 0.0001f;
		}
		float maxLocalNoiseHeight = float.MinValue;
		float minLocalNoiseHeight = float.MaxValue;

		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;
        

        float randX = halfWidth / scale * frequency + octaveOffsets[0].x * frequency;
        float randY = halfHeight / scale * frequency + octaveOffsets[0].y * frequency;

		bool spawnIsland = Mathf.PerlinNoise(randX, randY) > 0.7;

        if (spawnIsland)
		{
			for (int y = 0; y < mapHeight; y++)
			{
				for (int x = 0; x < mapWidth; x++)
				{

					amplitude = 1;
					frequency = 1;
					noiseHeight = 0;

					for (int i = 0; i < octaves; i++)
					{
						//float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * frequency;
						//float sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * frequency;
						float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x * frequency;
						float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y * frequency;

						float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
						noiseHeight += perlinValue * amplitude;

						amplitude *= persistance;
						frequency *= lacunarity;
					}

					if (noiseHeight > maxLocalNoiseHeight)
					{
						maxLocalNoiseHeight = noiseHeight;
					}
					else if (noiseHeight < minLocalNoiseHeight)
					{
						minLocalNoiseHeight = noiseHeight;
					}
					noiseMap[x, y] = noiseHeight;
				}
			}
		} else
		{
			for (int y = 0; y < mapHeight; y++)
			{
				for (int x = 0; x < mapWidth; x++)
				{
					noiseMap[x, y] = 0;
				}
			}

        }
		

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				if(normalizeMode == NormalizeMode.Local)
					noiseMap [x, y] = Mathf.InverseLerp (minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap [x, y]); // preferred if not endless
				else
				{
					float normalizedHeight = (noiseMap[x,y] + 1 ) / (2f * maxPossibleHeight / 1.7f);
					noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
				}
			}
		}

		return new NoiseMap(noiseMap, spawnIsland);
	}

}

public struct NoiseMap
{
    public float[,] noiseMap;
    public bool spawnsIsland;

    public NoiseMap(float[,] noiseMap, bool spawnsIsland)
    {
        this.noiseMap = noiseMap;
        this.spawnsIsland = spawnsIsland;
    }
}
