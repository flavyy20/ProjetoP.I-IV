using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PainelDiagnostico : MonoBehaviour
{
    //[Header("UI")]
    //public GameObject painel;
    //public Text textoProblemas;
    //public Transform listaChecklist;
    //public GameObject itemChecklistPrefab;

    //[Header("Config")]
    //public KeyCode teclaFechar = KeyCode.Q;

    //[Header("Puzzles")]
    //public GameObject puzzleDrenagemPrefab; // Adicione esta linha

    //[HideInInspector] public bool painelAtivo = false;
    //private Barragem barragemAtual;

    //void Update()
    //{
    //    if (painelAtivo && Input.GetKeyDown(teclaFechar))
    //    {
    //        FecharPainel();
    //    }
    //}

    //public void AbrirPainel(Barragem barragem)
    //{
    //    barragemAtual = barragem;
    //    painelAtivo = true;
    //    painel.SetActive(true);
    //    Time.timeScale = 0;

    //    foreach (Transform child in listaChecklist)
    //    {
    //        Destroy(child.gameObject);
    //    }

    //    textoProblemas.text = "DIAGN�STICO DA BARRAGEM:\n";

    //    foreach (var problema in barragem.problemas)
    //    {
    //        textoProblemas.text += $"� {problema.descricao}\n";

    //        var item = Instantiate(itemChecklistPrefab, listaChecklist);
    //        var toggle = item.GetComponent<Toggle>();
    //        var text = item.GetComponentInChildren<Text>();

    //        text.text = $"Resolver: {problema.descricao}";
    //        toggle.isOn = problema.resolvido;

    //        toggle.onValueChanged.AddListener((valor) => {
    //            if (valor && !problema.resolvido)
    //            {
    //                toggle.isOn = false;
    //               IniciarPuzzle(problema);
    //            }
    //            else
    //            {
    //                problema.resolvido = valor;
    //                VerificarSolucao();
    //            }
    //        });
    //    }
    //}

    //private void IniciarPuzzle(Barragem.Problema problema)
    //{

    //    FecharPainel();

    //    if (problema.descricao.Contains("N�vel de �gua"))
    //    {
    //        if (puzzleDrenagemPrefab == null)
    //        {
    //            Debug.LogError("puzzleDrenagemPrefab n�o est� atribu�do!");
    //            return;
    //        }

    //        GameObject puzzleObj = Instantiate(puzzleDrenagemPrefab);
    //        PuzzleManager puzzleManager = puzzleObj.GetComponentInChildren<PuzzleManager>();

    //        if (puzzleManager == null)
    //        {
    //            Debug.LogError("PuzzleManager n�o encontrado!");
    //            return;
    //        }

    //        // Usa o CoroutineManager para iniciar a Coroutine
    //        CoroutineM.Instance.StartCoroutine(VerificarPuzzleCompleto(puzzleManager, problema));
    //    }

    //}

    //void VerificarSolucao()
    //{
    //    if (barragemAtual.problemas.All(p => p.resolvido))
    //    {
    //        textoProblemas.text += "\nTODOS OS PROBLEMAS RESOLVIDOS!";
    //    }
    //}

    //private IEnumerator VerificarPuzzleCompleto(PuzzleManager puzzleManager, Barragem.Problema problema)
    //{
    //    // Espera at� o puzzle ser resolvido
    //    while (!puzzleManager.puzzleCompleto)
    //    {
    //        yield return null;
    //    }

    //    // Quando resolvido, marca o problema como resolvido e reabre o painel
    //    problema.resolvido = true;
    //    AbrirPainel(barragemAtual);
    //}

    //public void FecharPainel()
    //{
    //    painelAtivo = false;
    //    painel.SetActive(false);
    //    Time.timeScale = 1;
    //}


    [Header("UI")]
    public GameObject painel;
    public Text textoProblemas;
    public Transform listaChecklist;
    public GameObject itemChecklistPrefab;

    [Header("Config")]
    public KeyCode teclaFechar = KeyCode.Q;

    [Header("Puzzles")]
    public List<GameObject> puzzlesPrefabs; // Lista de prefabs de puzzles (arraste no Inspector)
    public GameObject puzzleAtual; // Refer�ncia ao puzzle instanciado

    [HideInInspector] public bool painelAtivo = false;
    private Barragem barragemAtual;



    public static PainelDiagnostico instance; // Singleton p�blico

    void Awake()
    {
        if (instance == null)
        {
            instance = this; // Define a inst�ncia �nica
        }
        else
        {
            Destroy(gameObject); // Evita duplicatas
        }
    }
    void Update()
    {
        if (painelAtivo && Input.GetKeyDown(teclaFechar))
        {
            FecharPainel();
        }
    }

    public void AbrirPainel(Barragem barragem)
    {
        barragemAtual = barragem;
        painelAtivo = true;
        painel.SetActive(true);
        Time.timeScale = 0;

        // Limpa a lista anterior
        foreach (Transform child in listaChecklist)
        {
            Destroy(child.gameObject);
        }

        textoProblemas.text = "DIAGN�STICO DA BARRAGEM:\n";

        // Cria os itens da checklist
        for (int i = 0; i < barragem.problemas.Count; i++)
        {
            var problema = barragem.problemas[i];
            textoProblemas.text += $"� {problema.descricao}\n";

            var item = Instantiate(itemChecklistPrefab, listaChecklist);
            var toggle = item.GetComponent<Toggle>();
            var text = item.GetComponentInChildren<Text>();

            text.text = $"Resolver: {problema.descricao}";
            toggle.isOn = problema.resolvido;

            int index = i; // Captura o �ndice para o closure
            toggle.onValueChanged.AddListener((valor) => {
                if (valor && !problema.resolvido)
                {
                    toggle.isOn = false;
                    IniciarPuzzle(index); // Passa o �ndice do problema
                }
                else
                {
                    problema.resolvido = valor;
                    VerificarSolucao();
                }
            });
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
        AbrirPainel(barragemAtual); // Reabre o painel
    }
    private void IniciarPuzzle(int indexProblema)
    {
        FecharPainel();

        if (indexProblema < 0 || indexProblema >= puzzlesPrefabs.Count)
        {
            Debug.LogError("�ndice de puzzle inv�lido!");
            return;
        }

        if (puzzleAtual != null) Destroy(puzzleAtual);

        puzzleAtual = Instantiate(puzzlesPrefabs[indexProblema]);
        PuzzlePiezometro puzzlePiezometro = puzzleAtual.GetComponent<PuzzlePiezometro>();

        if (puzzlePiezometro != null)
        {
            // Configura o callback DIRETAMENTE (sem condi��es extras)
            puzzlePiezometro.onPuzzleComplete = () =>
            {
                Debug.Log("Callback do puzzle executado!"); // Novo log
                barragemAtual.problemas[indexProblema].resolvido = true;
                CameraManager.Instance.VoltarAoJogador();
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


        PuzzleEstabilidade puzzleEstabilidade = puzzleAtual.GetComponent<PuzzleEstabilidade>();
        if (puzzleEstabilidade != null)
        {
            puzzleEstabilidade.onPuzzleComplete = () =>
            {
                barragemAtual.problemas[indexProblema].resolvido = true;
                CameraManager.Instance.VoltarAoJogador();
                Destroy(puzzleAtual);
                AbrirPainel(barragemAtual);
            };
        }
    }

    void VerificarSolucao()
    {
        if (barragemAtual.problemas.All(p => p.resolvido))
        {
            textoProblemas.text += "\nTODOS OS PROBLEMAS RESOLVIDOS!";
        }
    }

    public void FecharPainel()
    {
        painelAtivo = false;
        painel.SetActive(false);
        Time.timeScale = 1;
    }
}
