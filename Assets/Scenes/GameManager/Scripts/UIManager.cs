// Assets/Scripts/UIManager.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    // UI-элементы игры
    public TMP_Text coinText;
    public TMP_Text timerText;
    public GameObject endGamePanel;
    public TMP_Text finalScoreText;

    // UI-элементы рекордов
    public GameObject highScoresPanel;     // Панель "Рекорды"
    public GameObject scoreEntryPrefab;    // Префаб одной строки (TextMeshPro)
    public Transform scoresList;           // Контейнер для динамических записей

    private float remainingTime;
    private bool gameEnded = false;

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
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager не найден!");
            enabled = false;
            return;
        }

        GameManager.Instance.OnGameEnded += OnGameEnded;
        remainingTime = GameManager.Instance.gameTime;
        UpdateCoinsUI();
        StartCoroutine(UpdateTimer());
    }

    IEnumerator UpdateTimer()
    {
        while (!gameEnded && remainingTime > 0)
        {
            if (timerText != null)
                timerText.text = "Время: " + Mathf.Ceil(remainingTime).ToString();
            remainingTime -= Time.deltaTime;
            yield return null;
        }

        if (!gameEnded)
        {
            GameManager.Instance.EndGame();
        }
    }

    public void UpdateCoinsUI()
    {
        if (coinText != null)
            coinText.text = "Монеты: " + GameManager.Instance.coinsCollected;
    }

    void OnGameEnded(int coins)
    {
        gameEnded = true;
        if (finalScoreText != null)
            finalScoreText.text = "Собрано монет: " + coins;
        if (endGamePanel != null)
            endGamePanel.SetActive(true);

        // Сохраняем результат
        SQLiteManager.Instance?.SaveScore(coins);

        // Показываем курсор
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }

    // === КНОПКА "ОБНОВИТЬ РЕКОРДЫ" ===
    public void RefreshHighScores()
    {
        if (highScoresPanel == null || scoresList == null || scoreEntryPrefab == null)
        {
            Debug.LogError("UIManager: не все элементы рекордов подключены в Inspector!");
            return;
        }

        // Показываем панель рекордов
        highScoresPanel.SetActive(true);

        // Удаляем старые записи
        foreach (Transform child in scoresList)
        {
            Destroy(child.gameObject);
        }

        // Загружаем рекорды из SQLite
        var scores = SQLiteManager.Instance?.GetTopScores(5);
        if (scores == null || scores.Count == 0)
        {
            // Нет рекордов
            var entry = Instantiate(scoreEntryPrefab, scoresList);
            entry.GetComponent<TMP_Text>().text = "<i>Нет рекордов</i>";
            return;
        }

        // Создаём записи
        foreach (var record in scores)
        {
            var entry = Instantiate(scoreEntryPrefab, scoresList);
            entry.GetComponent<TMP_Text>().text = $"{record.Score} — {record.Date}";
        }
    }

    // === (Опционально) Кнопка "Назад" из панели рекордов ===
    public void HideHighScores()
    {
        if (highScoresPanel != null)
            highScoresPanel.SetActive(false);
    }
}