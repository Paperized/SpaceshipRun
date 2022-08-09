using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Custom Game Phase", menuName = "Game/New Phase", order = 0)]
public class GamePhase : ScriptableObject
{
    public int StartsAt => startsAt;
    public bool HasFinished { get; private set; }
    public string PhaseName => phaseName;
    private int currentBackgroundSprite;

    [Header("Phase Settings")]
    [SerializeField]
    private string phaseName;
    [SerializeField]
    private int startsAt;

    [Header("Backgrounds")]
    [Tooltip("First background required")]
    [SerializeField]
    private Sprite startingBackground;
    [SerializeField]
    [Tooltip("If its the last phase those middle background will be recycled, at least one required")]
    private List<Sprite> middleBackgrounds;
    [SerializeField]
    private Sprite endingBackground;

    private void OnEnable()
    {
        HasFinished = false;
        currentBackgroundSprite = 0;
    }

    public Sprite CheckNextSprite()
    {
        return HasFinished ? CheckNextSpritePhaseFinished() : CheckNextSpritePhaseNotFinished();
    }

    public Sprite NextSprite()
    {
        Sprite next;

        if (HasFinished)
        {
            next = CheckNextSpritePhaseFinished();
            if (currentBackgroundSprite + 1 >= middleBackgrounds.Count)
            {
                currentBackgroundSprite = 0;
            }
            else
            {
                currentBackgroundSprite++;
            }
        } else
        {
            next = CheckNextSpritePhaseNotFinished();
            if(next == null)
            {
                HasFinished = true;
                next = middleBackgrounds[0];

                if (middleBackgrounds.Count == 1)
                    currentBackgroundSprite = 0;
                else
                    currentBackgroundSprite = 1;
            } else
            {
                currentBackgroundSprite++;
            }
        }

        return next;
    }

    private Sprite CheckNextSpritePhaseNotFinished()
    {
        Sprite next = null;

        if (currentBackgroundSprite == 0)
        {
            next = startingBackground;
        }
        else if (currentBackgroundSprite - 1 < middleBackgrounds.Count)
        {
            next = middleBackgrounds[currentBackgroundSprite - 1];
        }
        else if (currentBackgroundSprite == middleBackgrounds.Count + 1 && endingBackground != null)
        {
            next = endingBackground;
        }

        return next;
    }

    private Sprite CheckNextSpritePhaseFinished()
    {
        return middleBackgrounds[currentBackgroundSprite];
    }
}
