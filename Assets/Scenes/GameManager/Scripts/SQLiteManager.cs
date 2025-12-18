
using UnityEngine;
using SQLite;
using System.IO;

public class SQLiteManager : MonoBehaviour
{
    public static SQLiteManager Instance;

    private string dbPath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDatabase();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeDatabase()
    {
        dbPath = Path.Combine(Application.persistentDataPath, "scores.db");
        using (var db = new SQLiteConnection(dbPath))
        {
            db.CreateTable<ScoreRecord>();
        }
        Debug.Log("SQLite инициализирован. Путь: " + dbPath);
    }

    public void SaveScore(int score)
    {
        using (var db = new SQLiteConnection(dbPath))
        {
            db.Insert(new ScoreRecord
            {
                Score = score,
                Date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            });
        }
    }

    public System.Collections.Generic.List<ScoreRecord> GetTopScores(int count = 5)
    {
        using (var db = new SQLiteConnection(dbPath))
        {
            return db.Table<ScoreRecord>()
                     .OrderByDescending(x => x.Score)
                     .Take(count)
                     .ToList();
        }
    }
}