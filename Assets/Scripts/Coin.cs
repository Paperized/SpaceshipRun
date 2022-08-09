using UnityEngine;

public class Coin : DisposableScrollingObject
{
    [Header("Value")]
    [SerializeField]
    private int coinValue = 0;

    [Header("Rotation")]
    [SerializeField]
    private float rotationSpeed = 10f;

    public int CoinValue => coinValue;

    void Update()
    {
        if (CanRunUpdate())
            return;

        if(MustDispose())
        {
            GamePool.Instance.DisposeObject(this, typeof(Coin));
            return;
        }

        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            IngameManager.Instance.GrabCoin(this);
            GamePool.Instance.DisposeObject(this, typeof(Coin));
        }
    }
}
