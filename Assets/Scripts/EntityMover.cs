using UnityEngine;

public class EntityMover : MonoBehaviour
{
    public float HorizontalSpeed
    {
        get => horizontalSpeed;
        set
        {
            horizontalSpeed = 1f * value;
        }
    }
    private enum MovingSide { Right = -1, Left = 1 }
    private MapManager map;
    private Quaternion maxRotationEuler;
    private Quaternion minRotationEuler;

    [HideInInspector]
    private float horizontalSpeed = 0.5f;
    [HideInInspector]
    private float rotationSpeed = 10f;
    [HideInInspector]
    private float rotationSpeedOnSwapSide = 50f;
    [HideInInspector]
    private float resetRotationSpeed = 80f;
    [HideInInspector]
    private float limitRotation = 40f;

    void Start()
    {
        maxRotationEuler = Quaternion.Euler(0, 0, limitRotation);
        minRotationEuler = Quaternion.Euler(0, 0, -limitRotation);
        map = MapManager.Instance;
    }

    public void Move(Vector2 pointerClick)
    {
        if (pointerClick.x < Screen.width / 2)
        {
            if (transform.rotation.z < maxRotationEuler.z)
            {
                float correctRotSpeed = transform.rotation.GetZEulerAngle() < 0 ? rotationSpeedOnSwapSide : rotationSpeed;
                transform.Rotate(0, 0, GetNextRotationAmount(correctRotSpeed, MovingSide.Left));
            }

            if (transform.rotation.GetZEulerAngle() > 0)
            {
                Vector3 currPosition = transform.position;
                currPosition.x = currPosition.x - horizontalSpeed * Time.deltaTime;

                if (currPosition.x < map.xMinBorder)
                    currPosition.x = map.xMinBorder;

                transform.position = currPosition;
            }
        }
        else if (pointerClick.x >= Screen.width / 2)
        {
            if (transform.rotation.z > minRotationEuler.z)
            {
                float correctRotSpeed = transform.rotation.GetZEulerAngle() > 0 ? rotationSpeedOnSwapSide : rotationSpeed;
                transform.Rotate(0, 0, GetNextRotationAmount(correctRotSpeed, MovingSide.Right));
            }

            if (transform.rotation.GetZEulerAngle() < 0)
            {
                Vector3 currPosition = transform.position;
                currPosition.x = currPosition.x + horizontalSpeed * Time.deltaTime;

                if (currPosition.x > map.xMaxBorder)
                    currPosition.x = map.xMaxBorder;
                transform.position = currPosition;
            }
        }
    }

    public void RotateTowardsDefault()
    {
        if(transform.rotation.z != 0)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, resetRotationSpeed * Time.deltaTime);
    }

    private float GetNextRotationAmount(float rotationSpeed, MovingSide movingTo)
    {
        float rotationAmount = (float)movingTo * rotationSpeed * Time.deltaTime;
        float currentZAngle = transform.rotation.GetZEulerAngle();

        if (movingTo == MovingSide.Left)
        {
            if (currentZAngle + rotationAmount > limitRotation)
                rotationAmount = limitRotation - currentZAngle;
        } else if(movingTo == MovingSide.Right)
        {
            if (currentZAngle + rotationAmount < -limitRotation)
                rotationAmount = -limitRotation - currentZAngle;
        }

        return rotationAmount;
    }
}
