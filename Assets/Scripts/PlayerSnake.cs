using UnityEngine;

public class PlayerSnake : Snake {
  protected override void FixedUpdate() {
    base.FixedUpdate();
    if (SnakeDied) {
      transform.parent.gameObject.SetActive(false);
    }

    // turning
    TurnDirection = Input.GetAxisRaw("Horizontal");
    transform.Rotate(0f, TurnDirection * TurnSpeed * Time.deltaTime, 0f, Space.Self);

    // boost
    StartBoost = Input.GetKey("space");
  }
}
