using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessEntityManager : MonoBehaviour
{
    private IngameManager stateManager;
    private GamePool gamePool;

    [Header("Where to Attach Entities")]
    [SerializeField]
    private MapManager map;

    [Header("Coins")]
    [SerializeField]
    private float minDistanceBetweenSpawnCoin = 1f;
    [SerializeField]
    private float maxDistanceBetweenSpawnCoin = 3f;
    [SerializeField]
    private List<SpawnPattern> coinSpawnPatterns;

    private List<Coin> coinsUsed;
    private SpawnPattern nextCoinPattern;
    private Transform currentHigherCoinPattern;
    private float nextLowerCoinYPattern;
    private float distanceBetweenPattern;

    [Header("Fuel")]
    [SerializeField]
    private float minDistanceSpawnFuel = 5f;
    [SerializeField]
    private float maxDistanceSpawnFuel = 10f;

    private List<Fuel> fuelsUsed;
    private float currentDistance = 0;
    private float checkpointDistance = 0;
    private float nextDistanceSpawnFuel;

    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        coinsUsed = new List<Coin>();
        fuelsUsed = new List<Fuel>();

        gamePool = GamePool.Instance;
        stateManager = IngameManager.Instance;

        gamePool.SetEndlessEntityManager(this);
        UpdateNextCoinsPatternSpawn();
        nextLowerCoinYPattern = map.bgHeight * 1.5f;
        UpdateNextFuelSpawn();
        SpawnCoinsWithPattern();

        stateManager.OnGamePaused += OnGamePaused;
        stateManager.OnPlayerDeadOrAlive += OnPlayerDead;
        stateManager.OnFuelOver += OnFuelOver;
        stateManager.OnDistanceChanged += OnDistanceChanged;
    }

    private void OnDestroy()
    {
        stateManager.OnGamePaused -= OnGamePaused;
        stateManager.OnPlayerDeadOrAlive -= OnPlayerDead;
        stateManager.OnFuelOver -= OnFuelOver;
        stateManager.OnDistanceChanged -= OnDistanceChanged;
    }

    void Update()
    {
        if (stateManager.IsPlayerDead || stateManager.IsPaused || stateManager.CurrentFuel == 0)
            return;

        ElaborateCoins();
        ElaborateFuels();
    }

    private void OnDistanceChanged(float d)
    {
        currentDistance = d;
    }

    private bool IsCoinPatternSpawnable()
    {
        return currentHigherCoinPattern.position.y <= nextLowerCoinYPattern - distanceBetweenPattern;
    }

    private void ElaborateCoins()
    {
        if (IsCoinPatternSpawnable())
        {
            SpawnCoinsWithPattern();
        }
    }

    private void SpawnCoinsWithPattern()
    {
        if (coinSpawnPatterns != null && coinSpawnPatterns.Count > 0)
        {
            Vector3 bounds = nextCoinPattern.LeftRightTopBound();
            float randomX = Random.Range(map.xMinBorder + System.Math.Abs(bounds.x), map.xMaxBorder - System.Math.Abs(bounds.y));

            for (int i = 0; i < nextCoinPattern.listObjects.Count; i++)
            {
                Vector3 spawnPoint = nextCoinPattern.listObjects[i];
                Coin newCoin = gamePool.GetObject<Coin>(typeof(Coin));
                newCoin.gameObject.SetActive(true);
                newCoin.gameObject.transform.parent = map.transform;
                newCoin.gameObject.transform.position = new Vector3(spawnPoint.x + randomX, spawnPoint.y + map.bgHeight * 1.5f, -1f);
                newCoin.DisposeObject(false);
                coinsUsed.Add(newCoin);

                if (spawnPoint.y == bounds.z)
                {
                    currentHigherCoinPattern = newCoin.transform;
                }
            }

            UpdateNextCoinsPatternSpawn();
        }
    }

    private void UpdateNextCoinsPatternSpawn()
    {
        int rndIndex = Random.Range(0, coinSpawnPatterns.Count);
        nextCoinPattern = coinSpawnPatterns[rndIndex];
        nextLowerCoinYPattern = map.bgHeight * 1.5f + nextCoinPattern.LowerBound();
        distanceBetweenPattern = Random.Range(minDistanceBetweenSpawnCoin, maxDistanceBetweenSpawnCoin);
    }

    private void ElaborateFuels()
    {
        if (currentDistance - checkpointDistance >= nextDistanceSpawnFuel)
        {
            Fuel newFuel = gamePool.GetObject<Fuel>(typeof(Fuel));
            newFuel.gameObject.SetActive(true);
            newFuel.gameObject.transform.parent = map.transform;
            newFuel.gameObject.transform.position = new Vector3(Random.Range(map.xMinBorder, map.xMaxBorder), map.bgHeight, -1f);
            newFuel.DisposeObject(false);
            UpdateNextFuelSpawn();
            fuelsUsed.Add(newFuel);

            checkpointDistance = currentDistance;
        }
    }

    private void UpdateNextFuelSpawn()
    {
        nextDistanceSpawnFuel = Random.Range(minDistanceSpawnFuel, maxDistanceSpawnFuel);
    }

    private void OnFuelOver()
    {
        StartCoroutine(StartLosingSpeed());
    }

    private IEnumerator StartLosingSpeed()
    {
        bool hasFoundFuel = false;
        float timeToGameOver = 1f;
        int steps = 10;
        float waitFor = timeToGameOver / steps;

        yield return null;

        while (steps > 0 && !hasFoundFuel)
        {
            if (IngameManager.Instance.CurrentFuel > 0)
            {
                hasFoundFuel = true;
            }
            else
            {
                for (int i = 0; i < coinsUsed.Count; i++)
                {
                    coinsUsed[i].ScrollingSpeed = map.ScrollingSpeed;
                }

                for (int i = 0; i < fuelsUsed.Count; i++)
                {
                    fuelsUsed[i].ScrollingSpeed = map.ScrollingSpeed;
                }

                yield return new WaitForSeconds(waitFor);
                steps--;
            }
        }

        if (hasFoundFuel)
        {
            BackToInitialSpeed();
        }
    }

    private IEnumerator BackToInitialSpeed()
    {
        float timeToAccumulate = 1f;
        int steps = 10;
        float waitFor = timeToAccumulate / steps;

        yield return null;

        while (steps > 0)
        {
            for (int i = 0; i < coinsUsed.Count; i++)
            {
                coinsUsed[i].ScrollingSpeed = map.ScrollingSpeed;
            }

            for (int i = 0; i < fuelsUsed.Count; i++)
            {
                fuelsUsed[i].ScrollingSpeed = map.ScrollingSpeed;
            }

            yield return new WaitForSeconds(waitFor);
            steps--;
        }
    }

    public void OnCoinDisposed(Coin coin)
    {
        coinsUsed.Remove(coin);
    }

    private void OnGamePaused(bool obj)
    {
        NotifyPauseEntities(obj);
    }

    private void OnPlayerDead(bool obj)
    {
        NotifyPauseEntities(obj);
    }

    private void NotifyPauseEntities(bool isPaused)
    {
        for(int i = 0; i < coinsUsed.Count; i++)
        {
            coinsUsed[i].IsScrolling(!isPaused);
        }

        for (int i = 0; i < fuelsUsed.Count; i++)
        {
            fuelsUsed[i].IsScrolling(!isPaused);
        }
    }
}
