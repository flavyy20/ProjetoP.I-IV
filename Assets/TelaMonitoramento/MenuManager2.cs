using UnityEngine;
public class MenuManager2 : MonoBehaviour
{
    public GameObject panelFicha1;
    public GameObject panelFicha2;

    public GameObject panelCidade1;
    public GameObject panelCidade2;
    public GameObject panelCidade3;

    public GameObject panelMissoes;

    // Chamado ao clicar no botão "Ficha"
    public void AbrirFicha()
    {
        panelFicha1.SetActive(true);
        panelFicha2.SetActive(false); // garante que só o primeiro aparece
    }

    // Chamado ao clicar no botão "Próximo"
    public void IrParaFicha2()
    {
        panelFicha1.SetActive(false);
        panelFicha2.SetActive(true);
    }
    public void FecharFicha2()
    {
        panelFicha2.SetActive(false);

    }
    public void AbrirCidade1()
    {
        panelCidade1.SetActive(true);
    }
    public void AbrirCidade2()
    {
        panelCidade1.SetActive(false);
        panelCidade2.SetActive(true);
    }
    public void AbrirCidade3()
    {
        panelCidade1.SetActive(false);
        panelCidade2.SetActive(false);
        panelCidade3.SetActive(true);
    }
    public void FecharCidade3()
    {
        panelCidade3.SetActive(false);
    }
    public void ListaMissoes()
    {
        panelMissoes.SetActive(true);
    }
    public void FecharListaMissoes()
    {
        panelMissoes.SetActive(false);
    }
}
