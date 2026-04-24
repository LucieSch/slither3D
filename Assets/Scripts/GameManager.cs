using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
  public GameObject GameOverPanel;
  public GameObject PausePanel;
  public PlayerSnake PlayerSnakeScript;
  public GameObject NonPlayerPrefab;
  public Text InputText;
  public bool IsStartMenu;

  public const string PLAYER_PREFS_HIGHSCORE = "Scores";
  public const string PLAYER_PREFS_PLAYER_NAME = "PlayerName";


  private readonly int _numOfAISnakes = 100;
  private GameObject _score;
  private GameObject _highscore;

  private void Start() {
    if (!IsStartMenu) {
      for (int i = 0; i < _numOfAISnakes; i++) {
        GameObject NewSnake = Instantiate(NonPlayerPrefab, new Vector3(Random.Range(2f, 38f), 0f, Random.Range(2f, 38f)), NonPlayerPrefab.transform.rotation);
        NewSnake.name = NewSnake.name.Replace("(Clone)", i.ToString());
        NewSnake.transform.GetChild(0).GetComponent<AISnake>().NonPlayerPrefab = NonPlayerPrefab;
      }

      _score = GameObject.Find("/Canvas/Score");
      _highscore = GameObject.Find("Canvas/Highscore");
    }
  }

  private void Update() {
    if (!IsStartMenu && PlayerSnakeScript.SnakeDied) {
      GameOver();
    }
    if (!IsStartMenu && Input.GetKey(KeyCode.Escape)) {
      PauseGame();
    }
  }

  private void GameOver() {
    GameOverPanel.SetActive(true);
    _score.SetActive(false);
    _highscore.SetActive(false);
  }
  public void PlayGame() {
    PlayerPrefs.SetString(PLAYER_PREFS_PLAYER_NAME, InputText.text);
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }
  public void ReplayGame() {
    Time.timeScale = 1;
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }
  public void QuitGame() {
    Application.Quit();
  }
  public void ChangePlayer() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
  }

  private void PauseGame() {
    PausePanel.SetActive(true);
    Time.timeScale = 0;
  }

  public void ResumeGame() {
    PausePanel.SetActive(false);
    Time.timeScale = 1;
  }
}
