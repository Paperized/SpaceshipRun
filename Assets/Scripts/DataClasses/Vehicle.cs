using UnityEngine;

[CreateAssetMenu(fileName = "New vehicle", menuName = "Game/New Vehicle", order = 3)]
public class Vehicle : ScriptableObject
{
    public GameObject Prefab => prefab;
    public string Name => name;
    public string Description => description;
    public int Speed => speed;
    public int Fuel => fuel;

    [Header("Prefab")]
    [SerializeField]
    private GameObject prefab;

    [Header("Vehicle")]
    [SerializeField]
    private new string name;
    [SerializeField]
    private string description;

    [Header("Stats")]
    [SerializeField]
    [Range(0, 5)]
    private int speed;
    [SerializeField]
    [Range(0, 5)]
    private int fuel;
}
