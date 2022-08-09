using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement Discovery", menuName = "Game/Achievement/New Discovery Achievement", order = 1)]
public class AchievementDiscovery : Achievement
{
    public DiscoverableEntity EntityToDiscover => entityToDiscover;

    [Header("Discovery", order = 1)]
    [SerializeField]
    private DiscoverableEntity entityToDiscover;

    private void Awake()
    {
        achievementType = AchievementTypeAction.PhaseDiscovered;
    }

    public void NotifyAsDiscovered()
    {
        isUnlocked = true;
    }
}