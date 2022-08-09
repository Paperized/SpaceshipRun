using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameManager : MonoBehaviour
{
    public static IngameManager Instance;
    public event Action<bool> OnPlayerDeadOrAlive;
    public event Action<bool> OnGamePaused;
    public event Action<int> OnScoreChanged;
    public event Action<int> OnCoinsChanged;
    public event Action<int> OnFuelGrabbed;
    public event Action<int> OnFuelChanged;
    public event Action OnFuelOver;
    public event Action<float> OnDistanceChanged;
    public event Action OnRunQuitted;

    private bool isPlayerDead = false;
    private bool isPaused = false;
    private int currentScore;
    private int currentCoins;
    private int currentFuel;
    private float currentDistance;

    public bool IsPlayerDead
    {
        get => isPlayerDead;
        set
        {
            if(value != isPlayerDead)
            {
                isPlayerDead = value;
                OnPlayerDeadOrAlive?.Invoke(isPlayerDead);
            }
        }
    }
    public bool IsPaused 
    {
        get => isPaused;
        set
        {
            if (value != isPaused)
            {
                isPaused = value;
                OnGamePaused?.Invoke(isPaused);
            }
        }
    }
    public int CurrentScore
    {
        get => currentScore;
        set
        {
            if (value != currentScore)
            {
                currentScore = value;
                OnScoreChanged?.Invoke(currentScore);
            }
        }
    }
    public int CurrentCoins
    {
        get => currentCoins;
        set
        {
            if (value != currentCoins)
            {
                currentCoins = value;
                OnCoinsChanged?.Invoke(currentCoins);
            }
        }
    }
    public float CurrentDistance
    {
        get => currentDistance;
        set
        {
            if (value != currentDistance)
            {
                currentDistance = value;
                OnDistanceChanged?.Invoke(currentDistance);
            }
        }
    }
    public int CurrentFuel
    {
        get => currentFuel;
        set
        {
            if (value != currentFuel)
            {
                currentFuel = value;

                if (currentFuel == 0)
                    OnFuelOver?.Invoke();

                OnFuelChanged?.Invoke(currentFuel);
            }
        }
    }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif
        Instance = this;
    }

    public void GrabCoin(Coin coin)
    {
        CurrentCoins += coin.CoinValue;
    }

    public void GrabFuel(Fuel fuel)
    {
        CurrentFuel += fuel.FuelValue;
        OnFuelGrabbed?.Invoke(fuel.FuelValue);
    }

    public void DecreaseFuel(int amount)
    {
        CurrentFuel -= amount;
    }

    public void IncreaseScore(int amount)
    {
        CurrentScore += amount;
    }
    public void IncreaseDistance(float amount)
    {
        CurrentDistance += amount;
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        OnRunQuitted?.Invoke();
    }
}

