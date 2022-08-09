using UnityEngine;

public class FuelComponent : MonoBehaviour
{
    public int CurrentFuel
    {
        get => currentFuel;
        set
        {
            currentFuel = value * 10;
            IngameManager.Instance.CurrentFuel = currentFuel;
        }
    }

    private float currentDistance = 0;
    private float checkpointDistance = 0;
    private int currentFuel = 0;

    [Header("Fuel Options")]
    [SerializeField]
    private float decreaseFuelDistance = 2f;
    [SerializeField]
    private int consumePerTick = 1;

    private void Start()
    {
        IngameManager.Instance.OnFuelGrabbed += OnFuelGrabbed;
        IngameManager.Instance.OnDistanceChanged += OnDistanceChanged;
    }

    private void OnDistanceChanged(float obj)
    {
        currentDistance = obj;
    }

    private void OnDestroy()
    {
        IngameManager.Instance.OnFuelGrabbed -= OnFuelGrabbed;
        IngameManager.Instance.OnDistanceChanged -= OnDistanceChanged;
    }

    private void OnFuelGrabbed(int value)
    {
        currentFuel += value;
    }

    private void Update()
    {
        if (IsFuelOver())
            return;

        if (currentDistance - checkpointDistance >= decreaseFuelDistance)
        {
            currentFuel -= consumePerTick;
            IngameManager.Instance.DecreaseFuel(consumePerTick);
            checkpointDistance = currentDistance;
        }
    }

    private bool IsFuelOver()
    {
        return currentFuel <= 0;
    }
}
