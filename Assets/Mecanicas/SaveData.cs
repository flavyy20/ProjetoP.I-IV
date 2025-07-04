[System.Serializable]
public class SaveData
{
    public Fase[] fases;

    public TempData progressao;
    public SaveData(int cena) 
    { 
        fases = new Fase[cena];
        for (int i = 0; i < fases.Length; i++)
        {
            fases[i] = new Fase();
        }
        progressao = new TempData();
    }
}

[System.Serializable]
public class Fase
{
    public bool _finalizado;
    public float _progressao;
    public bool _faseAtual;
    public Fase()
    {

    }
    public Fase(bool finalizado, float progressao, bool faseAtual)
    {
        _finalizado = finalizado;
        _progressao = progressao;
        _faseAtual = faseAtual;
    }
}

[System.Serializable]
public class TempData
{
    public float _progressao;
    public bool _finalizado;
    public bool _faseAtual;
    public int _numeroCidade;
}
