using UnityEngine;

public class simpleAnimator : MonoBehaviour
{
    public void Start()
    {
        playAnim(new Vector2(0, -500), Vector2.zero, 2f, AnimationType.EaseOut);
    }
    public void playAnim(Vector2 from, Vector2 to, float duration, AnimationType animType)
    {
        //without using DOTween or any other tweening library but using coroutines
        StartCoroutine(Animate(from, to, duration, animType));

    }

    private System.Collections.IEnumerator Animate(Vector2 from, Vector2 to, float duration, AnimationType animType)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            switch(animType)
            {
                case AnimationType.EaseIn:
                    transform.position = Vector2.Lerp(from, to, Mathf.Pow(elapsed / duration, 2));
                    break;
                case AnimationType.EaseOut:
                    transform.position = Vector2.Lerp(from, to, 1 - Mathf.Pow(1 - (elapsed / duration), 2));
                    break;
                case AnimationType.EaseInOut:
                    float t = elapsed / duration;
                    if (t < 0.5f)
                    {
                        transform.position = Vector2.Lerp(from, to, 2 * t * t);
                    }
                    else
                    {
                        transform.position = Vector2.Lerp(from, to, -1 + (4 - 2 * t) * t);
                    }
                    break;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = to;

    }
}


public enum AnimationType
{
    EaseIn,
    EaseOut,
    EaseInOut,
}