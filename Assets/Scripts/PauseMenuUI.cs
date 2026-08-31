using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject slotsPanel;
    
    private bool isSaving = false;
    private bool isPaused = false;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        slotsPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        slotsPanel.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void OpenSaveSlots()
    {
        isSaving = true;
        pausePanel.SetActive(false);
        slotsPanel.SetActive(true);
    }

    public void OpenLoadSlots()
    {
        isSaving = false;
        pausePanel.SetActive(false);
        slotsPanel.SetActive(true);
    }

    public void SelectSlot(int slotIndex)
    {
        if (isSaving)
        {
            // Salva APENAS no slot manual escolhido (1, 2 ou 3).
            SaveData data = LevelManager.Instance.GetCurrentStateData();
            SaveSystem.Instance.SaveGame(data, slotIndex);
            Resume();
        }
        else
        {
            if (SaveSystem.Instance.HasSave(slotIndex))
            {
                // Carrega o slot manual escolhido sem alterar o Slot 0!
                SaveData data = SaveSystem.Instance.LoadGame(slotIndex);
                LevelManager.currentDataToLoad = data;
                Time.timeScale = 1f;
                SceneManager.LoadScene(data.sceneName);
            }
        }
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}