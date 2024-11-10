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
    public GameObject Shelter;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        Shelter.gameObject.SetActive(GameManager.Instance.IsGameComplete());
        UiMissionResult.gameObject.SetActive(false);
        if (GameManager.Instance.PendingMissionCompleteDialogue != "")
        {
            UiCamp.SetActive(false);
            if (GameManager.Instance.PendingMissionCompleteDialogue == "Mission2Complete")
            {
                GameManager.Instance.StartDialogue(GameManager.Instance.PendingMissionCompleteDialogue, DialogueRunner,
                    CompleteGame);
            }
            else
            {
                GameManager.Instance.StartDialogue(GameManager.Instance.PendingMissionCompleteDialogue, DialogueRunner,
                    ShowMissionResult);
            }
        } else if (GameManager.Instance.pendingMissionResult != null)
        {
            UiCamp.SetActive(false);
            ShowMissionResult();
        }
    }

    private void CompleteGame()
    {
        UiCamp.SetActive(false);
        Shelter.gameObject.SetActive(true);
    }

    private void ShowMissionResult()
    {
        if (GameManager.Instance.pendingMissionResult.Success)
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

    public void TryStartNextMissionDialogue()
    {
        var mission = GameManager.Instance.GetCurrentMission();
        if (mission == Mission.SetUpShelter)
        {
            StartDialogue("Mission2Intro");
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