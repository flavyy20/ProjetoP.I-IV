using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colisor_Pulo : MonoBehaviour
{
    public float alturaParede = 1.0f;
    public float espessuraParede = 0.1f;

    void Start()
    {
        Bounds limites = GetComponent<Renderer>().bounds;
        CriarColisores(limites);
    }

    void CriarColisores(Bounds limites)
    {
        float yTopo = limites.max.y + (alturaParede / 2f);

        Vector3[] posicoes = new Vector3[4];
        posicoes[0] = new Vector3(limites.min.x - espessuraParede / 2, yTopo, limites.center.z); // esquerda
        posicoes[1] = new Vector3(limites.max.x + espessuraParede / 2, yTopo, limites.center.z); // direita
        posicoes[2] = new Vector3(limites.center.x, yTopo, limites.min.z - espessuraParede / 2); // frente
        posicoes[3] = new Vector3(limites.center.x, yTopo, limites.max.z + espessuraParede / 2); // trás

        Vector3[] tamanhos = new Vector3[4];
        tamanhos[0] = new Vector3(espessuraParede, alturaParede, limites.size.z);
        tamanhos[1] = new Vector3(espessuraParede, alturaParede, limites.size.z);
        tamanhos[2] = new Vector3(limites.size.x, alturaParede, espessuraParede);
        tamanhos[3] = new Vector3(limites.size.x, alturaParede, espessuraParede);

        int layerParede = LayerMask.NameToLayer("Parede");

        for (int i = 0; i < 4; i++)
        {
            GameObject parede = new GameObject("ParedeInvisivel_" + i);
            parede.transform.position = posicoes[i];
            parede.transform.SetParent(transform);

            BoxCollider col = parede.AddComponent<BoxCollider>();
            col.size = tamanhos[i];
            parede.layer = layerParede;
        }
    }
}
