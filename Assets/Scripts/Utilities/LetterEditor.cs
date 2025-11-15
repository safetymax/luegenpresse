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
        Idle,Opening,Correcting,Closing
    }

    private Letter letter;
    private State state = State.Idle;
    
    [SerializeField] private Camera cam;

    [Header("Letter Prefab References")]
    [SerializeField] private GameObject letterPrefab;
    [SerializeField] private Transform letterParent;
    private GameObject activeLetterObject;// Current spawned prefab instance
    private TextMeshProUGUI text;// TMP inside prefab

    private int wc; // fuckass bug fix for taking care of fully blacked out letter
    
    [SerializeField] private GameObject markingPrefab;

    void Start() {
        // TODO: Remove once gamemanager is fully integrated for all states
        this.state = State.Idle;
    }

    void Update()
    {
        switch (state)
        {
            case State.Idle:
            {
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    Vector2 mousePos = Mouse.current.position.ReadValue();
                    if (FindObjectOfType<mouseCursorManager>().currentCursorIndex == 0 && (mousePos - new Vector2(Screen.width/2, 0)).magnitude < Screen.height/10)
                    {
                        letter = GameManager.Instance.getNextLetter();
                        SpawnNewLetterObject(letter);
                        state = State.Opening;
                    }
                }
                break;
            }
            case State.Opening:
            {
                //TODO Play Animation here
                state = State.Correcting;
                break;
            }
            case State.Correcting:
            {
                // We are correcting, we can black out and file the letter

                if(Mouse.current.leftButton.wasPressedThisFrame)
                {
                    Vector2 mousePos = Mouse.current.position.ReadValue();
                    int charIndex = GetNearestCharacterWithBuffer(text, mousePos, cam, 20.0f);
                    switch (FindObjectOfType<mouseCursorManager>().currentCursorIndex) // Different actions based on what cursor we have selected
                    {
                        case -1: {break;} // Normal cursor, cant use here
                        case 0: {break;} // Letter opener, cant use here
                        case 1: // WE ARE USING THE MARKER
                            {
                                if(wc >= 1){
                                    if (charIndex != -1)
                                    {
                                        // get word index
                                        int wordIndex = GetNearestWordWithBuffer(text, mousePos, cam, 25.0f);
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
                                                newText = newText.Remove(startIndex+10, length);
                                                newText = newText.Insert(startIndex+10, new string(
                                                    (GameManager.Instance.badWordsGlobal.Contains(word.ToLower()) ? '$' : '€'), length));
                                                text.text = newText;
                                                wc-=1;
                                            }
                                        }
                                    }
                                }
                                break;
                            }

                        case 2: // WE ARE USING THE STAMP
                            Debug.Log("stamp click");
                            int stampIndex = FindObjectOfType<mouseCursorManager>().stampIndex;
                            Debug.Log(stampIndex);
                            
                            if (Screen.width/3 < mousePos.x
                                && (Screen.width/3)*2 > mousePos.x
                                && Screen.height/10 < mousePos.y
                                && Screen.height/2 > mousePos.y)
                            { 
                                Debug.Log("clicked in letter");
                                //spawn stamp and send
                                GameObject mark = Instantiate<GameObject>(markingPrefab, activeLetterObject.transform);
                                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
                                mouseWorldPos.z = -15f;
                                mark.transform.position = mouseWorldPos;
                                for (int i = 0; i < 3; i++)
                                {
                                    if (i == stampIndex) continue;
                                    mark.transform.Find("Mark"+i).gameObject.SetActive(false);
                                }
                                //get simpleAnimator of letter, play slide out to bottom animation and wait for it to finish
                                activeLetterObject.GetComponent<simpleAnimator>().playAnim(activeLetterObject.transform.position, new Vector3(activeLetterObject.transform.position.x, -500, -10), 1f, AnimationType.EaseInOut);
                                Invoke("SendLetterDelayed", 1f);
                                this.state = State.Closing;
                                //SendLetter(stampIndex);
                            }
                            break;
                    }
                }
                break;
            }
            case State.Closing:
            {
                //play animation for closing
                state = State.Idle;
                break;
            }
        }
    }
    private void SpawnNewLetterObject(Letter newLetter)
    {

        activeLetterObject = Instantiate(letterPrefab, letterParent);
        //animate letter to slide from -500 to 0 y position
        activeLetterObject.GetComponent<simpleAnimator>().playAnim(new Vector3(0, -500, -10), new Vector3(0, 0, -10), 1f, AnimationType.EaseInOut);
        text = activeLetterObject.GetComponentInChildren<TextMeshProUGUI>();
        if (text == null)
        {
            Debug.LogError("Prefab missing TextMeshProUGUI component!");
            return;
        }
        text.text = "<mspace=5>" + newLetter.getLetterContent() + "</mspace>";
        text.ForceMeshUpdate();
        wc = text.textInfo.wordCount;
    }
    void SendLetter(int filingIndex)
    {
        letter.setActualFilingIndex(filingIndex);
        GameManager.Instance.updateScore(letter.Evaluate());

        // Destroy old letter gameObject
        if (activeLetterObject != null)
            Destroy(activeLetterObject);

        this.state = State.Closing;
    }

    // --- HELPER FUNCTIONS ---
    int GetNearestCharacterWithBuffer(TMP_Text text, Vector2 mousePos, Camera cam, float maxDistancePixels)
    {
        int charIndex = TMP_TextUtilities.FindNearestCharacter(text, mousePos, cam, true);
        if (charIndex == -1)
            return -1;

        TMP_CharacterInfo c = text.textInfo.characterInfo[charIndex];

        // Compute real screen position of the character center
        Vector3 charWorldPos = (c.bottomLeft + c.topRight) * 0.5f;
        Vector2 charScreenPos = RectTransformUtility.WorldToScreenPoint(cam, charWorldPos);

        // Distance from mouse → character center
        float dist = Vector2.Distance(mousePos, charScreenPos);

        // Only accept if within threshold
        return (dist <= maxDistancePixels) ? charIndex : -1;
    }
    int GetNearestWordWithBuffer(TMP_Text text, Vector2 mousePos, Camera cam, float maxDistancePixels)
    {
        int wordIndex = TMP_TextUtilities.FindNearestWord(text, mousePos, cam);
        if (wordIndex == -1)
            return -1;

        Debug.Log("WordIndex: " + wordIndex + "");
        TMP_WordInfo wordInfo = text.textInfo.wordInfo[wordIndex];
        Debug.Log("Clicked word checker: " + wordInfo.GetWord());

        TMP_WordInfo word = text.textInfo.wordInfo[wordIndex];

        // Get word center in world space
        TMP_CharacterInfo firstChar = text.textInfo.characterInfo[word.firstCharacterIndex];
        TMP_CharacterInfo lastChar  = text.textInfo.characterInfo[word.lastCharacterIndex];

        Vector3 bl = firstChar.bottomLeft;
        Vector3 tr = lastChar.topRight;
        Vector3 wordCenterWorld = (bl + tr) * 0.5f;
        Vector2 wordCenterScreen = RectTransformUtility.WorldToScreenPoint(cam, wordCenterWorld);

        float dist = Vector2.Distance(mousePos, wordCenterScreen);

        return (dist <= maxDistancePixels) ? wordIndex : -1;
    }

    System.Collections.IEnumerator SendLetterDelayed()
    {
        SendLetter(FindObjectOfType<mouseCursorManager>().stampIndex);
        yield return null;
    }

}

