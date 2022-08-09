
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class ScrollingObject : MonoBehaviour
{
    public float ScrollingSpeed { 
        get => scrollingSpeed;
        set {
            scrollingSpeed = value;
            UpdateVelocityScroll();
        }
    }
    protected Rigidbody2D rb;
    protected bool isScrolling = false;
    protected float scrollingSpeed;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        scrollingSpeed = MapManager.Instance.ScrollingSpeed;
    }

    public virtual void IsScrolling(bool value)
    {
        isScrolling = value;
        UpdateVelocityScroll();
    }

    protected void UpdateVelocityScroll()
    {
        if (isScrolling)
            rb.velocity = new Vector2(0, -scrollingSpeed);
        else
            rb.velocity = new Vector2(0, 0);
    }
}