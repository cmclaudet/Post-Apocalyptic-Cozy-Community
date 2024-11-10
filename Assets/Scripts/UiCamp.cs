using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiCamp : MonoBehaviour
{
    public TextMeshProUGUI GoalText;
    public Button StartMissionButton;
    
    void Start()
    {
        var mission = GameManager.Instance.GetCurrentMission();
        GoalText.text = mission.GetMissionGoal();

        StartMissionButton.gameObject.SetActive(GameManager.Instance.GetCurrentMission() != Mission.None);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
