using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LetterEditor : MonoBehaviour, IPointerClickHandler
{
    private enum State
    {
        Idle,Opening,Correcting
    }

    private Letter letter;
    private State state = State.Idle;
    
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Camera cam;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("KLICK");
        Vector3 mousePos = eventData.position;
        int charIndex = TMP_TextUtilities.FindIntersectingCharacter(text, mousePos, cam, true);
        Debug.Log(charIndex);
        if (charIndex != -1)
        {
            // get word index
            int wordIndex = TMP_TextUtilities.FindIntersectingWord(text, mousePos, cam);
            Debug.Log(wordIndex);

            if (wordIndex != -1)
            {
                TMP_WordInfo wordInfo = text.textInfo.wordInfo[wordIndex];
                string word = wordInfo.GetWord();

                Debug.Log("Clicked word: " + word);
            }
        }
    }

    void SendLetter(int filingIndex)
    {
        letter.setActualFilingIndex(filingIndex);
        //evaluate letter
    }
}

