using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiMission : MonoBehaviour
{
    private const int CharactersToSelect = 2;
    public UiCharacterMissionProfile[] CharacterMissionProfiles;
    public TextMeshProUGUI SelectXMoreText;
    public TextMeshProUGUI MissionDescription;
    public Button StartMissionButton;
    
    List<CharacterBio> SelectedCharacters = new List<CharacterBio>();
    
    void Start()
    {
        var characters = GameManager.Instance.CharacterBios;
        for (int i = 0; i < characters.Length; i++)
        {
            InitProfile(CharacterMissionProfiles[i], characters[i]);
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
        Debug.Log("Starting mission with characters");
        ExplorationManager.Instance.SetSelectedCharacters(SelectedCharacters.ToArray ());
        var missionDialogue = ExplorationManager.Instance.GetMissionDialogue();
        GameManager.Instance.LoadDialogueScene(missionDialogue, OnEndMissionDialogue);
    }

    private void OnEndMissionDialogue()
    {
        SceneManager.LoadScene("Camp");
    }
}
