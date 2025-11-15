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
        int amountBlacked = 0;
        amountBlacked+= Regex.Matches(letter.getLetterContent(), "$").Count; //words that were bad and blacked correctly
        amountBlacked+= Regex.Matches(letter.getLetterContent(), "€").Count; //words that were good but blacked

        //returns either -1 for incorrent or 1 for corrent filing
        int actualFilingIndex = letter.getActualFilingIndex(); // 

        if (letter.getTags().Contains(positiveTags[0]))  // letter should be filed to the press also nothing should have been blacked
        {
            if(letter.getActualFilingIndex() == 0 && amountBlacked==0)
            {
                // correctly filed and nothing blacked
                return 10; 
            } else if(letter.getActualFilingIndex() == 0 && amountBlacked > 0)
            {
                // correctly filed but something was blacked
                return 0;
            }else if(letter.getActualFilingIndex() != 0 && amountBlacked ==0 )
            {
                // wrongly filed but nothing was blacked at least
                return 0;
            }
            else
            {
                //wrongly filed and wrongly blacked
                return -10;
            }
        }
        else
        {
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

            if (isSuperVisorLetter) // we should file correctly to supervisor and also not black anything
            {
                if(letter.getActualFilingIndex() == 2 && amountBlacked==0)
                {
                    // correctly filed and nothing blacked
                    return 10; 
                } else if(letter.getActualFilingIndex() == 2 && amountBlacked > 0)
                {
                    // correctly filed but something was blacked
                    return 0;
                }else if(letter.getActualFilingIndex() != 0 && amountBlacked == 0)
                {
                    // wrongly filed and nothing blacked
                    return 0;
                }
                else
                {
                    // wrongly filed and something was blacked
                    return -10;
                }
            }
            else   // The letter is a simple redirection and we need to check how much was correctly/wrongly blacked
            {
                if(letter.getActualFilingIndex() == 1 && wrongAmountBlacked==0)
                {
                    // correctly filed and nothing was wrongly blacked
                    return 10; 
                } else if (letter.getActualFilingIndex() == 1 && wrongAmountBlacked > 0)
                {
                    //correclty filed but some was wrongly blacked
                    return 0;
                }
                else if (letter.getActualFilingIndex() != 1 && wrongAmountBlacked == 0)
                {
                    //wrongly filed but all blacking was right
                    return 0;
                }
                else
                {
                    // wrongly filed and blacking was wrong
                    return -10;
                }
            }
        }
    }
}
