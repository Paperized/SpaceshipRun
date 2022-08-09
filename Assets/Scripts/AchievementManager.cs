using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    private AchievementManager Instance;

    [Header("List Achievements")]
    [SerializeField]
    private List<AchievementDiscovery> achievementDiscoveries;
    [SerializeField]
    private List<AchievementGoalAmount> achievementGoalAmounts;

    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;

        if (achievementDiscoveries == null)
            achievementDiscoveries = new List<AchievementDiscovery>();

        if (achievementGoalAmounts == null)
            achievementGoalAmounts = new List<AchievementGoalAmount>();
    }

    private void Start()
    {
        IngameManager.Instance.OnCoinsChanged += OnCoinsChanged;
        IngameManager.Instance.OnDistanceChanged += OnDistanceChanged;
        IngameManager.Instance.OnScoreChanged += OnScoreChanged;
        IngameManager.Instance.OnRunQuitted += OnRunQuitted;
    }

    private void OnDestroy()
    {
        IngameManager.Instance.OnCoinsChanged -= OnCoinsChanged;
        IngameManager.Instance.OnDistanceChanged -= OnDistanceChanged;
        IngameManager.Instance.OnScoreChanged -= OnScoreChanged;
        IngameManager.Instance.OnRunQuitted -= OnRunQuitted;
    }

    private void OnRunQuitted()
    {
        AchievementGoalAmount achievementGoal;

        for (int i = 0; i < achievementGoalAmounts.Count; i++)
        {
            achievementGoal = achievementGoalAmounts[i];
            if(!achievementGoal.IsUnlocked && achievementGoal.MustCompleteInOneRun)
            {
                achievementGoal.CurrentAmount = 0;
            }
        }
    }

    private void OnCoinsChanged(int coins)
    {
        AchievementGoalAmount achievementGoal;

        for (int i = 0; i < achievementGoalAmounts.Count; i++)
        {
            achievementGoal = achievementGoalAmounts[i];
            if (achievementGoal.IsUnlocked || achievementGoal.AchievementType != AchievementTypeAction.CoinsCollected)
                continue;

            achievementGoal.CurrentAmount = coins;
            if(achievementGoal.IsUnlocked)
            {
                
            }
        }
    }

    private void OnDistanceChanged(float distance)
    {
        AchievementGoalAmount achievementGoal;

        for (int i = 0; i < achievementGoalAmounts.Count; i++)
        {
            achievementGoal = achievementGoalAmounts[i];
            if (achievementGoal.IsUnlocked || achievementGoal.AchievementType != AchievementTypeAction.DistanceReached)
                continue;

            achievementGoal.CurrentAmount = distance;
            if (achievementGoal.IsUnlocked)
            {
                
            }
        }
    }

    private void OnScoreChanged(int score)
    {
        AchievementGoalAmount achievementGoal;

        for (int i = 0; i < achievementGoalAmounts.Count; i++)
        {
            achievementGoal = achievementGoalAmounts[i];
            if (achievementGoal.IsUnlocked || achievementGoal.AchievementType != AchievementTypeAction.ScoreReached)
                continue;

            achievementGoal.CurrentAmount = score;
            if (achievementGoal.IsUnlocked)
            {
                
            }
        }
    }
}
