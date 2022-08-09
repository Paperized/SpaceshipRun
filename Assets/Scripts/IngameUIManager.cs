using TMPro;
using UnityEngine;

public class IngameUIManager : MonoBehaviour
{
    public static IngameUIManager Instance { get; private set; }
    private Vector3 defaultPositionMenu;
    private Vector3 defaultPositionGameOver;
    private bool isMenuOpen = false;
    private bool isMenuAnimating = false;

    [Header("Stat Elements")]
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text coinsText;
    [SerializeField]
    private TMP_Text fuelText;

    [Header("Menu Settings")]
    [SerializeField]
    private RectTransform menuButton;
    [SerializeField]
    private float menuAnimationTime;
    [SerializeField]
    private RectTransform menuModal;
    [SerializeField]
    private RectTransform gameOverModal;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;

        IngameManager.Instance.OnScoreChanged += SetScoreDistance;
        IngameManager.Instance.OnCoinsChanged += SetCoins;
        IngameManager.Instance.OnFuelChanged += SetFuel;
        IngameManager.Instance.OnPlayerDeadOrAlive += OnGameOver;
    }

    private void Start()
    {
        defaultPositionMenu = menuModal.localPosition;
        defaultPositionGameOver = gameOverModal.localPosition;
        menuModal.localPosition = menuButton.localPosition;
        menuModal.localScale = new Vector3(0, 0, 0);
        gameOverModal.localPosition = menuButton.localPosition;
        gameOverModal.localScale = new Vector3(0, 0, 0);
    }

    private void OnDestroy()
    {
        IngameManager.Instance.OnScoreChanged -= SetScoreDistance;
        IngameManager.Instance.OnCoinsChanged -= SetCoins;
        IngameManager.Instance.OnFuelChanged -= SetFuel;
        IngameManager.Instance.OnPlayerDeadOrAlive -= OnGameOver;
    }

    public void SetScoreDistance(int distance)
    {
        if(scoreText != null)
        {
            scoreText.text = distance.ToString();
        }
    }

    public void SetCoins(int coins)
    {
        if (coinsText != null)
        {
            coinsText.text = coins.ToString();
        }
    }

    private void SetFuel(int fuel)
    {
        if(fuelText != null)
        {
            fuelText.text = fuel.ToString();
        }
    }

    public void ReloadGame()
    {
        IngameManager.Instance.ReloadGame();
    }

    private void OnGameOver(bool obj)
    {
        if (obj)
        {
            gameOverModal.gameObject.SetActive(true);
            gameOverModal.LeanScale(new Vector2(1, 1), menuAnimationTime);
            gameOverModal.LeanMove(defaultPositionGameOver, menuAnimationTime);
            isMenuOpen = true;
        }
    }

    public void OpenOrCloseMenu()
    {
        if (isMenuAnimating)
            return;

        if(isMenuOpen)
        {
            menuModal.LeanScale(new Vector2(0, 0), menuAnimationTime);
            menuModal.LeanMove(menuButton.localPosition, menuAnimationTime).setOnComplete(() =>
            {
                menuModal.gameObject.SetActive(false);
                isMenuOpen = false;
                isMenuAnimating = false;
                IngameManager.Instance.IsPaused = false;
            });
        } else
        {
            IngameManager.Instance.IsPaused = true;
            menuModal.gameObject.SetActive(true);
            menuModal.LeanScale(new Vector2(1, 1), menuAnimationTime);
            menuModal.LeanMove(defaultPositionMenu, menuAnimationTime).setOnComplete(() =>
            {
                isMenuOpen = true;
                isMenuAnimating = false;
            });
        }
    }

    public void OpenGameOverMenu()
    {
        if (isMenuAnimating)
            return;

        if (isMenuOpen)
        {
            menuModal.LeanScale(new Vector2(0, 0), menuAnimationTime);
            menuModal.LeanMove(menuButton.localPosition, menuAnimationTime).setOnComplete(() =>
            {
                menuModal.gameObject.SetActive(false);
                isMenuOpen = false;
                isMenuAnimating = false;
                IngameManager.Instance.IsPaused = false;
            });
        }
        else
        {
            IngameManager.Instance.IsPaused = true;
            menuModal.gameObject.SetActive(true);
            menuModal.LeanScale(new Vector2(1, 1), menuAnimationTime);
            menuModal.LeanMove(defaultPositionMenu, menuAnimationTime).setOnComplete(() =>
            {
                isMenuOpen = true;
                isMenuAnimating = false;
            });
        }
    }
}
