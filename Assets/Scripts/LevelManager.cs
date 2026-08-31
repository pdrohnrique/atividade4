using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    // Variável temporária de memória para saber qual save carregar ao abrir a cena
    public static SaveData currentDataToLoad = null;

    [Header("UI")]
    public TMP_Text coinText;
    public GameObject victoryPanel;
    public TMP_Text victoryCoinText;

    [Header("Player")]
    public GameObject playerObj;

    public int currentCoins { get; private set; }
    private List<string> collectedCoinIDs = new List<string>();
    private SaveData activeData;
    private string pendingNextScene;
    private bool waitingNextSceneInput = false;

    void Awake()
    {
        Instance = this;

        // Pega os dados passados pelo Menu (seja Continuar, Slot 1/2/3 ou null para Novo Jogo)
        activeData = currentDataToLoad;
        currentDataToLoad = null; // Limpa a memória temporária

        currentCoins = 0;

        if (activeData != null && activeData.sceneName == SceneManager.GetActiveScene().name)
        {
            if (activeData.hasCheckpoint)
            {
                collectedCoinIDs = new List<string>(activeData.collectedCoinIDs);
            }
        }
    }

    void Start()
    {
        if (activeData != null && activeData.sceneName == SceneManager.GetActiveScene().name && activeData.hasCheckpoint)
        {
            playerObj.transform.position = activeData.playerPosition;
            currentCoins = activeData.currentCoins;
        }

        UpdateCoinUI();
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    void Update()
    {
        bool anyKeyPressed = Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame;
        bool mouseClicked = Mouse.current != null && (Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame);

        if (waitingNextSceneInput && (anyKeyPressed || mouseClicked))
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

    // AQUI É O ÚNICO LUGAR DO PROJETO QUE GRAVA NO SLOT 0!
    public void ActivateCheckpoint(Vector3 pos)
    {
        SaveData checkpointData = new SaveData
        {
            sceneName = SceneManager.GetActiveScene().name,
            playerPosition = pos,
            currentCoins = currentCoins,
            collectedCoinIDs = new List<string>(collectedCoinIDs),
            hasCheckpoint = true
        };

        SaveSystem.Instance.SaveGame(checkpointData, 0);
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

        currentDataToLoad = null;
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
            playerPosition = playerObj.transform.position,
            currentCoins = currentCoins,
            collectedCoinIDs = new List<string>(collectedCoinIDs),
            hasCheckpoint = true
        };
    }
}