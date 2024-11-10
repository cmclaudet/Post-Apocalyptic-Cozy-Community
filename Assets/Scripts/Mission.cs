
public enum Mission
{
    GatherWood = 1,
    SetUpShelter = 2,
    None = 0
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
            case Mission.None:
                return "Enjoy your shelter!";
        }
        
        return string.Empty;
    }
} 