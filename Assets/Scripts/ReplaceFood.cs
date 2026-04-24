using UnityEngine;

public class ReplaceFood : MonoBehaviour {

  public GameObject FoodPrefab;
  public Material[] Materials;
  public int NumOfFood = 0;

  private Terrain _terrain;

  private void Awake() {
    _terrain = Terrain.activeTerrain;
  }

  public void CreateFood() {
    if (NumOfFood < 2000) {
      GameObject Food = Instantiate(FoodPrefab, new Vector3(Random.Range(0, _terrain.terrainData.bounds.size.x), 0.05f, Random.Range(0, _terrain.terrainData.bounds.size.z)), FoodPrefab.transform.rotation);

      Food.GetComponent<MeshRenderer>().material = Materials[Random.Range(0, Materials.Length)];
      Food.name = Food.name.Replace("(Clone)", "");
      NumOfFood++;
    }
  }
}
