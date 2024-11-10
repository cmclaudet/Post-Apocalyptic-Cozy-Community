using System;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class DialogueScene : MonoBehaviour
{
    public DialogueRunner DialogueRunner;
    public Image Background;

    private void Awake()
    {
        DialogueRunner.AddCommandHandler<int>(
            "setWinMission",
            SetWinMission
        );
        DialogueRunner.AddCommandHandler<int>(
            "setLoseMission",
            SetLoseMission
        );
    }

    private void SetLoseMission(int mission)
    {
        GameManager.Instance.SetMissionResult(false, mission);
    }

    private void SetWinMission(int mission)
    {
        GameManager.Instance.SetMissionResult(true, mission);
    }

    public void StartDialogue(string dialogueName, Action onEnd, Sprite background)
    {
        GameManager.Instance.PlayerInput.SetController(InputController.Dialogue);

        DialogueRunner.StartDialogue(dialogueName);
        DialogueRunner.onDialogueComplete.AddListener(() =>
        {
            GameManager.Instance.PlayerInput.SetController(InputController.World);
            onEnd.Invoke();
        });

        if (background != null) Background.sprite = background;
    }
}
