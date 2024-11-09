using UnityEngine;

public class StartGame : MonoBehaviour
{
    public string StartDialogueName;
    private void Start()
    {
       StartDialogue(StartDialogueName);
    }

    private void StartDialogue(string dialogueName)
    {
        GameManager.Instance.StartDialogue(dialogueName);
    }
}