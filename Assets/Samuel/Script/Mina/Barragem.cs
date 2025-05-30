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
            descricao = "Estabilidade do Talude",
            critico = true // Marque quais problemas s�o cr�ticos
        },
        new Problema(){
            descricao = "N�vel de �gua acima do recomendado",
            critico = true
        },
        new Problema(){
            descricao = "Sensores de press�o com falha",
            critico = false
        }
    };

    public bool TemProblemas()
    {
        return problemas.Exists(p => !p.resolvido);
    }

    // Novo m�todo para verificar problemas cr�ticos
    public bool TemProblemasCriticos()
    {
        return problemas.Exists(p => !p.resolvido && p.critico);
    }
}
