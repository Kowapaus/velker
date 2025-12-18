// Assets/Scripts/GameManager.cs
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int coinsCollected = 0;
    public float gameTime = 30f;
    private bool gameEnded = false;

    public delegate void GameEndedDelegate(int coins);
    public event GameEndedDelegate OnGameEnded;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(GameTimer());
    }

    public void CollectCoin()
    {
        coinsCollected++;
        UIManager.Instance?.UpdateCoinsUI();
    }

    IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(gameTime);
        EndGame();
    }

    public void EndGame()
    {
        if (!gameEnded)
        {
            gameEnded = true;
            OnGameEnded?.Invoke(coinsCollected);
        }
    }
}