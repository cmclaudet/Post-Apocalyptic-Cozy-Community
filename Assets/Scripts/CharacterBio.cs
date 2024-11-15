using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/CharacterBio")]
public class CharacterBio : ScriptableObject
{
    public CharacterNames Name;
    public CharacterTrait[] Traits;
    public CharacterSkills[] Skills;
    public Relationship[] Relationships;
    public Sprite ProfilePic;
    public Sprite SelectedPic;
    public Sprite LockedPic;
}

public enum CharacterTrait
{
    Patient,
    Sociable,
    Curious,
    Brave,
    Loyal,
    Honest,
}

public enum CharacterSkills
{
    Lumberjack,
    Cooking,
    Math,
    Planning
}

[Serializable]
public class Relationship
{
    public string CharacterName;
    public RelationshipValue Value;
}

public enum RelationshipValue
{
    Neutral,
    Like,
    Dislike
}

public enum CharacterNames
{
    Abuela,
    Stella,
    Lance
}