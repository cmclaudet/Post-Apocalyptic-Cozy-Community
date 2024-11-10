using System.Linq;
using UnityEngine;

public class ExplorationManager : MonoBehaviour
{
    private static ExplorationManager _instance;
    public static ExplorationManager Instance => _instance;
    
    public CharacterBio[] SelectedCharacters { get; private set; }

    public bool TestMissionResult;
    
    private void Awake()
    {
        _instance = this;
        if (TestMissionResult)
        {
            GameManager.Instance.SetOpeningDialogueFinished();
        }
    }

    public void SetSelectedCharacters(CharacterBio[] characterBio)
    {
        SelectedCharacters = characterBio;
    }
    
    public string GetMissionDialogue()
    {
        Mission currentMission = GameManager.Instance.GetCurrentMission();
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
        var missionPostfix = (int)currentMission;
        
        return $"{dialogueCharacterPrefix}_Mission{missionPostfix}";
    }
}