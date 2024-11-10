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
    public UiMissionResult UiMissionResult;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        if (GameManager.Instance.pendingMissionResult != null)
        {
            UiMissionResult.gameObject.SetActive(true);
            UiMissionResult.SetUp(GameManager.Instance.pendingMissionResult.Success);
            GameManager.Instance.ClearPendingMissionResult();
        }
        else
        {
            UiMissionResult.gameObject.SetActive(false);
        }
    }

    public void StartDialogue(string dialogueName)
    {
        UiCamp.SetActive(false);
        GameManager.Instance.StartDialogue(dialogueName, DialogueRunner, EndDialogue);
    }

    private void EndDialogue()
    {
        UiCamp.SetActive(true);
    }
}