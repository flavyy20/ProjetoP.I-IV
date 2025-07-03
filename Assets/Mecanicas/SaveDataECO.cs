
[System.Serializable]
public class SaveDataECO
{
    public FaseECO[] fases;
    public TempData progressao;

    public SaveDataECO(int cena)
    {
        fases = new FaseECO[cena];
        for (int i = 0; i < fases.Length; i++)
        {
            fases[i] = new FaseECO();
        }
        progressao = new TempData();
    }
}

[System.Serializable]
public class FaseECO
{
    public bool finalizado;
    public bool cidadePerdida;
    public float progressao; // % de conclusão
    public bool faseAtual;
    public int vitimasSalvas;
    public float tempoConclusao; // em segundos

    public FaseECO()
    {
        finalizado = false;
        cidadePerdida = false;
        progressao = 0f;
        faseAtual = false;
        vitimasSalvas = 0;
        tempoConclusao = 0f;
    }
}

[System.Serializable]
public class TempDataECO
{
    public float progressao;
    public bool finalizado;
    public bool faseAtual;
}