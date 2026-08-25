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
        SaveData data = SaveSystem.Instance.LoadGame(0);
        if (data != null) SceneManager.LoadScene(data.sceneName);
    }

    public void OnNewGamePressed()
    {
        SaveSystem.Instance.SaveGame(new SaveData { sceneName = "Fase1", currentCoins = 0, hasCheckpoint = false }, 0);
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