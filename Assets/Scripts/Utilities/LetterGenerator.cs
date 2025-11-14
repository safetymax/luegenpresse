using UnityEngine;
using System.Collections.Generic;

// this class generates letters and gives a list of them
public class LetterGenerator : MonoBehaviour
{
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
}
