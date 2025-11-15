using UnityEngine;
using System.Collections.Generic;

// this class generates letters and gives a list of them
// TODO: Add functionality for the days, topics, etc.
public class LetterGenerator : MonoBehaviour
{
    public static LetterGenerator Instance { get; private set;}

    public List<Letter> generateLetters(int letterCount)
    {
        List<Letter> letterList = new List<Letter>();

        for (int i = 0; i < letterCount; i++)
        {
            // right now i just choose the ith letter in the database
            LetterData data = LetterLoader.Database.letters[i];
            Letter letter = new Letter(data);
            letterList.Add(letter);
        }

        return letterList;
    }

    private void Awake() {
        if(Instance!=null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
