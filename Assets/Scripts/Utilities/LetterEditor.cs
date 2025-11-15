using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;
public class LetterEditor : MonoBehaviour
{
    private enum State
    {
        Idle,Opening,Correcting
    }

    private Letter letter;
    private State state = State.Idle;
    
    [SerializeField] private Camera cam;

    [Header("Letter Prefab References")]
    [SerializeField] private GameObject letterPrefab;
    [SerializeField] private Transform letterParent;
    private GameObject activeLetterObject;// Current spawned prefab instance
    private TextMeshProUGUI text;// TMP inside prefab

    void Start() {
        // TODO: Remove once gamemanager is fully integrated for all states
        this.state = State.Correcting;

        letter = GameManager.Instance.getNextLetter();
        SpawnNewLetterObject(letter);
    }

    void Update()
    {
        switch (state)
        {
            case State.Idle:
            {
                // TODO: Add functionality to open letter
                break;
            }
            case State.Opening:
            {
                // TOOD: this is a intermediate step that just plays the animation?
                break;
            }
            case State.Correcting:
            {
                // We are correcting, we can black out and file the letter

                if(Mouse.current.leftButton.wasPressedThisFrame)
                {
                    Vector2 mousePos = Mouse.current.position.ReadValue();
                    int charIndex = TMP_TextUtilities.FindIntersectingCharacter(text, mousePos, cam, true);
                    switch (FindObjectOfType<mouseCursorManager>().currentCursorIndex) // Different actions based on what cursor we have selected
                    {
                        case -1: {break;} // Normal cursor, cant use here
                        case 0: {break;} // Letter opener, cant use here
                        case 1: // WE ARE USING THE MARKER
                            {
                                if (charIndex != -1)
                                {
                                    // get word index
                                    int wordIndex = TMP_TextUtilities.FindIntersectingWord(text, mousePos, cam);
                                    //Debug.Log(wordIndex);
                                    if (wordIndex != -1 && FindObjectOfType<mouseCursorManager>().currentCursorIndex == 1) 
                                    {
                                        // WE ARE USING THE MARKER
                                        TMP_WordInfo wordInfo = text.textInfo.wordInfo[wordIndex];
                                        string word = wordInfo.GetWord();

                                        //this is clicked on word
                                        Debug.Log("Clicked word: " + word);

                                        if(word.ToLower() != null)
                                        {
                                            //replacing word with "$" characters
                                            //TODO: add "€" for wrong words
                                            Debug.Log("Replacing word");
                                            int startIndex = wordInfo.firstCharacterIndex;
                                            int length = wordInfo.characterCount;
                                            string newText = text.text;
                                            newText = newText.Remove(startIndex+10, length); // frage: +10 wegen dem mspace ding?
                                            newText = newText.Insert(startIndex+10, new string('$', length));
                                            text.text = newText;
                                        }
                                    }
                                }
                                break;
                            }

                        case 2: // WE ARE USING THE STAMP
                            // TODO: Check which stamp we are using, stamp it and SendLetter()
                            break; 
                    }
                }
                break;
            }
        }
    }
    private void SpawnNewLetterObject(Letter newLetter)
    {
        // Destroy old letter gameObject
        if (activeLetterObject != null)
            Destroy(activeLetterObject);

        activeLetterObject = Instantiate(letterPrefab, letterParent);
        text = activeLetterObject.GetComponentInChildren<TextMeshProUGUI>();
        if (text == null)
        {
            Debug.LogError("Prefab missing TextMeshProUGUI component!");
            return;
        }
        text.text = newLetter.getLetterContent();
    }
    void SendLetter(int filingIndex)
    {
        letter.setActualFilingIndex(filingIndex);
        GameManager.Instance.updateScore(letter.Evaluate());

        letter = GameManager.Instance.getNextLetter();

        SpawnNewLetterObject(letter);

        this.state = State.Idle;
    }
}

