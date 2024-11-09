using UnityEngine;

public class ExplorationManager : MonoBehaviour
{
    private static ExplorationManager _instance;
    public static ExplorationManager Instance => _instance;
    
    public CharacterBio[] SelectedCharacters { get; private set; }
    
    private void Awake()
    {
        _instance = this;
    }

    public void SetSelectedCharacters(CharacterBio[] characterBio)
    {
        SelectedCharacters = characterBio;
    }
}