using UnityEngine;

public class backgroundAnimator : MonoBehaviour
{
    public float minSpeed = 0.5f;
    public float maxSpeed = 2f;
    public Sprite[] frames;


    // Update is called once per frame
    void Start()
    {
        //using coroutine pick random frame every random time in range of minSpeed and maxSpeed
        StartCoroutine(AnimateBackground());

    }

    private System.Collections.IEnumerator AnimateBackground()
    {
        float waitTime = Random.Range(minSpeed, maxSpeed);
        yield return new WaitForSeconds(waitTime);
        //pick random frame
        int frameIndex = Random.Range(0, frames.Length);
        GetComponent<SpriteRenderer>().sprite = frames[frameIndex];
        //repeat
        StartCoroutine(AnimateBackground());
    }
}
