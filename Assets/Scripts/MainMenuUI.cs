using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenuUI : MonoBehaviour
{
    public GameObject continueButton;
    public GameObject slotsPanel;

    void Start()
    {
        if (continueButton != null)
        {
            continueButton.SetActive(SaveSystem.Instance.HasSave(0));
        }
        if (slotsPanel != null) slotsPanel.SetActive(false);
    }
    
    void Update()
    {
        // Atalho para apagar todos os saves ao pressionar a tecla DELETE no editor/jogo
        if (Keyboard.current != null && Keyboard.current.deleteKey.wasPressedThisFrame)
        {
            DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath);
            foreach (FileInfo file in di.GetFiles("*.json")) file.Delete();
            Debug.Log("Saves apagados com sucesso!");

            if (continueButton != null)
            {
                continueButton.SetActive(false);
            }
        }
    }

    public void OnContinuePressed()
    {
        SaveData data = SaveSystem.Instance.LoadGame(0);
        if (data != null) SceneManager.LoadScene(data.sceneName);
    }

    public void OnNewGamePressed()
    {
        // Deleta o save do Slot 0 antigo para não carregar posições de layouts passados
        string autosavePath = Path.Combine(Application.persistentDataPath, "save_slot_0.json");
        if (File.Exists(autosavePath))
        {
            File.Delete(autosavePath);
        }

        // Cria o save zerado para a Fase 1 usando a posição inicial padrão da cena
        SaveData newData = new SaveData 
        { 
            sceneName = "Fase1", 
            playerPosition = Vector3.zero,
            currentCoins = 0, 
            collectedCoinIDs = new System.Collections.Generic.List<string>(),
            hasCheckpoint = false 
        };
        
        SaveSystem.Instance.SaveGame(newData, 0);
        SceneManager.LoadScene("Fase1");
    }

    public void OnLoadGamePressed()
    {
        if (slotsPanel != null) slotsPanel.SetActive(true);
    }

    public void SelectSlotToLoad(int slotIndex)
    {
        if (SaveSystem.Instance.HasSave(slotIndex))
        {
            SaveData data = SaveSystem.Instance.LoadGame(slotIndex);
            SceneManager.LoadScene(data.sceneName);
        }
    }

    public void OnExitPressed()
    {
        Application.Quit();
    }
}