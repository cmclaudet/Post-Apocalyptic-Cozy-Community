using System;
using UnityEngine;
using Yarn.Unity;

public class CampManager : MonoBehaviour
{
    private static CampManager _instance;
    public static CampManager Instance => _instance;
    
    public UiCharacterBio UiCharacterBio;
    public GameObject UiCamp;
    public DialogueRunner DialogueRunner;

    private void Awake()
    {
        _instance = this;
    }

    public void StartDialogue(string dialogueName)
    {
        UiCamp.SetActive(false);
        GameManager.Instance.PlayerInput.SetController(InputController.Dialogue);
        DialogueRunner.StartDialogue(dialogueName);
        DialogueRunner.onDialogueComplete.AddListener(EndDialogue);
    }

    private void EndDialogue()
    {
        UiCamp.SetActive(true);
        GameManager.Instance.PlayerInput.SetController(InputController.World);
    }
}