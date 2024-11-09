using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterBio characterBio;
    
    void OnMouseDown()
    {
        GameManager.Instance.PlayerInput.OnCharacterMouseDown(characterBio);
    }
}
