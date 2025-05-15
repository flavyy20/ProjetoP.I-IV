using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject painel1;
    public GameObject painel2;
    public GameObject painel3;

    public void AbrirPainel1() => painel1.SetActive(true);
    public void FecharPainel1() => painel1.SetActive(false);

    public void AbrirPainel2() => painel2.SetActive(true);
    public void FecharPainel2() => painel2.SetActive(false);

    public void AbrirPainel3() => painel3.SetActive(true);
    public void FecharPainel3() => painel3.SetActive(false);



}



