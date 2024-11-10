using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class GameManager : MonoBehaviour
{
    public class PendingMissionResult
    {
        public Mission Mission;
        public bool Success;
    }
    
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
    public PlayerInput PlayerInput;
    public Mission[] Missions;
    public PendingMissionResult pendingMissionResult { get; private set; }
    
    public bool HasFinishedOpeningDialogue {get; private set;} = false;
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
        Missions = new[] { Mission.GatherWood, Mission.SetUpShelter };
    }
    
    public void SetOpeningDialogueFinished()
    {
        HasFinishedOpeningDialogue = true;
    }

    public Mission GetCurrentMission()
    {
        if (Missions.Length <= currentMissionIndex)
        {
            return Mission.None;
        }
        return Missions[currentMissionIndex];
    }
    
    public void SetMissionResult(bool success, int mission)
    {
        pendingMissionResult = new PendingMissionResult
        {
            Mission = (Mission) mission,
            Success = success
        };
        currentMissionIndex++;
    }
    
    public void ClearPendingMissionResult()
    {
        pendingMissionResult = null;
    }

    public void LoadDialogueScene(string missionDialogue, Action onEnd, Sprite background = null)
    {
        var sceneName = "DialogueScene";
        SceneManager.LoadScene(sceneName);
        StartCoroutine(WaitDialogueSceneLoad(sceneName, missionDialogue, onEnd, background));

        var dialogueRunner = FindObjectOfType<DialogueRunner>();
        StartDialogue(missionDialogue, dialogueRunner, onEnd);
    }
    
    private IEnumerator WaitDialogueSceneLoad(string sceneName, string missionDialogue, Action onEnd, Sprite background)
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == sceneName);
        var dialogueScene = FindObjectOfType<DialogueScene>();

        if (dialogueScene != null)
        {
            dialogueScene.StartDialogue(missionDialogue, onEnd, background);
        }
        else
        {
            Debug.Log("No GameObject with MyComponent found in the scene.");
        }
    }
    
    public void StartDialogue(string dialogueName, DialogueRunner dialogueRunner, Action endDialogue)
    {
        Instance.PlayerInput.SetController(InputController.Dialogue);
        dialogueRunner.StartDialogue(dialogueName);
        dialogueRunner.onDialogueComplete.AddListener(() =>
        {
            endDialogue.Invoke();
            EndDialogue();
        });
    }

    private void EndDialogue()
    {
        Instance.PlayerInput.SetController(InputController.World);
    }
}