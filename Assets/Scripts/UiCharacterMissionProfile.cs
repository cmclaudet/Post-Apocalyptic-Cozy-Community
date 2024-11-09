using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiCharacterMissionProfile : MonoBehaviour
{
    public TextMeshProUGUI CharacterName;
    public Image CharacterPic;
    public Button InfoButton;
    public Button SelectButton;
    public Image SelectedImage;
    
    public CharacterBio CharacterBio { get; private set; }

    public void SetCharacter(CharacterBio characterBio)
    {
        CharacterBio = characterBio;
        CharacterName.text = characterBio.Name;
        CharacterPic.sprite = characterBio.ProfilePic;
    }
}
