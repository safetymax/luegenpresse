using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

// PERSISTENT INSTANCE OF THIS ACROSS SCENES
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}

    private int currentDayIndex = -1;
    private HashSet<string> tagSet = new HashSet<string>();
    private int totalScore = 0;
    private int currentScore = 0;
    [SerializeField] private float shiftTimeAmountInSeconds;
    public float timeLeft;
    private List<string> dailyTags = new List<string>();
    private List<string> supervisorDailyTags; // picks new tags every day for letters to be filed to him
    private List<Letter> lettersOfTheDay;
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
    private List<string> positiveTags= new List<string>{"positiv"}; // global list of positive tags

    // Variables to report to player after each day
    private int wordsWronglyBlackedOrMissed=0;
    private int wrongFilingCount = 0;

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
        HashSet<string> tagSetTemp = new HashSet<string>();
        foreach (LetterData letterData in LetterLoader.Database.letters)
        {
            foreach (string tag in letterData.tags)
            {
                tagSetTemp.Add(tag);
            }
        }
        this.tagSet = tagSetTemp;
        changeDay();
    }
    public void changeDay()
    {
        this.currentDayIndex++;
        if(currentDayIndex == 5)
        {
            Application.Quit();
        }
        this.wordsWronglyBlackedOrMissed = 0;
        this.wrongFilingCount = 0;
        this.timeLeft = shiftTimeAmountInSeconds;
        
        // generate tags
        System.Random rnd = new System.Random();
        dailyTags = tagSet.OrderBy(_ => rnd.Next()).Take(4).ToList();
        foreach (string tag in dailyTags)
        {
            Debug.Log("Tag: " + tag + ", ");
        }
        // sv chooses tags
        supervisorDailyTags = dailyTags.OrderBy(_ => rnd.Next()).Take(1).ToList();
        supervisorDailyTags.Add("Gus");
        Debug.Log("Supervisor chose: " + supervisorDailyTags[0]);
        // sv chooses bad words 
        currentBadWordList.AddRange(
            badWordsGlobal.OrderBy(_ => rnd.Next()).Take(3)
        );

        lettersOfTheDay = LetterGenerator.Instance.generateLetters(6, dailyTags);  //letter generation
    }
    public void evaluateDay()
    {
        Debug.Log("EVALUATING");
        // check if we failed or not
        if(this.currentScore < 30)
        {
            // failed cause of bad score
            Application.Quit();
        }
        // add letter to acknowledge scores
        ResultLetter resultLetter = new ResultLetter(wordsWronglyBlackedOrMissed, wrongFilingCount);
        lettersOfTheDay.Add(resultLetter);
        //continue to next day if we acknoledge
        // we do this by calling changeDay(); on the button click on the eval letter!
    }
    private void Update() {
        timeLeft -= Time.deltaTime;
        if(timeLeft < 0)
        {
            Application.Quit();
        }
    }
    public void updateScore(int score)
    {
        this.currentScore+=score;
        this.totalScore+=score;
    }
    public Letter getNextLetter()
    {
        if(lettersOfTheDay.Count == 0)
        {
            evaluateDay();
            Letter ret = lettersOfTheDay[0];
            lettersOfTheDay.RemoveAt(0);
            return ret;
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

        foreach (string badWord in GameManager.Instance.badWordsGlobal)
        {
            badWords += Regex.Matches(letter.getLetterContent(), badWord).Count; // wie oft das schlechte noch drinnen ist
        }

        bool isSuperVisorLetter = false;
        // check if supervisor letter
        foreach(string supervisorTag in supervisorDailyTags)
        {
            if (letter.getTags().Count!=0 && letter.getTags().Contains(supervisorTag))
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
            }else if (letter.getActualFilingIndex() == 2 && amountBlacked != 0 ) 
            {
                wordsWronglyBlackedOrMissed+=wrongAmountBlacked;
                return 0;
            }else if (letter.getActualFilingIndex() != 2 && amountBlacked == 0 )
            {
                wrongFilingCount++;
                return 0;
            }
            else
            {
                wordsWronglyBlackedOrMissed+=wrongAmountBlacked;
                wrongFilingCount++;
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
                }else if (letter.getActualFilingIndex() == 1 && wrongAmountBlacked != 0   ) 
                {
                    wordsWronglyBlackedOrMissed+=wrongAmountBlacked;
                    return 0;
                } else if (letter.getActualFilingIndex() != 1 && wrongAmountBlacked == 0)
                {
                    wrongFilingCount++;
                    return 0;
                }
                else
                {
                    wordsWronglyBlackedOrMissed+=wrongAmountBlacked;
                    wrongFilingCount++;
                    return -10;
                }
            }
            else
            {
                if (letter.getTags().Count!=0  && letter.getTags().Contains("positive"))
                {
                    // we need to file to press with no blacking
                    if(letter.getActualFilingIndex() == 0 && amountBlacked == 0)
                    {
                        return 10;
                    }else if (letter.getActualFilingIndex() == 0 && amountBlacked != 0 ) 
                    {
                        wordsWronglyBlackedOrMissed+=wrongAmountBlacked;
                        return 0;
                    }else if (letter.getActualFilingIndex() != 0 && amountBlacked == 0 )
                    {
                        wrongFilingCount++;
                        return 0;
                    }
                    else
                    {
                        wordsWronglyBlackedOrMissed+=wrongAmountBlacked;
                        wrongFilingCount++;
                        return -10;
                    }
                }
                else
                {
                    // we need to file normally with no blacking
                    if(letter.getActualFilingIndex() == 1 && amountBlacked == 0)
                    {
                        return 10;
                    }else if (letter.getActualFilingIndex() == 1 && amountBlacked != 0  ) 
                    {
                        wordsWronglyBlackedOrMissed+=wrongAmountBlacked;
                        return 0;
                    }else if (letter.getActualFilingIndex() != 1 && amountBlacked == 0)
                    {
                        wrongFilingCount++;
                        return 0;
                    }
                    else
                    {
                        wordsWronglyBlackedOrMissed+=wrongAmountBlacked;
                        wrongFilingCount++;
                        return -10;
                    }
                }
            }
        }
            
    }


    public int getTotalScore()
    {
        return totalScore;
    }
}
