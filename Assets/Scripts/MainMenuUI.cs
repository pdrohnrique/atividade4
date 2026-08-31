using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void OnContinuePressed()
    {
        // Apenas LÊ o Slot 0 sem gravar nada por cima!
        SaveData data = SaveSystem.Instance.LoadGame(0);
        if (data != null)
        {
            LevelManager.currentDataToLoad = data;
            SceneManager.LoadScene(data.sceneName);
        }
    }

    public void OnNewGamePressed()
    {
        // Passa null para indicar jogo novo. O Slot 0 do Checkpoint fica intocado no disco!
        LevelManager.currentDataToLoad = null;
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
            // Lê o Slot manual (1, 2 ou 3) e passa para a cena. NÃO MEXE no Slot 0!
            SaveData data = SaveSystem.Instance.LoadGame(slotIndex);
            LevelManager.currentDataToLoad = data;
            SceneManager.LoadScene(data.sceneName);
        }
    }

    public void OnExitPressed()
    {
        Application.Quit();
    }
}