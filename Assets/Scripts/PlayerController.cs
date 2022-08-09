using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EntityMover))]
[RequireComponent(typeof(FuelComponent))]
public class PlayerController : MonoBehaviour
{
    public bool editorMode = false;

    [SerializeField]
    private Vehicle currentVehicle;

    private bool hasClickedThisFrame = false;
    private Vector2 lastClickPosition;

    private PointerEventData eventDataOnUI;
    private List<RaycastResult> raycastResultsOnUI;

    private EntityMover entityMover;
    private FuelComponent fuelComponent;

    void Start()
    {
        eventDataOnUI = new PointerEventData(EventSystem.current);
        raycastResultsOnUI = new List<RaycastResult>();

        entityMover = GetComponent<EntityMover>();
        fuelComponent = GetComponent<FuelComponent>();

        if (!editorMode)
        {
            Instantiate(currentVehicle.Prefab, transform);
        }

        entityMover.HorizontalSpeed = currentVehicle.Speed;
        fuelComponent.CurrentFuel = currentVehicle.Fuel;
    }

    private void Update()
    {
        if (IngameManager.Instance.IsPaused || IngameManager.Instance.IsPlayerDead)
            return;

        CheckForClick();

        if (IsPointerOverUIElement())
            return;

        if (hasClickedThisFrame)
            entityMover.Move(lastClickPosition);
        else
            entityMover.RotateTowardsDefault();
    }

    private void CheckForClick()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        hasClickedThisFrame = Input.GetMouseButton(0);
        if (hasClickedThisFrame)
        {
            lastClickPosition = Input.mousePosition;
        }
#elif UNITY_ANDROID || UNITY_IOS
        hasClickedThisFrame = Input.touchCount > 0;
        if (hasClickedThisFrame)
        {
            lastClickPosition = Input.GetTouch(0).position;
        }
#endif
    }

    public bool IsPointerOverUIElement()
    {
        bool result = false;
        SetEventSystemRaycastResults();

        int index = 0;
        while (index < raycastResultsOnUI.Count && !result)
        {
            RaycastResult curRaysastResult = raycastResultsOnUI[index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("InputBlockingObject"))
            {
                result = true;
            }

            index++;
        }

        raycastResultsOnUI.Clear();
        eventDataOnUI.Reset();
        return result;
    }

    private void SetEventSystemRaycastResults()
    {
        eventDataOnUI.position = lastClickPosition;
        EventSystem.current.RaycastAll(eventDataOnUI, raycastResultsOnUI);
    }
}
