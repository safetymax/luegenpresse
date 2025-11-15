using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class LetterEditor : MonoBehaviour
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
        //checking for pressing on word
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            int charIndex = TMP_TextUtilities.FindIntersectingCharacter(text, mousePos, cam, true);
            if (charIndex != -1)
            {
                // get word index
                int wordIndex = TMP_TextUtilities.FindIntersectingWord(text, mousePos, cam);
                //Debug.Log(wordIndex);

                if (wordIndex != -1 && FindObjectOfType<mouseCursorManager>().currentCursorIndex == 1) //1 is marker cursor
                {
                    TMP_WordInfo wordInfo = text.textInfo.wordInfo[wordIndex];
                    string word = wordInfo.GetWord();

                    //this is clicked on word
                    Debug.Log("Clicked word: " + word);

                    if(word.ToLower() != null)
                    {
                        //replacing word with "$" characters
                        //TODO add "€" for wrong words
                        Debug.Log("Replacing word");
                        int startIndex = wordInfo.firstCharacterIndex;
                        int length = wordInfo.characterCount;
                        string newText = text.text;
                        newText = newText.Remove(startIndex+10, length);
                        newText = newText.Insert(startIndex+10, new string('$', length));
                        text.text = newText;
                    }
                }
            }
        }
    }
    

    /*public void OnPointerClick(PointerEventData eventData)
    {
        System.Console.WriteLine("Clicked on text");
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
    }*/

    void SendLetter(int filingIndex)
    {
        letter.setActualFilingIndex(filingIndex);
        //evaluate letter
    }
}

