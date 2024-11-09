using UnityEngine;
using Yarn.Unity;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(GameManager).ToString());
                    _instance = singleton.AddComponent<GameManager>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }

    public CharacterBio[] CharacterBios;
    public UiCharacterBio UiCharacterBio;
    public GameObject UiCamp;
    public PlayerInput PlayerInput;
    public DialogueRunner DialogueRunner;
    public Mission[] Missions;
    private int currentMissionIndex = 0;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        CharacterBios = Resources.LoadAll<CharacterBio>("Characters");
        PlayerInput = new PlayerInput();
    }
    
    public void StartDialogue(string dialogueName)
    {
        UiCamp.SetActive(false);
        PlayerInput.SetController(InputController.Dialogue);
        DialogueRunner.StartDialogue(dialogueName);
        DialogueRunner.onDialogueComplete.AddListener(EndDialogue);
    }

    public Mission GetCurrentMission()
    {
        return Missions[currentMissionIndex];
    }

    private void EndDialogue()
    {
        UiCamp.SetActive(true);
        PlayerInput.SetController(InputController.World);
    }
}