using UnityEngine;

public class sceneTransitionIn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get simple animator component
        simpleAnimator animator = GetComponent<simpleAnimator>();
        //play animation from current position to (0,0,-10) in 2 seconds with ease out animation type
        animator.playAnim(transform.position, new Vector3(-1500, 354, -10), 2f, AnimationType.EaseOut);
    }
}
