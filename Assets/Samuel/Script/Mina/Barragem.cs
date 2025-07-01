using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barragem : MonoBehaviour
{
    [System.Serializable]
    public class Problema
    {
        public string descricao;
        public bool resolvido;
        public bool critico; // Adicione esta linha
       
    }

    public List<Problema> problemas = new List<Problema>(){
        new Problema(){
            descricao = "Nível de água acima do recomendado",
            critico = true
        },
        new Problema(){
            descricao = "Sensores de pressão com falha",
            critico = false
        },
        new Problema(){
            descricao = "Zonas de Fuga Identificadas",
            critico = false
        }
    };

    public bool TemProblemas()
    {
        return problemas.Exists(p => !p.resolvido);
    }

    // Novo método para verificar problemas críticos
    public bool TemProblemasCriticos()
    {
        return problemas.Exists(p => !p.resolvido && p.critico);
    }
}
