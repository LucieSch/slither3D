using UnityEngine;

public class CameraFollow : MonoBehaviour {
  public float ZoomFactor = 0.005f;

  private Vector3 _baseOffset = new Vector3(0.7f, 1.6f, 0f);
  private PlayerSnake _player;

  private void Start() {
    _player = GameObject.Find("Player/Snake").GetComponent<PlayerSnake>();
  }
  private void LateUpdate() {
    Vector3 offset = _baseOffset + (_player.BodyParts.Count - _player.MinLength) * ZoomFactor * Vector3.up;
    transform.position = _player.transform.position + offset;
  }
}
