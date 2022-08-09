using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement Amount", menuName = "Game/Achievement/New Amount Achievement", order = 0)]
public class AchievementGoalAmount : Achievement
{
    public bool DisplayProgressMade => displayProgressMade;
    public float AmountNeeded => amountNeeded;
    public bool MustCompleteInOneRun => mustCompleteInOneRun;

    public float CurrentAmount {
        get => currentAmount;
        set
        {
            currentAmount = value;
            if(currentAmount >= amountNeeded)
            {
                currentAmount = amountNeeded;
                isUnlocked = true;
            }
        }
    }

    [Header("Goal Amount", order = 1)]
    [SerializeField]
    private float amountNeeded = 5;
    [SerializeField]
    private float currentAmount = 0;
    [SerializeField]
    private bool displayProgressMade = false;
    [SerializeField]
    private bool mustCompleteInOneRun = false;
}