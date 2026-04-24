using UnityEngine;
#nullable enable

public class FoodScanner : MonoBehaviour {

  public Transform? NewTarget;

  private void OnTriggerEnter(Collider other) {
    if (other.gameObject.layer == Tag.Food) {
      NewTarget = other.transform;
    }
  }
}
