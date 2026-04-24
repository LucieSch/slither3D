using UnityEngine;

public class AISnake : Snake {
  private Terrain _terrain;
  private readonly float _gap = 2f;

  public FoodScanner _foodScanner;
  public SnakeScanner _snakeScanner;
  public GameObject NonPlayerPrefab;
  public Material[] Materials;

  private bool TryDodgeOtherSnakes(out float dodgeDirection) {
    bool SnakeIsNear = false;
    dodgeDirection = Direction.Straight;
    float MinDistance = 1f;
    for (int i = 0; i < _snakeScanner.OtherDetectedSnakes.Count; i++) {
      if (_snakeScanner.OtherDetectedSnakes[i] != null) {
        if (Vector3.Distance(transform.position, _snakeScanner.OtherDetectedSnakes[i].transform.position) <= MinDistance) {
          SnakeIsNear = true;

          if (transform.position.z <= _snakeScanner.OtherDetectedSnakes[i].transform.position.z) {
            dodgeDirection = Direction.Right;
          } else {
            dodgeDirection = Direction.Left;
          }
        }
      } else {
        _snakeScanner.OtherDetectedSnakes.RemoveAt(i);
      }
    }
    return SnakeIsNear;
  }

  private void CalculateAI() {
    // In case the AI is in one of the corners
    if (transform.position.z >= (_terrain.terrainData.bounds.size.z - _gap) && transform.position.x >= (_terrain.terrainData.bounds.size.x - _gap)) {
      if (transform.rotation.eulerAngles.y <= 180 && transform.rotation.eulerAngles.y >= 45) {
        TurnDirection = Direction.Right;
      } else if (transform.rotation.eulerAngles.y < 45 || transform.rotation.eulerAngles.y >= 270) {
        TurnDirection = Direction.Left;
      } else {
        TurnDirection = Direction.Straight;
      }
      return;
    } else if (transform.position.z <= _gap && transform.position.x >= (_terrain.terrainData.bounds.size.x - _gap)) {
      if (transform.rotation.eulerAngles.y >= 0 && transform.rotation.eulerAngles.y <= 135) {
        TurnDirection = Direction.Left;
      } else if (transform.rotation.eulerAngles.y > 135 && transform.rotation.eulerAngles.y <= 270) {
        TurnDirection = Direction.Right;
      } else {
        TurnDirection = Direction.Straight;
      }
      return;
    } else if (transform.position.z <= _gap && transform.position.x <= _gap) {
      if (transform.rotation.eulerAngles.y <= 360 && transform.rotation.eulerAngles.y >= 225) {
        TurnDirection = Direction.Right;
      } else if (transform.rotation.eulerAngles.y >= 90 && transform.rotation.eulerAngles.y < 225) {
        TurnDirection = Direction.Left;
      } else {
        TurnDirection = Direction.Straight;
      }
      return;
    } else if (transform.position.z >= (_terrain.terrainData.bounds.size.z - _gap) && transform.position.x <= _gap) {
      if (transform.rotation.eulerAngles.y >= 90 && transform.rotation.eulerAngles.y <= 315) {
        TurnDirection = Direction.Left;
      } else if (transform.rotation.eulerAngles.y > 315 || (transform.rotation.eulerAngles.y >= 0 && transform.rotation.eulerAngles.y < 90)) {
        TurnDirection = Direction.Right;
      } else {
        TurnDirection = Direction.Straight;
      }
      return;
    }
    // In case the AI is at the very end of terrain
    if (transform.position.z >= (_terrain.terrainData.bounds.size.z - _gap)) {
      if (transform.rotation.eulerAngles.y < 90) {
        TurnDirection = Direction.Right;
      } else if (transform.rotation.eulerAngles.y > 270) {
        TurnDirection = Direction.Left;
      } else {
        TurnDirection = Direction.Straight;
      }
    } else if (transform.position.z <= _gap) {
      if (transform.rotation.eulerAngles.y > 90 && transform.rotation.eulerAngles.y < 270) {
        if (transform.rotation.eulerAngles.y <= 180) {
          TurnDirection = Direction.Left;
        } else {
          TurnDirection = Direction.Right;
        }
      } else {
        TurnDirection = Direction.Straight;
      }
    } else if (transform.position.x >= (_terrain.terrainData.bounds.size.x - _gap)) {
      if (transform.rotation.eulerAngles.y > 0 && transform.rotation.eulerAngles.y < 180) {
        if (transform.rotation.eulerAngles.y <= 90) {
          TurnDirection = Direction.Left;
        } else {
          TurnDirection = Direction.Right;
        }
      } else {
        TurnDirection = Direction.Straight;
      }
    } else if (transform.position.x <= _gap) {
      if (transform.rotation.eulerAngles.y > 180 && transform.rotation.eulerAngles.y < 360) {
        if (transform.rotation.eulerAngles.y <= 270) {
          TurnDirection = Direction.Left;
        } else {
          TurnDirection = Direction.Right;
        }
      } else {
        TurnDirection = Direction.Straight;
      }
    } else {
      TurnDirection = Direction.Straight;
    }
  }

  private void RespawnAI() {
    GameObject NewSnake = Instantiate(NonPlayerPrefab, new Vector3(Random.Range(2f, 38f), 0f, Random.Range(2f, 38f)), NonPlayerPrefab.transform.rotation);
    NewSnake.name = transform.parent.name;
    NewSnake.transform.GetChild(0).GetComponent<AISnake>().NonPlayerPrefab = NonPlayerPrefab;

    Destroy(transform.parent.gameObject);
  }

  private void Awake() {
    _terrain = Terrain.activeTerrain;
  }

  protected override void Start() {
    base.Start();
    SnakeMat = Materials[Random.Range(0, Materials.Length)];
    for (int i = 0; i < BodyParts.Count; i++) {
      BodyParts[i].GetComponent<MeshRenderer>().material = SnakeMat;
    }
  }

  protected override void FixedUpdate() {
    base.FixedUpdate();
    if (SnakeDied) {
      RespawnAI();
      return;
    }

    // turning
    if (_foodScanner.NewTarget != null) {
      transform.LookAt(_foodScanner.NewTarget);
    }
    if (TryDodgeOtherSnakes(out float dodgeDirection)) {
      TurnDirection = dodgeDirection;
      transform.Rotate(0f, TurnDirection * TurnSpeed * Time.deltaTime, 0f, Space.Self);
      _foodScanner.NewTarget = null;
    } else {
      CalculateAI();
      transform.Rotate(0f, TurnDirection * TurnSpeed * Time.deltaTime, 0f, Space.Self);
    }

    // boost randomly
    StartBoost = Random.value > 0.975f;
  }

  protected override void OnTriggerEnter(Collider other) {
    base.OnTriggerEnter(other);
    if (other.gameObject.layer == Tag.Food) {
      _foodScanner.NewTarget = null;
    }
  }
}
