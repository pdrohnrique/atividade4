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
            // Pega os dados de checkpoint e salva. O SaveSystem já replica para o Slot 0 automaticamente.
            SaveData data = LevelManager.Instance.GetStateToSave();
            SaveSystem.Instance.SaveGame(data, slotIndex);
            Resume();
        }
        else
        {
            if (SaveSystem.Instance.HasSave(slotIndex))
            {
                // Lê o slot escolhido. O SaveSystem já sobrescreve o Slot 0 automaticamente.
                SaveData data = SaveSystem.Instance.LoadGame(slotIndex);
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