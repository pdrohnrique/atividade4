using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    void Start()
    {
        if (SaveSystem.Instance == null)
        {
            GameObject obj = new GameObject("SaveSystem");
            obj.AddComponent<SaveSystem>();
        }

        SceneManager.LoadScene("MainMenu");
    }
}