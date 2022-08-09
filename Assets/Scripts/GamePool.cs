using System;
using System.Collections.Generic;
using UnityEngine;

public class GamePool : MonoBehaviour
{
    public static GamePool Instance;
    private Dictionary<Type, Stack<DisposableScrollingObject>> poolDict;
    private Dictionary<Type, DisposableScrollingObject> prefabPoolDict;
    private EndlessEntityManager entityManager;

    [Header("Game Pool Options")]
    [SerializeField]
    private Vector2 defaultPoolLocation = new Vector2(-10, -10);

    [Header("Coin Options")]
    [SerializeField]
    private int coinPoolSize = 10;
    [SerializeField]
    private Coin coinPrefab;

    [Header("Fuel Options")]
    [SerializeField]
    private int fuelPoolSize = 5;
    [SerializeField]
    private Fuel fuelPrefab;


    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
        poolDict = new Dictionary<Type, Stack<DisposableScrollingObject>>();
        poolDict.Add(typeof(Coin), new Stack<DisposableScrollingObject>(coinPoolSize));
        poolDict.Add(typeof(Fuel), new Stack<DisposableScrollingObject>(fuelPoolSize));
        prefabPoolDict = new Dictionary<Type, DisposableScrollingObject>();
        prefabPoolDict.Add(typeof(Coin), coinPrefab);
        prefabPoolDict.Add(typeof(Fuel), fuelPrefab);
    }

    private void Start()
    {
        CreatePools();
    }

    public void SetEndlessEntityManager(EndlessEntityManager entityManager)
    {
        this.entityManager = entityManager;
    }

    public T GetObject<T>(Type type) where T : DisposableScrollingObject
    {
        Stack<DisposableScrollingObject> pool = poolDict[type];
        if(pool.Count > 0)
        {
            return (T)pool.Pop();
        }

        return (T)Instantiate(prefabPoolDict[type], defaultPoolLocation, Quaternion.identity);
    }

    public void DisposeObject(DisposableScrollingObject obj, Type type)
    {
        obj.gameObject.SetActive(false);
        obj.DisposeObject(true);
        obj.gameObject.transform.position = defaultPoolLocation;

        if (type == typeof(Coin))
            entityManager.OnCoinDisposed((Coin)obj);

        poolDict[type].Push(obj);
    }

    private void CreatePools()
    {
        foreach(KeyValuePair<Type, Stack<DisposableScrollingObject>> pair in poolDict)
        {
            for (int i = 0; i < 10; i++)
            {
                DisposableScrollingObject newObject = Instantiate(prefabPoolDict[pair.Key], defaultPoolLocation, Quaternion.identity);
                pair.Value.Push(newObject);
                newObject.gameObject.SetActive(false);
            }
        }
    }
}
