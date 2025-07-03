using UnityEngine;
using System.Collections.Generic;

public class CityManager : MonoBehaviour
{
    public List<CityMonitor> cities;
    public enum Level { Verde, Amarelo, Vermelho }

    [Range(0.2f, 1f)]
    public float mainSpeed = 0.01f;
    public float minDiff = 0.003f;

    void Start()
    {
        if (cities.Count == 0) return;

        int mainIndex = Random.Range(0, cities.Count);
        cities[mainIndex].SetSpeed(mainSpeed);
        Debug.Log($"Principal: Cidade {mainIndex} velocidade: {mainSpeed}");

        float[] usedSpeeds = new float[cities.Count];
        usedSpeeds[mainIndex] = mainSpeed;

        for (int i = 0; i < cities.Count; i++)
        {
            if (i == mainIndex) continue;

            float speed = 0.001f;
            int attempts = 0;

            do
            {
                speed = Random.Range(0.001f, mainSpeed - minDiff);
                attempts++;
            } while (TooClose(speed, usedSpeeds, i, minDiff) && attempts < 20);

            cities[i].SetSpeed(speed);
            Debug.Log($"Cidade {i} velocidade: {speed}");
            usedSpeeds[i] = speed;
        }
    }

    bool TooClose(float newSpeed, float[] existingSpeeds, int index, float minDifference)
    {
        for (int i = 0; i < existingSpeeds.Length; i++)
        {
            if (i == index) continue;
            if (Mathf.Abs(newSpeed - existingSpeeds[i]) < minDifference)
                return true;
        }
        return false;
    }
    public float GetProgress(int cityIndex)
    {
        if (cityIndex < 0 || cityIndex >= cities.Count)
        {
            Debug.LogWarning("Índice de cidade inválido.");
            return 0f;
        }
        return cities[cityIndex].Progress;
    }
}
