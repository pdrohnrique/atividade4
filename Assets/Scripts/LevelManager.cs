using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("UI")]
    public TMP_Text coinText;
    public GameObject victoryPanel;
    public TMP_Text victoryCoinText;

    [Header("Player")]
    public GameObject playerObj;

    public int CurrentCoins { get; private set; }
    private List<string> collectedCoinIDs = new List<string>();
    
    // Guarda os dados de como a fase começou ou do último checkpoint tocado
    private SaveData checkpointState; 
    
    private string pendingNextScene;
    private bool waitingNextSceneInput;

    void Awake()
    {
        Time.timeScale = 1f;
        
        Instance = this;

        SaveData data = SaveSystem.Instance.LoadGame(0);
        CurrentCoins = 0;

        if (data != null && data.sceneName == SceneManager.GetActiveScene().name)
        {
            checkpointState = data;
            if (data.hasCheckpoint)
            {
                collectedCoinIDs = new List<string>(data.collectedCoinIDs);
            }
        }
        else
        {
            // Se for fase nova ou novo jogo, cria o estado inicial com base na posição atual na Unity
            checkpointState = new SaveData
            {
                sceneName = SceneManager.GetActiveScene().name,
                playerPosition = playerObj != null ? playerObj.transform.position : Vector3.zero,
                currentCoins = 0,
                collectedCoinIDs = new List<string>(),
                hasCheckpoint = false
            };
        }
    }

    void Start()
    {
        // SÓ altera a posição do player se o save realmente tiver um CHECKPOINT ATIVO.
        // Se for NOVO JOGO (hasCheckpoint == false), o player se mantém na posição original do novo layout.
        if (checkpointState != null && checkpointState.hasCheckpoint)
        {
            if (playerObj != null)
            {
                playerObj.transform.position = checkpointState.playerPosition;
            }
            CurrentCoins = checkpointState.currentCoins;
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
            CurrentCoins++;
            UpdateCoinUI();
        }
    }

    public void ActivateCheckpoint(Vector3 pos)
    {
        // Atualiza o estado gravado com a posição do checkpoint atingido
        checkpointState = new SaveData
        {
            sceneName = SceneManager.GetActiveScene().name,
            playerPosition = pos,
            currentCoins = CurrentCoins,
            collectedCoinIDs = new List<string>(collectedCoinIDs),
            hasCheckpoint = true
        };

        // Autosave no Slot 0 ao tocar no checkpoint
        SaveSystem.Instance.SaveGame(checkpointState, 0);
    }

    public void TriggerVictory(string nextScene)
    {
        Time.timeScale = 0f;
        pendingNextScene = nextScene;
        waitingNextSceneInput = true;

        int totalCoins = FindObjectsByType<Coin>(FindObjectsInactive.Exclude).Length + collectedCoinIDs.Count;
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            if (victoryCoinText != null) 
                victoryCoinText.text = $"Moedas: {CurrentCoins} / {totalCoins}";
        }

        // Save para a próxima fase zerando as moedas e removendo o checkpoint
        SaveData nextLevelData = new SaveData
        {
            sceneName = nextScene,
            playerPosition = Vector3.zero,
            currentCoins = 0,
            collectedCoinIDs = new List<string>(),
            hasCheckpoint = false
        };
        SaveSystem.Instance.SaveGame(nextLevelData, 0);
    }

    public SaveData GetStateToSave()
    {
        return checkpointState;
    }

    private void UpdateCoinUI()
    {
        if (coinText != null) coinText.text = $"Moedas: {CurrentCoins}";
    }
}