using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    public float ScrollingSpeed => scrollingSpeed;
    public float InitialScrollingSpeed => initialScrollingSpeed;
    private float initialScrollingSpeed;

    private MovingBackground[] movingBackgrounds;
    private int nextBgRepeatIndex = 0;

    private GamePhase nextPhase;
    private float nextDistancePrepareBackground;
    private int currentIndexPhase = -1;

    [HideInInspector]
    public float xMinBorder, xMaxBorder, bgHeight, bgWidth;

    [SerializeField]
    private float playerOffsetStart;

    [Header("Scrolling Option")]
    [SerializeField]
    private float scrollingSpeed = 1f;
    [SerializeField]
    private List<GamePhase> phases;

    [Header("Map Size")]
    [SerializeField]
    private float mapHorizontalSize = 5f;
    [SerializeField]
    private float offsetX = 0.2f;

    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
        initialScrollingSpeed = scrollingSpeed;
    }

    protected void Start()
    {
        IngameManager.Instance.OnPlayerDeadOrAlive += OnPause;
        IngameManager.Instance.OnGamePaused += OnPause;
        IngameManager.Instance.OnFuelOver += OnFuelOver;
        SetupBackgrounds();

        if (nextPhase != null)
            IngameManager.Instance.OnDistanceChanged += OnDistanceChanged;
    }

    private void OnDistanceChanged(float distance)
    {
        if(distance >= nextDistancePrepareBackground)
        {
            SetNextPhase();
        }
    }

    private void OnDestroy()
    {
        IngameManager.Instance.OnPlayerDeadOrAlive -= OnPause;
        IngameManager.Instance.OnGamePaused -= OnPause;
        IngameManager.Instance.OnDistanceChanged -= OnDistanceChanged;
    }

    public void OnBackgroundRepeated()
    {
        SetNextBackgroundSprite();
    }

    private void SetupBackgrounds()
    {
        Transform bgContainer = transform.Find("Backgrounds");
        movingBackgrounds = bgContainer.GetComponentsInChildren<MovingBackground>();

        movingBackgrounds[0].multiplierPosition = movingBackgrounds.Length;
        movingBackgrounds[0].Init();
        movingBackgrounds[0].IsScrolling(true);
        bgHeight = movingBackgrounds[0].bgHeight;
        bgWidth = movingBackgrounds[0].bgWidth;

        IngameManager.Instance.IncreaseDistance(playerOffsetStart);

        SetNextPhase();
        SetNextBackgroundSprite();

        if (HasNextPhase())
        {
            if (phases[currentIndexPhase + 1].StartsAt == 1)
            {
                SetNextPhase();
            }
        }

        movingBackgrounds[1].multiplierPosition = movingBackgrounds.Length;
        movingBackgrounds[1].Init();
        movingBackgrounds[1].IsScrolling(true);
        SetNextBackgroundSprite();

        for(int i = 2; i < movingBackgrounds.Length; i++)
        {
            movingBackgrounds[i].multiplierPosition = movingBackgrounds.Length;
            movingBackgrounds[i].Init();
            movingBackgrounds[i].IsScrolling(true);
        }

        movingBackgrounds[0].BackgroundLeader = true;

        if (mapHorizontalSize == 0)
        {
            xMinBorder = movingBackgrounds[0].xMinBorder + offsetX;
            xMaxBorder = movingBackgrounds[0].xMaxBorder - offsetX;
        } else
        {
            xMinBorder = -mapHorizontalSize / 2 + offsetX;
            xMaxBorder = mapHorizontalSize / 2 - offsetX;
        }
    }

    private void SetNextPhase()
    {
        if(HasNextPhase())
        {
            currentIndexPhase++;

            if(HasNextPhase())
            {
                nextPhase = phases[currentIndexPhase + 1];
                nextDistancePrepareBackground = bgHeight * (nextPhase.StartsAt - 1.5f);
            } else
            {
                nextPhase = null;
                IngameManager.Instance.OnDistanceChanged -= OnDistanceChanged;
            }
        } else
        {
            nextPhase = null;
        }
    }

    private bool HasNextPhase()
    {
        return currentIndexPhase < phases.Count - 1;
    }

    private void UpdateBackgroundIndex()
    {
        if (nextBgRepeatIndex + 1 == movingBackgrounds.Length)
            nextBgRepeatIndex = 0;
        else
            nextBgRepeatIndex++;
    }

    private void SetNextBackgroundSprite()
    {
        GamePhase bgPhase = phases[currentIndexPhase];
        movingBackgrounds[nextBgRepeatIndex].SetSprite(bgPhase.NextSprite());

        UpdateBackgroundIndex();
    }

    public void OnPlayerDead(bool value)
    {
        for (int i = 0; i < movingBackgrounds.Length; i++)
        {
            movingBackgrounds[i].IsScrolling(!value);
        }
    }

    public void OnPause(bool value)
    {
        for (int i = 0; i < movingBackgrounds.Length; i++)
        {
            movingBackgrounds[i].IsScrolling(!value);
        }
    }

    private void OnFuelOver()
    {
        StartCoroutine(StartLosingSpeed());
    }

    private IEnumerator StartLosingSpeed()
    {
        bool hasFoundFuel = false;
        float timeToGameOver = 1f;
        int steps = 10;
        float waitFor = timeToGameOver / steps;
        float perTick = scrollingSpeed / steps;

        yield return null;

        while (steps > 0 && !hasFoundFuel)
        {
            if (IngameManager.Instance.CurrentFuel > 0)
            {
                hasFoundFuel = true;
            }
            else
            {
                scrollingSpeed -= perTick;
                for (int i = 0; i < movingBackgrounds.Length; i++)
                {
                    movingBackgrounds[i].ScrollingSpeed = scrollingSpeed;
                }

                yield return new WaitForSeconds(waitFor);
                steps--;
            }
        }

        if(hasFoundFuel)
        {
            BackToInitialSpeed();
        } else
        {
            IngameManager.Instance.IsPlayerDead = true;
        }
    }

    private IEnumerator BackToInitialSpeed()
    {
        float missingSpeed = initialScrollingSpeed - scrollingSpeed;
        float timeToAccumulate = 1f;
        int steps = 10;
        float waitFor = timeToAccumulate / steps;
        float perTick = missingSpeed / steps;

        yield return null;

        while (steps > 0)
        {
            scrollingSpeed += perTick;
            for (int i = 0; i < movingBackgrounds.Length; i++)
            {
                movingBackgrounds[i].ScrollingSpeed = scrollingSpeed;
            }

            yield return new WaitForSeconds(waitFor);
            steps--;
        }
    }
}
