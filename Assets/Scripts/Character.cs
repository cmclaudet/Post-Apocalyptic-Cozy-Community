using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterBio characterBio;
    
    void OnMouseDown()
    {
        GameManager.Instance.UiCharacterBio.SetCharacter(characterBio);
        GameManager.Instance.UiCharacterBio.gameObject.SetActive(true);
    }
}
