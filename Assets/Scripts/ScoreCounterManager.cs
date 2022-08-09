using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounterManager : MonoBehaviour
{
    private IngameManager gameManager;
    public float UpdateScoreFrequency => updateScoreFrequency;

    [Header("Options")]
    [SerializeField]
    private float updateScoreFrequency = 0.1f;
    [SerializeField]
    private int scoreMultiplier = 10;

    void Start()
    {
        gameManager = IngameManager.Instance;
    }

    public void OnUpdateScore(int times)
    {
        gameManager.IncreaseScore(times * scoreMultiplier);
    }
}
