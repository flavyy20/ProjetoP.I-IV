using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Save : MonoBehaviour
{
    private string saveFilePath;
    private const string saveFileName = "saveData.json";

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);
        //Debug.Log(saveFilePath);
    }
    
    public void SalvarJogo(SaveData cena)
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

    public SaveData CarregarSave()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(saveFilePath);
                return JsonUtility.FromJson<SaveData>(json);
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
}
