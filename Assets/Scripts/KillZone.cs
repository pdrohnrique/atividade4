using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Ao recarregar a cena, o LevelManager lê o Slot 0 automaticamente
            // e faz a bolinha renascer no último Checkpoint atingido!
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}