using UnityEngine;

[RequireComponent(typeof(Collider))]
public class VictoryZone : MonoBehaviour
{
    public string nextSceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.Instance?.TriggerVictory(nextSceneName);
        }
    }
}