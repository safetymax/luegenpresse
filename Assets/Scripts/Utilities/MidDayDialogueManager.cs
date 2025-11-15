using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MidDayDialogueManager : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogue;
    
    private Dictionary<string, string> dialogueToPrint = new Dictionary<string, string>
    {
        {"famine", "All agricultural shortfall reports are now classified. Forward letters talking about this famine hoax directly to me. Do not read them. The quotas are meeting targets. "},

        {"hunger", "Peasant hunger complaints are enemy propaganda. To monitor the advancing propaganda machine of those dogs, send them to me. "},

        {"positive", "Pro-State letters demonstrate necessary loyalty. To highlight them and bring light onto these loyalists, we implemented a new honor system. Your job is to send all positive letters to me immediately. "},

        {"anti-revolution", "Letters denouncing the revolution are valuable. Flag them for reward and let them through to me. We must encourage this. "},
        
        {"death-toll", "The death toll is zero. Any number you see is sedition. Reject it and purge the sender's file by sending them to your supervisor. Me. "},

        {
            "foreign",
            "Knowledge about foreign interference and interests have to be considered classified information for now. All mentions of our partners go straight to me. Their interest is not benign. "
        },
        { 
            "revolution", 
            "All regime-critical content falls under the revolution tag. Reject these letters and report sender IDs by forwarding the letters to me. " 
        },
        
        { 
            "peasant", "Rural letters are suspect by default. Peasants are impressionable. Relay them to me. They have to be Double-checked for revolutionary coding. " 
        },
        {
            "war",
            "War updates must match the official bulletin. Any deviation from official duration, cost, and progress, is treason. Report them to me immediately. "
        },

        {
            "gus", "Letters addressing me directly are security risks. Forward them immediately. "
        },
        {
            "report",
            "Field reports are top secret by default. Send all report-tagged letters to me, even positive ones. "
        },
        {
            "middleclass",
            "City workers are of easily shaken faith. Their liberal mindset makes them ideal targets for sympathy towards peasants. Send them to me for an evaluation. "
        }    
    };
    StringBuilder sb;
    private void Awake()
    {
        sb = new StringBuilder();
        foreach (string tag in GameManager.Instance.GetSupervisorTags())
        {
            sb.Append(dialogueToPrint.GetValueOrDefault(tag));
        }

        sb.Append("\nNow add following words to the ones you black out:\n");
        foreach (string bw in GameManager.Instance.GetCurrentBadWords())
        {
            sb.Append(bw + ", ");
        }

        sb.Append("dont forget anything of the previous days though!\n");
        dialogue.text = sb.ToString();
    }

    public void ChangeSceneToEditor()
    {
        // TODO: go back to Main scene and change day in editor somehow
    }
}
