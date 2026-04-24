using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreDisplay : MonoBehaviour {
  public ScoreDisplay ScoreDisplay;
  public Text CurrentHighscoreText;
  public Text PlayerNamesText;
  public Text HighscoresText;

  private string _playerName;
  private int _highscore;
  private bool _isHighscoreDisplayUpdated = false;
  private const string CurrentHighscorePrefix = "Personal Best: ";

  private void Start() {
    _playerName = PlayerPrefs.GetString(GameManager.PLAYER_PREFS_PLAYER_NAME);
    if (string.IsNullOrEmpty(_playerName)) {
      _playerName = "<Anonymous>";
    }
    Highscore highscore = GetScore();

    _highscore = 0;

    if (TryGetPlayersHighestScore(highscore, _playerName, out ScoreEntry score)) {
      _highscore = score.Score;
    }

    CurrentHighscoreText.text = CurrentHighscorePrefix + _highscore;
  }

  private void Update() {
    int currentScore = ScoreDisplay.Score;

    if (currentScore > _highscore) {
      _highscore = currentScore;
      CurrentHighscoreText.text = CurrentHighscorePrefix + _highscore;
    }

    if (ScoreDisplay.PlayerSnake.SnakeDied && !_isHighscoreDisplayUpdated) {
      SaveScore(_playerName, currentScore);
      Highscore highscore = GetScore();
      PlayerNamesText.text = string.Join("\n", highscore.Scores.Select(it => $"{it.Name}"));
      HighscoresText.text = string.Join("\n", highscore.Scores.Select(it => $"{it.Score}"));
      _isHighscoreDisplayUpdated = true;
    }
  }

  [Serializable]
  public class ScoreEntry {
    public string Name;
    public int Score;
  }

  [Serializable]
  public class Highscore {
    public List<ScoreEntry> Scores = new List<ScoreEntry>();
  }

  public Highscore GetScore() {
    string json = PlayerPrefs.GetString(GameManager.PLAYER_PREFS_HIGHSCORE);
    Highscore highscore = JsonUtility.FromJson<Highscore>(json) ?? new Highscore();
    //sort list by score
    highscore.Scores.Sort((ScoreEntry x, ScoreEntry y) => y.Score.CompareTo(x.Score));
    return highscore;
  }

  public void SaveScore(string playerName, int score) {
    string json = PlayerPrefs.GetString(GameManager.PLAYER_PREFS_HIGHSCORE);
    Highscore highscore = JsonUtility.FromJson<Highscore>(json) ?? new Highscore();

    highscore.Scores.Add(new ScoreEntry { Name = playerName, Score = score });

    string resultJson = JsonUtility.ToJson(highscore, true);
    PlayerPrefs.SetString(GameManager.PLAYER_PREFS_HIGHSCORE, resultJson);
    PlayerPrefs.Save();
  }

  public static bool TryGetPlayersHighestScore(Highscore highscore, string playerName, out ScoreEntry score) {
    score = highscore.Scores.FirstOrDefault(it => it.Name == playerName);
    return score != null;
  }
}
