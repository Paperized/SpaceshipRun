using UnityEngine;

public class MovingBackground : ScrollingObject
{
    private ScoreCounterManager scoreCounterManager;
    private MapManager mapParent;
    private SpriteRenderer spriteRenderer;
    private float amountScrolled;
    private Vector2 previousPosition;
    private float minimumAmountScrolledBeforeUpdate;
    private bool backgroundLeader = false;

    [HideInInspector]
    public float xMinBorder, xMaxBorder, bgHeight, bgWidth;
    [HideInInspector]
    public int multiplierPosition = 1;

    public bool BackgroundLeader
    {
        set
        {
            if(value != backgroundLeader)
            {
                backgroundLeader = value;
                if(backgroundLeader)
                {
                    scoreCounterManager = GetComponent<ScoreCounterManager>();
                    minimumAmountScrolledBeforeUpdate = scoreCounterManager.UpdateScoreFrequency;
                }
            }
        }
    }

    protected override void Awake() { }

    public void Init()
    {
        mapParent = MapManager.Instance;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        BoxCollider2D bgCollider = GetComponent<BoxCollider2D>();
        xMinBorder = -bgCollider.size.x / 2;
        xMaxBorder = bgCollider.size.x / 2;
        bgHeight = bgCollider.size.y;
        bgWidth = bgCollider.size.x;

        amountScrolled = 0;
        previousPosition = transform.position;
        scrollingSpeed = MapManager.Instance.ScrollingSpeed;
    }

    void Update()
    {
        if (!isScrolling)
            return;

        if (transform.position.y <= -bgHeight)
        {
            mapParent.OnBackgroundRepeated();
            RepeatBackgroundAndUpdatePreviousPosition();
        }

        if (backgroundLeader)
        {
            amountScrolled += Vector2.Distance(transform.position, previousPosition);
            int times = (int)(amountScrolled / minimumAmountScrolledBeforeUpdate);

            if (times > 0)
            {
                scoreCounterManager.OnUpdateScore(times);

                float amountDistanceUpdatable = times * minimumAmountScrolledBeforeUpdate;
                IngameManager.Instance.IncreaseDistance(amountDistanceUpdatable);
                amountScrolled -= amountDistanceUpdatable;
            }

            previousPosition = transform.position;
        }
    }

    private void RepeatBackgroundAndUpdatePreviousPosition()
    {
        Vector2 bgOffset = new Vector2(0, bgHeight * multiplierPosition);
        float distanceFromPrevious = Vector2.Distance(transform.position, previousPosition);
        transform.position = (Vector2)transform.position + bgOffset;
        previousPosition = new Vector2(0, transform.position.y + distanceFromPrevious);
    }

    public void SetSprite(Sprite sprite)
    {
        if(sprite != spriteRenderer.sprite)
            spriteRenderer.sprite = sprite;
    }
}
