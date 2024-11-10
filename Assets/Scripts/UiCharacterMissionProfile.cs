using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiCharacterMissionProfile : MonoBehaviour
{
    public TextMeshProUGUI CharacterName;
    public Image CharacterPic;
    public Image SelectedImage;
    public Image LockedImage;
    public Button InfoButton;
    public Button SelectButton;
    
    public CharacterBio CharacterBio { get; private set; }

    public void SetCharacter(CharacterBio characterBio)
    {
        CharacterBio = characterBio;
        CharacterName.text = characterBio.Name.ToString();
        CharacterPic.sprite = characterBio.ProfilePic;
        SelectedImage.sprite = characterBio.SelectedPic;
        LockedImage.sprite = characterBio.LockedPic;
    }
    
    public void SetLocked(bool locked)
    {
        LockedImage.gameObject.SetActive(locked);
        SelectButton.interactable = !locked;
    }

    public void SetSelected(bool isSelected)
    {
        SelectedImage.gameObject.SetActive(isSelected);
        if (isSelected)
        {
            SetLocked(false);
        }
    }
}
