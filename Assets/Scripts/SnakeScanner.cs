using System.Collections.Generic;
using UnityEngine;

public class SnakeScanner : MonoBehaviour {

  public List<Transform> OtherDetectedSnakes = new List<Transform>();
  private void OnTriggerEnter(Collider other) {
    // If collider is triggered by a different snake, all body parts of that snake inside the collider get added to list
    if (other.gameObject.layer == Tag.Snake && other.transform.parent.name != transform.parent.parent.name && other.name != "FoodScanner" && other.name != "SnakeScanner") {
      OtherDetectedSnakes.Add(other.transform);
    }
  }
  private void OnTriggerExit(Collider other) {
    // When a body part of another snake exits collider, it gets removed from list
    if (other.gameObject.layer == Tag.Snake && other.transform.parent.name != transform.parent.parent.name) {
      OtherDetectedSnakes.Remove(other.transform);
    }
  }
}
