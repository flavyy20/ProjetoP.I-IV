using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PainelDiagnostico : MonoBehaviour
{
    [Header("UI")]
    public GameObject painel;
    public Text textoProblemas;
    public Transform listaChecklist;
    public GameObject itemChecklistPrefab;

    [Header("Config")]
    public KeyCode teclaFechar = KeyCode.Q;
    public string cenaInicial = "TelaMonitoramento";

    [Header("Puzzles")]
    public List<GameObject> puzzlesPrefabs;
    public GameObject puzzleAtual;

    [HideInInspector] public bool painelAtivo = false;
    private Barragem barragemAtual;

    [Header("Seta de Navegação")]
    public List<SetaVoadora> setasFuga;  // Arraste as setas aqui no Inspector
    public string descricaoAtivarSetas = "Zonas de Fuga Identificadas"; // Descrição que ativa as setas


    public static PainelDiagnostico instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (painelAtivo && Input.GetKeyDown(teclaFechar))
        {
            FecharPainel();
        }
        VerificarSolucao();
    }

    public void AbrirPainel(Barragem barragem)
    {
        barragemAtual = barragem;
        painelAtivo = true;
        painel.SetActive(true);
        Time.timeScale = 0;

        AtivarSetasFuga(false);

        foreach (Transform child in listaChecklist)
        {
            Destroy(child.gameObject);
        }

        textoProblemas.text = "DIAGNÓSTICO DA BARRAGEM:\n";

        for (int i = 0; i < barragem.problemas.Count; i++)
        {
            var problema = barragem.problemas[i];
            textoProblemas.text += $"• {problema.descricao}\n";

            var item = Instantiate(itemChecklistPrefab, listaChecklist);
            var toggle = item.GetComponent<Toggle>();
            var text = item.GetComponentInChildren<Text>();

            text.text = $"Resolver: {problema.descricao}";
            toggle.isOn = problema.resolvido;

            int index = i;
            toggle.onValueChanged.AddListener((valor) => {

                if (problema.descricao == descricaoAtivarSetas)
                {
                    // Caso especial para Zonas de Fuga - não inicia puzzle
                    problema.resolvido = valor;
                    AtivarSetasFuga(valor);
                    VerificarSolucao();
                }
                else if (valor && !problema.resolvido)
                {
                    toggle.isOn = false;
                    IniciarPuzzle(index);
                }
                else
                {
                    problema.resolvido = valor;
                    VerificarSolucao();
                }
            });
            //    if (valor && !problema.resolvido)
            //    {
            //        toggle.isOn = false;
            //        IniciarPuzzle(index);
            //    }
            //    else
            //    {
            //        problema.resolvido = valor;
            //        VerificarSolucao();

            //        if (problema.descricao == descricaoAtivarSetas)
            //        {
            //            AtivarSetasFuga(valor);
            //        }
            //    }

            //});
        }
    }



    public void AtivarSetasFuga(bool ativar)
    {
        foreach (var seta in setasFuga)
        {
            if (seta != null)
            {
                seta.gameObject.SetActive(ativar);

                // Se ativando, configura os alvos das setas
                if (ativar)
                {
                    // Encontra todos os pontos de fuga selecionados
                    PontoDeFuga[] pontosFuga = FindObjectsOfType<PontoDeFuga>();
                    List<Transform> alvos = new List<Transform>();

                    foreach (var ponto in pontosFuga)
                    {
                        if (ponto.selecionado)
                        {
                            alvos.Add(ponto.transform);
                        }
                    }

                    // Configura os alvos nas setas (assumindo que você tem 2 setas)
                    if (alvos.Count > 0)
                    {
                        if (setasFuga.Count >= 1 && alvos.Count >= 1)
                            setasFuga[0].target = alvos[0];

                        if (setasFuga.Count >= 2 && alvos.Count >= 2)
                            setasFuga[1].target = alvos[1];
                        else if (setasFuga.Count >= 2)
                            setasFuga[1].target = alvos[0]; // Se só houver um ponto, ambas apontam
                    }
                }
            }
        }
    }
    public void RegistrarProblemaResolvido(string nomeProblema)
    {
        foreach (var problema in barragemAtual.problemas)
        {
            if (problema.descricao.Contains(nomeProblema))
            {
                problema.resolvido = true;
                break;
            }
        }

        VerificarSolucao();
        AbrirPainel(barragemAtual); 
    }
    private void IniciarPuzzle(int indexProblema)
    {
        FecharPainel();

        // Verifica se é o problema de Zonas de Fuga (não deve ter puzzle)
        if (barragemAtual.problemas[indexProblema].descricao == descricaoAtivarSetas)
        {
            Debug.Log("Zonas de Fuga não requerem puzzle");
            return;
        }

        // Restante do código original...
        if (indexProblema < 0 || indexProblema >= puzzlesPrefabs.Count)
        {
            Debug.LogError("Índice de puzzle inválido!");
            return;
        }


        //if (indexProblema < 0 || indexProblema >= puzzlesPrefabs.Count)
        //{
        //    Debug.LogError("Índice de puzzle inválido!");
        //    return;
        //}

        if (puzzleAtual != null) Destroy(puzzleAtual);

        puzzleAtual = Instantiate(puzzlesPrefabs[indexProblema]);
        PuzzlePiezometro puzzlePiezometro = puzzleAtual.GetComponent<PuzzlePiezometro>();

        if (puzzlePiezometro != null)
        {
            
            puzzlePiezometro.onPuzzleComplete = () =>
            {
                Debug.Log("Callback do puzzle executado!"); 
                barragemAtual.problemas[indexProblema].resolvido = true;
                Destroy(puzzleAtual);
                AbrirPainel(barragemAtual);
            };
        }
       
            PuzzleManager puzzleManager = puzzleAtual.GetComponentInChildren<PuzzleManager>();
            if (puzzleManager != null)
            {
                puzzleManager.onPuzzleComplete = () => {
                    barragemAtual.problemas[indexProblema].resolvido = true;
                    Destroy(puzzleAtual);
                    AbrirPainel(barragemAtual);
                };
            }


        //PuzzleEstabilidade puzzleEstabilidade = puzzleAtual.GetComponent<PuzzleEstabilidade>();
        //if (puzzleEstabilidade != null)
        //{
        //    puzzleEstabilidade.onPuzzleComplete = () =>
        //    {
        //        barragemAtual.problemas[indexProblema].resolvido = true;
        //        CameraManager.Instance.VoltarAoJogador();
        //        Destroy(puzzleAtual);
        //        AbrirPainel(barragemAtual);
        //    };
        //}
    }

    void VerificarSolucao()
    {
        //if (barragemAtual.problemas.All(p => p.resolvido))
        //{
        //    textoProblemas.text += "\nTODOS OS PROBLEMAS RESOLVIDOS!";
        //    StartCoroutine(CarregarCenaInicial());
        //}

        if(PuzzleManager.puzzleDrenagemCompleto==true && PuzzlePiezometro.puzzleCompleto == true)
        {
            Debug.Log("entrou no verificar");
            textoProblemas.text += "\nTODOS OS PROBLEMAS RESOLVIDOS!";
            StartCoroutine(CarregarCenaInicial());
        }
    }

    IEnumerator CarregarCenaInicial()
    {
        
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(cenaInicial);
    }

    public void FecharPainel()
    {
        painelAtivo = false;
        painel.SetActive(false);
        Time.timeScale = 1;
    }
}
