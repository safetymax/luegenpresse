using UnityEngine;
using System.Collections.Generic;

// PERSISTENT INSTANCE OF THIS ACROSS SCENES
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}

    private int currentDayIndex;
    private int currentScore;
    private List<Letter> lettersOfTheDay;

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
}
