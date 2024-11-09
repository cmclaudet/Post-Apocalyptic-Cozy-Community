
public enum Mission
{
    GatherWood,
    SetUpShelter
}

public static class MissionExtensions
{
    public static string GetMissionGoal(this Mission mission)
    {
        switch (mission)
        {
            case Mission.GatherWood:
                return "Gather wood";
            case Mission.SetUpShelter:
                return "Set up shelter";
        }
        
        return string.Empty;
    }
} 