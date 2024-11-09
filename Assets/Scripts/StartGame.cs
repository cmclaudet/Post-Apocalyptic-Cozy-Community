using UnityEngine;

public class StartGame : MonoBehaviour
{
    public string StartDialogueName;
    public bool SkipFirstDialogue = false;
    
    private void Start()
    {
        if (SkipFirstDialogue)
        {
            return;
        }
        StartDialogue(StartDialogueName);
    }

    private void StartDialogue(string dialogueName)
    {
        GameManager.Instance.StartDialogue(dialogueName);
    }
}