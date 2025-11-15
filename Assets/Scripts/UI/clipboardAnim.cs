using UnityEngine;

public class clipboardAnim : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private AudioClip dingSound;
    bool hasAnimatedIn = false;
    public void animrein()
    {
        if (hasAnimatedIn) return;
            //get simple animator component
            simpleAnimator animator = GetComponent<simpleAnimator>();
            //play animation from current position to (0,0,-10) in 2 seconds with ease out animation type
            animator.playAnim(transform.position, new Vector3(0, 0, -10), 2f, AnimationType.EaseOut);
            AudioSource.PlayClipAtPoint(dingSound, Camera.main.transform.position);
            hasAnimatedIn = true;
        
    }

    public void animraus()
    {
        if (!hasAnimatedIn) return;
            //get simple animator component
            simpleAnimator animator = GetComponent<simpleAnimator>();
            //play animation from current position to (1500,354,-10) in 2 seconds with ease in animation type
            animator.playAnim(transform.position, new Vector3(0, -328, -10), 2f, AnimationType.EaseIn);
            hasAnimatedIn = false;
        
    }
}
