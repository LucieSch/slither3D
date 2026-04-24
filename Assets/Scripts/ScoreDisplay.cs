using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {
  public Text ScoreText;
  public PlayerSnake PlayerSnake;
  public int Score;

  private void Update() {
    if (!PlayerSnake.SnakeDied) {
      Score = PlayerSnake.BodyParts.Count - PlayerSnake.MinLength;
      ScoreText.text = Score.ToString();
    }
  }
}
