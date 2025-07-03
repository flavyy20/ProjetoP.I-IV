using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Save : MonoBehaviour
{
    private string saveFilePath;
    private const string saveFileName = "saveDataECO.json";

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);
        //Debug.Log(saveFilePath);
    }
    
    public void SalvarJogo(SaveDataECO cena)
    {
        try
        {
            string json = JsonUtility.ToJson(cena, true);
            File.WriteAllText(saveFilePath, json);
        }
        catch(System.Exception e) 
        {
            Debug.LogError(e.Message);
        }
    }

    public SaveDataECO CarregarSave()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(saveFilePath);
                return JsonUtility.FromJson<SaveDataECO>(json);
            }
            catch(System.Exception e) 
            {
                Debug.LogError(e.Message);
            }
        }
        else
        {
            Debug.Log("Save não encontrado");
        }
        return null;
    }

    public bool VerificarSave()
    {
        return File.Exists(saveFilePath);
    }

    public void DeletarSave()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }
    }
}
