using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("UI")]
    public TMP_Text coinText;
    public GameObject victoryPanel;
    public TMP_Text victoryCoinText;

    [Header("Player")]
    public GameObject playerObj;

    public int currentCoins { get; private set; }
    private List<string> collectedCoinIDs = new List<string>();
    private SaveData currentSave;
    private string pendingNextScene;
    private bool waitingNextSceneInput = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentSave = SaveSystem.Instance.LoadGame(0);
        currentCoins = 0; // Moedas resetam no início da fase

        if (currentSave != null && currentSave.sceneName == SceneManager.GetActiveScene().name)
        {
            if (currentSave.hasCheckpoint)
            {
                playerObj.transform.position = currentSave.playerPosition;
                currentCoins = currentSave.currentCoins;
                collectedCoinIDs = new List<string>(currentSave.collectedCoinIDs);
            }
        }

        UpdateCoinUI();
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    void Update()
    {
        if (waitingNextSceneInput && Input.anyKeyDown)
        {
            SceneManager.LoadScene(pendingNextScene);
        }
    }

    public bool IsCoinCollected(string id) => collectedCoinIDs.Contains(id);

    public void CollectCoin(string id)
    {
        if (!collectedCoinIDs.Contains(id))
        {
            collectedCoinIDs.Add(id);
            currentCoins++;
            UpdateCoinUI();
        }
    }

    public void ActivateCheckpoint(Vector3 pos)
    {
        SaveData data = new SaveData
        {
            sceneName = SceneManager.GetActiveScene().name,
            playerPosition = pos,
            currentCoins = currentCoins,
            collectedCoinIDs = new List<string>(collectedCoinIDs),
            hasCheckpoint = true
        };
        SaveSystem.Instance.SaveGame(data, 0);
    }

    public void TriggerVictory(string nextScene)
    {
        Time.timeScale = 0f;
        pendingNextScene = nextScene;
        waitingNextSceneInput = true;

        int totalCoins = FindObjectsByType<Coin>(FindObjectsSortMode.None).Length + collectedCoinIDs.Count;
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            if (victoryCoinText != null) 
                victoryCoinText.text = $"Moedas: {currentCoins} / {totalCoins}";
        }

        // Reseta o autosave para iniciar a próxima fase do zero
        SaveData data = new SaveData
        {
            sceneName = nextScene,
            playerPosition = Vector3.zero,
            currentCoins = 0,
            collectedCoinIDs = new List<string>(),
            hasCheckpoint = false
        };
        SaveSystem.Instance.SaveGame(data, 0);
    }

    private void UpdateCoinUI()
    {
        if (coinText != null) coinText.text = $"Moedas: {currentCoins}";
    }

    public SaveData GetCurrentStateData()
    {
        return new SaveData
        {
            sceneName = SceneManager.GetActiveScene().name,
            playerPosition = currentSave != null && currentSave.hasCheckpoint ? currentSave.playerPosition : playerObj.transform.position,
            currentCoins = currentSave != null && currentSave.hasCheckpoint ? currentSave.currentCoins : 0,
            collectedCoinIDs = new List<string>(collectedCoinIDs),
            hasCheckpoint = currentSave != null && currentSave.hasCheckpoint
        };
    }
}