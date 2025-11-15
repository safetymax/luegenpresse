using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultLetterButton : MonoBehaviour
{
    public void OnContinuePressed()
    {
        LetterEditor editor = FindObjectOfType<LetterEditor>();
        GameManager.Instance.changeDay();
        editor.CloseActiveLetter();
        SceneManager.LoadScene("MidDayDialogScene");

    }
}
