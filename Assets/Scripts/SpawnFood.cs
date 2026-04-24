using UnityEngine;

public class SpawnFood : MonoBehaviour {
  public int MaxAmountFood = 2000;

  public ReplaceFood ReplaceFoodScript;

  // Start is called before the first frame update
  private void Start() {

    for (int i = 0; i < MaxAmountFood; i++) {
      ReplaceFoodScript.CreateFood();
    }

  }
}
