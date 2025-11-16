using UnityEngine;

public class introManager : MonoBehaviour
{
    public void Start()
    {
        //wait 2 seconds then load main menu (index 1) using coroutine
        StartCoroutine(LoadMainMenu());
    }

    private System.Collections.IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
