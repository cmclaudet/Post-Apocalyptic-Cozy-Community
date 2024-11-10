using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiMission : MonoBehaviour
{
    private const int CharactersToSelect = 2;

    public UiCharacterMissionProfile[] CharacterMissionProfiles => new[]
    {
        AbuelaCharacterMissionProfile,
        StellaCharacterMissionProfile,
        LanceCharacterMissionProfile
    };
    public UiCharacterMissionProfile AbuelaCharacterMissionProfile;
    public UiCharacterMissionProfile StellaCharacterMissionProfile;
    public UiCharacterMissionProfile LanceCharacterMissionProfile;
    public TextMeshProUGUI SelectXMoreText;
    public TextMeshProUGUI MissionDescription;
    public Button StartMissionButton;
    
    List<CharacterBio> SelectedCharacters = new List<CharacterBio>();
    
    void Start()
    {
        var characters = GameManager.Instance.CharacterBios;
        foreach (var character in characters)
        {
            if (character.Name == CharacterNames.Abuela)
            {
                InitProfile(AbuelaCharacterMissionProfile, character);
            } else if (character.Name == CharacterNames.Stella)
            {
                InitProfile(StellaCharacterMissionProfile, character);
            } else if (character.Name == CharacterNames.Lance)
            {
                InitProfile(LanceCharacterMissionProfile, character);
            }
        }
        
        SetMissionDescription();
        UpdateSelectXMoreText();
        StartMissionButton.onClick.AddListener(StartMission);
    }

    private void InitProfile(UiCharacterMissionProfile characterMissionProfile, CharacterBio characterBio)
    {
        characterMissionProfile.SetCharacter(characterBio);
        characterMissionProfile.SelectButton.onClick.AddListener(() => OnCharacterSelected(characterBio, characterMissionProfile));
    }

    private void SetMissionDescription()
    {
        var missionText = GameManager.Instance.GetCurrentMission().GetMissionGoal();
        MissionDescription.text = missionText;
    }

    private void OnCharacterSelected(CharacterBio characterBio, UiCharacterMissionProfile characterMissionProfile)
    {
        if (SelectedCharacters.Contains(characterBio))
        {
            SelectedCharacters.Remove(characterBio);
            characterMissionProfile.SelectedImage.gameObject.SetActive(false);
        }
        else
        {
            SelectedCharacters.Add(characterBio);
            characterMissionProfile.SelectedImage.gameObject.SetActive(true);
        }
        
        UpdateSelectXMoreText();
        UpdateCharacterSelection();
    }

    private void UpdateCharacterSelection()
    {
        foreach (var characterUi in CharacterMissionProfiles)
        {
            var isSelected = SelectedCharacters.Contains(characterUi.CharacterBio);
            characterUi.SetSelected(isSelected);

            if (SelectedCharacters.Count == CharactersToSelect)
            {
                characterUi.SetLocked(!isSelected);
            }
            else
            {
                characterUi.SetLocked(false);
            }
        }
    }

    private void UpdateSelectXMoreText()
    {
        var selectXMore = CharactersToSelect - SelectedCharacters.Count;
        var finishedSelection = SelectedCharacters.Count == CharactersToSelect;
        if (finishedSelection)
        {
            SelectXMoreText.text = $"Ready to start mission!";
        }
        else
        {
            SelectXMoreText.text = $"Select {selectXMore} more";
        }
        
        StartMissionButton.interactable = finishedSelection;
    }
    
    private void StartMission()
    {
        // todo replace with making characters move to the forest
        //TODO: TRigger Explorer walk 

        // once characters are done moving do this
        ExplorationManager.Instance.SetSelectedCharacters(SelectedCharacters.ToArray ());

        //TODO Move this when game state changes to forrest reached 
        //var missionDialogue = ExplorationManager.Instance.GetMissionDialogue();
        //ExplorationManager.Instance.StartDialogue(missionDialogue, OnEndMissionDialogue);
    }

    private void OnEndMissionDialogue()
    {
        // todo replace with making characters move back to camp
        
        // once characters are done moving do this

        //TODO: Trigger this when characters reach camp.
        SceneManager.LoadScene("Camp");
    }
}
