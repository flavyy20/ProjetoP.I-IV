using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bote : MonoBehaviour
{
    Helicopter helicopter;

    private void Awake() => helicopter = GetComponentInParent<Helicopter>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("npc"))
        {
            Transform npc = other.transform;
            npc.GetComponent<Rigidbody>().useGravity = false;
            npc.SetParent(transform, true);
            npc.localPosition = new()
            {
                y = helicopter.npcDistanceToGround*2,
            };
        }
    }
}