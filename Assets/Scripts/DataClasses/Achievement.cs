using UnityEngine;

public class Achievement : ScriptableObject
{
    public string Title => title;
    public string Description => description;
    public bool IsUnlocked => isUnlocked;
    public AchievementTypeAction AchievementType => achievementType;

    [Header("Common Achievement Data", order = 0)]
    [SerializeField]
    protected string title;
    [SerializeField]
    protected string description;
    [SerializeField]
    protected bool isUnlocked;
    [SerializeField]
    protected AchievementTypeAction achievementType;
}

public enum AchievementTypeAction
{
    DistanceReached,
    ScoreReached,
    CoinsCollected,
    SpeedReached,
    PhaseDiscovered
}

