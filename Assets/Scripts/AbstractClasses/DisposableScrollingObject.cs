
public abstract class DisposableScrollingObject : ScrollingObject
{
    private float yDispose;
    private bool isDisposed = true;

    protected override void Awake()
    {
        base.Awake();
        yDispose = MapManager.Instance.bgHeight;
    }

    protected bool CanRunUpdate()
    {
        return !isScrolling || isDisposed;
    }

    public void DisposeObject(bool isDisposed)
    {
        this.isDisposed = isDisposed;

        if (isDisposed)
        {
            transform.parent = null;
        }
        else
        {
            scrollingSpeed = MapManager.Instance.ScrollingSpeed;
        }

        IsScrolling(!isDisposed);
    }

    public bool MustDispose()
    {
        return transform.position.y < -yDispose / 2;
    }
}