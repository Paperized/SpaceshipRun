using UnityEngine;

public class Fuel : DisposableScrollingObject
{
    [Header("Value")]
    [SerializeField]
    private int fuelValue = 0;

    [Header("Rotation")]
    [SerializeField]
    private float rotationSpeed = 10f;

    public int FuelValue => fuelValue;

    void Update()
    {
        if (CanRunUpdate())
            return;

        if (MustDispose())
        {
            GamePool.Instance.DisposeObject(this, typeof(Fuel));
            return;
        }

        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            IngameManager.Instance.GrabFuel(this);
            GamePool.Instance.DisposeObject(this, typeof(Fuel));
        }
    }
}
