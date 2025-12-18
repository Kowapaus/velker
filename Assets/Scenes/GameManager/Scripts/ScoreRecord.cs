
using SQLite;

public class ScoreRecord
{
    [SQLite.PrimaryKey, SQLite.AutoIncrement]
    public int Id { get; set; }

    public int Score { get; set; }

    public string Date { get; set; }
}