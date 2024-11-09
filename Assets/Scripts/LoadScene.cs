using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    bool isLoading = false;
    public void Load(string sceneName)
    {
        if (isLoading)
        {
            return;
        }
        SceneManager.LoadScene(sceneName);
        isLoading = true;
    }
}
