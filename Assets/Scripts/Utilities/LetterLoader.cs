using UnityEngine;

public class LetterLoader : MonoBehaviour
{
    public static LetterLoader Instance { get; private set;}
    
    public static LetterDatabase Database { get; private set; }

    void Awake()
    {
        if(Instance!=null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);


        string path = System.IO.Path.Combine(Application.streamingAssetsPath, "letters.json");
        string json = System.IO.File.ReadAllText(path);
        Database = JsonUtility.FromJson<LetterDatabase>(json);
    }
}
