using UnityEngine;

public class UiMissionResult : MonoBehaviour
{
    public GameObject SucceedResult;
    public GameObject FailResult;
    
    public void SetUp(bool result)
    {
        SucceedResult.SetActive(result);
        FailResult.SetActive(!result);
    }
}
