using UnityEngine;

public class StartGame : MonoBehaviour
{
    public string StartDialogueName;
    public bool SkipFirstDialogue = false;
    
    private void Start()
    {
        if (SkipFirstDialogue || GameManager.Instance.HasFinishedOpeningDialogue)
        {
            return;
        }
        StartDialogue(StartDialogueName);
    }

    private void StartDialogue(string dialogueName)
    {
        CampManager.Instance.StartDialogue(dialogueName);
        GameManager.Instance.SetOpeningDialogueFinished();
    }
}