using UnityEngine;
using System.Collections.Generic;
using System;


[Serializable]
public class ScoreEntry
{
    public int score;
    //public string playerName;
    public string date;
    public string difficulty;
}

public static class ScoreBoard
{
    const string Key = "ranking";
    const int MaxEntries = 10;

    [Serializable] class Wrapper 
    { 
        public List<ScoreEntry> entries = new List<ScoreEntry>(); 
    }

    public static List<ScoreEntry> GetTop()
    {
        if (!PlayerPrefs.HasKey(Key))
            return new List<ScoreEntry>();

        var json = PlayerPrefs.GetString(Key);
        var wrapper = JsonUtility.FromJson<Wrapper>(json);
        return wrapper.entries;
    }

    public static int Add(int score, string difficulty)
    {
        var entries = GetTop();
        var newEntry = new ScoreEntry
        {
            score = score,
            //playerName = PlayerPrefs.GetString("playerName", "Player"),
            date = DateTime.Now.ToString("dd/MM HH:mm"),
            difficulty = difficulty
        };

        entries.Add(newEntry);
        entries.Sort((a, b) => b.score.CompareTo(a.score));

        if (entries.Count > MaxEntries)
            entries.RemoveRange(MaxEntries, entries.Count - MaxEntries);

        var json = JsonUtility.ToJson(new Wrapper { entries = entries });
        PlayerPrefs.SetString(Key, json);
        PlayerPrefs.Save();   // al final: escribe en disco lo que acabás de guardar

        return entries.IndexOf(newEntry);
    }

}
