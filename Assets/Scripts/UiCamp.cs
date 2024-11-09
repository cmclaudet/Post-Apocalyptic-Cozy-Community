using TMPro;
using UnityEngine;

public class UiCamp : MonoBehaviour
{
    public TextMeshProUGUI GoalText;
    
    void Start()
    {
        var mission = GameManager.Instance.GetCurrentMission();
        GoalText.text = mission.GetMissionGoal();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
