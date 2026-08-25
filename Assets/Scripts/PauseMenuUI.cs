using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject slotsPanel;
    
    private bool isSaving = false;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
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
            SaveData data = LevelManager.Instance.GetCurrentStateData();
            SaveSystem.Instance.SaveGame(data, slotIndex);
            Resume();
        }
        else
        {
            if (SaveSystem.Instance.HasSave(slotIndex))
            {
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