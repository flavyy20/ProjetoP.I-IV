using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public List<GroundSink> grounds;
    public float delayBetweenSinks = 5f;

    void Start()
    {
        StartCoroutine(StartSinking());
    }

    IEnumerator StartSinking()
    {
        yield return new WaitForSeconds(2f);

        List<GroundSink> available = new List<GroundSink>(grounds);

        while (available.Count > 0)
        {
            int i = Random.Range(0, available.Count);
            GroundSink ground = available[i];

            if (!ground.isSinking)
            {
                StartCoroutine(ground.Sink());
            }

            available.RemoveAt(i);
            yield return new WaitForSeconds(delayBetweenSinks);
        }
    }
}
