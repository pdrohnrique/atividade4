using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }
    private const string Key = "ChaveCriptografiaUnityAtividade4";

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private string GetPath(int slot) => Path.Combine(Application.persistentDataPath, $"save_slot_{slot}.json");

    public bool HasSave(int slot) => File.Exists(GetPath(slot));

    public void SaveGame(SaveData data, int slotIndex)
    {
        string json = JsonUtility.ToJson(data);
        string encrypted = EncryptDecrypt(json);
        File.WriteAllText(GetPath(slotIndex), encrypted);

        // REGRA DO PROFESSOR: Salvar em slot manual deve replicar no Slot 0
        if (slotIndex != 0)
        {
            File.WriteAllText(GetPath(0), encrypted);
        }
    }

    public SaveData LoadGame(int slotIndex)
    {
        if (!HasSave(slotIndex)) return null;
        
        string encrypted = File.ReadAllText(GetPath(slotIndex));
        string json = EncryptDecrypt(encrypted);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // REGRA DO PROFESSOR: Carregar um slot manual deve sobrescrever o Slot 0 com esses dados
        if (slotIndex != 0)
        {
            SaveGame(data, 0);
        }

        return data;
    }

    private string EncryptDecrypt(string text)
    {
        char[] result = new char[text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            result[i] = (char)(text[i] ^ Key[i % Key.Length]);
        }
        return new string(result);
    }
}