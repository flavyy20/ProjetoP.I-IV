using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colisor_Pulo : MonoBehaviour
{
    public float alturaParede = 1.0f;
    public float espessuraParede = 0.1f;
    public float anguloMaximoParaCima = 25f;

    void Start()
    {
        CriarParedesNasBordasDoTopo();
    }

    void CriarParedesNasBordasDoTopo()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        if (mesh == null) return;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        Dictionary<Edge, int> arestasContagem = new Dictionary<Edge, int>();
        Vector3[] worldVertices = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
            worldVertices[i] = transform.TransformPoint(vertices[i]);

        for (int i = 0; i < triangles.Length; i += 3)
        {
            int i0 = triangles[i];
            int i1 = triangles[i + 1];
            int i2 = triangles[i + 2];

            Vector3 v0 = worldVertices[i0];
            Vector3 v1 = worldVertices[i1];
            Vector3 v2 = worldVertices[i2];

            Vector3 normal = Vector3.Cross(v1 - v0, v2 - v0).normalized;

            if (Vector3.Angle(normal, Vector3.up) <= anguloMaximoParaCima)
            {
                ContarAresta(arestasContagem, new Edge(i0, i1));
                ContarAresta(arestasContagem, new Edge(i1, i2));
                ContarAresta(arestasContagem, new Edge(i2, i0));
            }
        }

        int idx = 0;
        foreach (var par in arestasContagem)
        {
            if (par.Value == 1) // aresta de borda
            {
                Vector3 a = worldVertices[par.Key.v1];
                Vector3 b = worldVertices[par.Key.v2];

                float comprimento = Vector3.Distance(a, b);
                if (comprimento < 0.01f) continue;

                Vector3 centro = (a + b) / 2f;
                Vector3 altura = Vector3.up * (alturaParede / 2f);
                Vector3 posicao = centro + altura;
                Vector3 direcao = (b - a).normalized;

                GameObject parede = new GameObject("ParedeInvisivel_" + idx++);
                parede.transform.position = posicao;
                parede.transform.rotation = Quaternion.LookRotation(direcao, Vector3.up);
                parede.transform.SetParent(transform);

                BoxCollider col = parede.AddComponent<BoxCollider>();
                col.size = new Vector3(espessuraParede, alturaParede, comprimento);

                parede.layer = LayerMask.NameToLayer("Parede");
            }
        }
    }

    void ContarAresta(Dictionary<Edge, int> mapa, Edge aresta)
    {
        if (mapa.ContainsKey(aresta))
            mapa[aresta]++;
        else
            mapa[aresta] = 1;
    }

    struct Edge
    {
        public int v1;
        public int v2;

        public Edge(int a, int b)
        {
            v1 = Mathf.Min(a, b);
            v2 = Mathf.Max(a, b);
        }

        public override int GetHashCode()
        {
            return v1 * 73856093 ^ v2 * 19349663;
        }

        public override bool Equals(object obj)
        {
            return obj is Edge other && v1 == other.v1 && v2 == other.v2;
        }
    }
}
