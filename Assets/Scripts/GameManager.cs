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
    public PlayerInput PlayerInput;
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

    public Mission GetCurrentMission()
    {
        return Missions[currentMissionIndex];
    }
}