using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteracaoPuzzleDrenagem : MonoBehaviour
{
    public GameObject puzzleDrenagemPrefab;
    private GameObject puzzleAtivo;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && puzzleAtivo == null)
        {
            // Instancia o puzzle
            puzzleAtivo = Instantiate(puzzleDrenagemPrefab, transform.position, transform.rotation);

            // Configura o callback corretamente
            var puzzle = puzzleAtivo.GetComponent<PuzzleDrenagem>();
            if (puzzle != null)
            {
                puzzle.OnPuzzleConcluido = new UnityEngine.Events.UnityEvent();
                puzzle.OnPuzzleConcluido.AddListener(() =>
                {
                    Destroy(puzzleAtivo, 2f);
                });
            }
        }
    }
}