using System.Linq;
using TMPro;
using UnityEngine;

public class UiCharacterBio : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Traits;
    public TextMeshProUGUI Skills;
    public TextMeshProUGUI RelationshipNames;
    public TextMeshProUGUI RelationshipValues;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetCharacter(CharacterBio characterBio)
    {
        Name.text = characterBio.Name;
        var traits = "";
        var skills = "";
        foreach (var trait in characterBio.Traits)
        {
            traits += trait + "\n";
        }
        foreach (var skill in characterBio.Skills)
        {
            skills += skill + "\n";
        }

        Traits.text = traits;
        Skills.text = skills;
        
        var characterBios = GameManager.Instance.CharacterBios;
        string relationshipNames = "";
        string relationshipValues = "";
        foreach (var character in characterBios)
        {
            if (character.Name == characterBio.Name)
            {
                continue;
            }
            
            if (characterBio.Relationships.Select(r => r.CharacterName).Contains(character.Name))
            {
                relationshipNames += character.Name + "\n";
                relationshipValues += characterBio.Relationships.First(r => r.CharacterName == character.Name).Value + "\n";
            }
        }
        
        RelationshipNames.text = relationshipNames;
        RelationshipValues.text = relationshipValues;
    }
}
