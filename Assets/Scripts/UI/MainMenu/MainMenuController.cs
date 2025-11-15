using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{

    [SerializeField] private GameObject pointerArm;
    [SerializeField] private GameObject swipingArm;
    [SerializeField] private AudioClip dingSound;
    // OnClick of the clock in button  
    public void startGame()
    {
        //get pointer arm simple animator
        pointerArm.GetComponent<simpleAnimator>().playAnim(pointerArm.transform.position, new Vector3(pointerArm.transform.position.x,-500,-15), 0.5f, AnimationType.EaseInOut);
        //delete worldCursor script from pointer arm
        Destroy(pointerArm.GetComponent<worldCursor>());
        swipingArm.GetComponent<simpleAnimator>().playAnim(new Vector3(500,-20,-15), new Vector3(-700, -20, -15), 2f, AnimationType.EaseInOut);
        //wait for 2 seconds and load main scene
        Invoke("loadMainScene", 2f);
    }
    private void loadMainScene()
    {
        GameManager.Instance.init();
        SceneManager.LoadScene("InstructionScene");
    }

    public void playDingSound()
    {
        AudioSource.PlayClipAtPoint(dingSound, Camera.main.transform.position);
    }
}
