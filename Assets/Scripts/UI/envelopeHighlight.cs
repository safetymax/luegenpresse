using UnityEngine;

public class envelopeHighlight : MonoBehaviour
{
    [SerializeField] private Sprite highlightedEnvelope;
    [SerializeField] private Sprite normalEnvelope;

    public void Update()
    {
        //get mousecursor manager
        mouseCursorManager cursorManager = FindObjectOfType<mouseCursorManager>();
        if (cursorManager != null && cursorManager.currentCursorIndex == 0)
        {
            GetComponent<SpriteRenderer>().sprite = highlightedEnvelope;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = normalEnvelope;
        }
    }
}
