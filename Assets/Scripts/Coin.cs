using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Coin : MonoBehaviour
{
    [Tooltip("ID único para cada moeda da fase (Ex: F1_Coin_01)")]
    public string coinID;

    private void Start()
    {
        if (LevelManager.Instance != null && LevelManager.Instance.IsCoinCollected(coinID))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.Instance?.CollectCoin(coinID);
            Destroy(gameObject);
        }
    }
}