using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

// PERSISTENT INSTANCE OF THIS ACROSS SCENES
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}

    private int currentDayIndex;
    private int currentScore;
    private List<Letter> lettersOfTheDay;
    private List<string> positiveTags= new List<string>{"positiv"}; // global list of positive tags
    private List<string> supervisorTagsOfTheDay; // picks new tags every day for letters to be filed to him
    private List<string> currentBadWordList = new List<string>(); // contiously expanded with new bad words to black out (if we arent filing to supervisor ofc)
    public List<string> badWordsGlobal = new List<string>
    {
        "Ehc",
        "starvation",
        "famine",
        "revolution",
        "peasant",
        "bombing",
        "bombs",
        "bombings",
        "foreign",
        "quota",
        "11",
        "casino",
        "weather",
        "airstrike",
        "shortfalls",
        "mineral"
    };

    private void Awake() {
        if(Instance!=null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Called when the main menu "clock in" button is clicked
    public void init()
    {
        currentDayIndex = 1;
        currentScore = 0;
        lettersOfTheDay = LetterGenerator.Instance.generateLetters(2);
        Debug.Log("Letters 0: " + lettersOfTheDay[0].toString());
        Debug.Log("Letters 1: " + lettersOfTheDay[1].toString());

        supervisorTagsOfTheDay = new List<string>{"Gus"}; // gus is always a supervisor tag
    }
    public void updateScore(int score)
    {
        this.currentScore+=score;
    }
    public Letter getNextLetter()
    {
        if(lettersOfTheDay.Count == 0)
        {
            // we are done, switch day
            return null;
        }
        else
        {
            Letter ret = lettersOfTheDay[0];
            lettersOfTheDay.RemoveAt(0);
            return ret;
        }
    }

    public bool isABadWord(string word)
    {
        if (string.IsNullOrEmpty(word)) return false; 

        word = word.ToLower();

        foreach (string badWord in badWordsGlobal)
        {
            if (!string.IsNullOrEmpty(badWord) && word.Contains(badWord.ToLower()))
            {
                return true;
            }
        }

        return false; 
    }

    // returns score between [-10,10] depending on criteria
    public int evaluateFilingOfLetter(Letter letter, int wrongAmountBlacked)
    {
        int badWords = 0;
        int amountBlacked = 0;
        amountBlacked+= Regex.Matches(letter.getLetterContent(), "$").Count; //words that were bad and blacked correctly
        badWords += Regex.Matches(letter.getLetterContent(), "$").Count;
        amountBlacked+= Regex.Matches(letter.getLetterContent(), "€").Count; //words that were good but blacked

        foreach (String badWord in GameManager.Instance.badWordsGlobal)
        {
            badWords += Regex.Matches(letter.getLetterContent, badWord).Count; // wie oft das schlechte noch drinnen ist
        }

        bool isSuperVisorLetter = false;
        // check if supervisor letter
        foreach(string supervisorTag in supervisorTagsOfTheDay)
        {
            if (letter.getTags().Contains(supervisorTag))
            {
                isSuperVisorLetter = true;
                break;
            }
        }

        if (isSuperVisorLetter) 
        {
            // We need to file to supervisor without blacking
            if(letter.getActualFilingIndex() == 2 && amountBlacked == 0)
            {
                return 10;
            }else if ((letter.getActualFilingIndex() == 2 && amountBlacked != 0 ) || (letter.getActualFilingIndex() != 2 && amountBlacked == 0 )) 
            {
                return 0;
            }
            else
            {
                return -10;
            }
        }
        else
        {
            if(badWords > 0) 
            {
                // We need to file normally and apply blacking to all bad words
                if(letter.getActualFilingIndex() == 1 && wrongAmountBlacked == 0)
                {
                    return 10;
                }else if ((letter.getActualFilingIndex() == 1 && wrongAmountBlacked != 0 ) || (letter.getActualFilingIndex() != 1 && wrongAmountBlacked == 0 )) 
                {
                    return 0;
                }
                else
                {
                    return -10;
                }
            }
            else
            {
                if (letter.getTags().Contains("positive"))
                {
                    // we need to file to press with no blacking
                    if(letter.getActualFilingIndex() == 0 && amountBlacked == 0)
                    {
                        return 10;
                    }else if ((letter.getActualFilingIndex() == 0 && amountBlacked != 0 ) || (letter.getActualFilingIndex() != 0 && amountBlacked == 0 )) 
                    {
                        return 0;
                    }
                    else
                    {
                        return -10;
                    }
                }
                else
                {
                    // we need to file normally with no blacking
                    if(letter.getActualFilingIndex() == 1 && amountBlacked == 0)
                    {
                        return 10;
                    }else if ((letter.getActualFilingIndex() == 1 && amountBlacked != 0 ) || (letter.getActualFilingIndex() != 1 && amountBlacked == 0 )) 
                    {
                        return 0;
                    }
                    else
                    {
                        return -10;
                    }
                }
            }
        }
            
    }
}
