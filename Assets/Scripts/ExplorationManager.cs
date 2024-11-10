using System;
using System.Linq;
using UnityEngine;
using Yarn.Unity;

public class ExplorationManager : MonoBehaviour
{
    private static ExplorationManager _instance;
    public static ExplorationManager Instance => _instance;
    
    public CharacterBio[] SelectedCharacters { get; private set; }
    public DialogueRunner DialogueRunner;

    public bool TestMissionResult;
    
    private void Awake()
    {
        _instance = this;
        if (TestMissionResult)
        {
            GameManager.Instance.SetOpeningDialogueFinished();
        }
        DialogueRunner.AddCommandHandler<int>(
            "setWinMission",
            SetWinMission
        );
        DialogueRunner.AddCommandHandler<int>(
            "setLoseMission",
            SetLoseMission
        );
    }
    
    private void SetLoseMission(int mission)
    {
        var completedMissionDialogue = GetCompletedMissionDialogue(success: false);
        if (mission == 1)
        {
            GameManager.Instance.SetPendingDialogue(completedMissionDialogue);
        }
        GameManager.Instance.SetMissionResult(false, mission);
    }

    private void SetWinMission(int mission)
    {
        var completedMissionDialogue = GetCompletedMissionDialogue(success: true);
        if (mission == 1)
        {
            GameManager.Instance.SetPendingDialogue(completedMissionDialogue);
        }
        GameManager.Instance.SetMissionResult(true, mission);
    }

    public void StartDialogue(string dialogueName, Action onEnd)
    {
        GameManager.Instance.StartDialogue(dialogueName, DialogueRunner, onEnd);
    }
    
    public void SetSelectedCharacters(CharacterBio[] characterBio)
    {
        SelectedCharacters = characterBio;
    }
    
    public string GetMissionDialogue()
    {
        var dialogueCharacterPrefix = GetMissionDialogueCharacterPrefix();
        
        Mission currentMission = GameManager.Instance.GetCurrentMission();
        var missionPostfix = (int)currentMission;

        return $"{dialogueCharacterPrefix}_Mission{missionPostfix}";
    }

    private string GetCompletedMissionDialogue(bool success)
    {
        var dialogueCharacterPrefix = GetMissionDialogueCharacterPrefix();
        Mission currentMission = GameManager.Instance.GetCurrentMission();
        var missionPostfix = (int)currentMission;
        var successPostfix = success ? "Success" : "Fail";

        return $"{dialogueCharacterPrefix}_Mission{missionPostfix}{successPostfix}Camp";
    }

    private string GetMissionDialogueCharacterPrefix()
    {
        var abuela = GameManager.Instance.CharacterBios.FirstOrDefault(c => c.Name == CharacterNames.Abuela);
        var lance = GameManager.Instance.CharacterBios.FirstOrDefault(c => c.Name == CharacterNames.Lance);
        var stella = GameManager.Instance.CharacterBios.FirstOrDefault(c => c.Name == CharacterNames.Stella);
        
        var abuelaLanceCombo = "AbuelaLance";
        var stellaLanceCombo = "StellaLance";
        var stellaAbuelaCombo = "StellaAbuela";
        var dialogueCharacterPrefix = "";
        
        if (SelectedCharacters.Contains(abuela) && SelectedCharacters.Contains(lance))
        {
            dialogueCharacterPrefix = abuelaLanceCombo;
        } else if (SelectedCharacters.Contains(stella) && SelectedCharacters.Contains(lance))
        {
            dialogueCharacterPrefix = stellaLanceCombo;
        } else if (SelectedCharacters.Contains(stella) && SelectedCharacters.Contains(abuela))
        {
            dialogueCharacterPrefix = stellaAbuelaCombo;
        }

        return dialogueCharacterPrefix;
    }
}