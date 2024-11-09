public enum InputController
{
    Dialogue,
    World
}

public class PlayerInput
{
    private InputController currentController = InputController.World;
    
    public void SetController(InputController controller)
    {
        currentController = controller;
    }
    
    public void OnCharacterMouseDown(CharacterBio characterBio)
    {
        if (currentController == InputController.World)
        {
            GameManager.Instance.UiCharacterBio.SetCharacter(characterBio);
            GameManager.Instance.UiCharacterBio.gameObject.SetActive(true);
        }
    }
}