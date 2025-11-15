using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{

    // OnClick of the clock in button  
    public void startGame()
    {
        GameManager.Instance.init();
        SceneManager.LoadScene("Main");
    }
}
