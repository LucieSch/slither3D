using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Snake : MonoBehaviour {
  public float TurnSpeed = 180f;
  public float NormalSpeed = 1f;
  public float BoostSpeed = 2f;

  public List<GameObject> BodyParts = new List<GameObject>();

  public GameObject BodyPrefab;
  public GameObject FoodPrefab;
  public Material SnakeMat;
  public bool SnakeDied = false;
  public int MinLength = 4;

  protected float CurrentSpeed;
  protected float TurnDirection;
  protected float BodyPartDistance = 0.025f;
  protected bool StartBoost = false;

  private ReplaceFood _replaceFoodScript;


  protected virtual void Start() {
    _replaceFoodScript = GameObject.Find("FoodManager").GetComponent<ReplaceFood>();
    StartCoroutine(Boost());
  }
  protected virtual void FixedUpdate() {
    if (SnakeDied) {
      // Spawns food at the position of every body part of the dead snake with a random offset
      foreach (Transform child in transform.parent) {
        var offset = new Vector3(Random.Range(-0.08f, 0.08f), 0f, Random.Range(-0.08f, 0.08f));
        GameObject Food = Instantiate(FoodPrefab, child.transform.position + offset, child.transform.rotation);
        Food.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
        Food.GetComponent<MeshRenderer>().material = SnakeMat;
      }
      return;
    }
    Move();
  }
  protected virtual void OnTriggerEnter(Collider other) {
    if (other.gameObject.layer == Tag.Food) {
      if (other.gameObject.transform.localScale == new Vector3(0.04f, 0.04f, 0.04f)) {
        if (_replaceFoodScript == null) {
          _replaceFoodScript = GameObject.Find("FoodManager").GetComponent<ReplaceFood>();
        }
        _replaceFoodScript.NumOfFood--;
        _replaceFoodScript.CreateFood();
      }
      Destroy(other.gameObject);
      Grow();
    }
    if (other.gameObject.layer == Tag.Wall) {
      SnakeDied = true;
    }
    // Snake dies if it collides with another snake but not if it collides with its Food- or Snake Scanner
    if (other.gameObject.layer == Tag.Snake && other.transform.parent.name != transform.parent.name && other.name != "FoodScanner" && other.name != "SnakeScanner") {
      SnakeDied = true;
    }
  }

  private void Grow() {
    GameObject Body = Instantiate(BodyPrefab, BodyParts[BodyParts.Count - 1].transform.position, BodyParts[BodyParts.Count - 1].transform.rotation, transform.parent);
    BodyParts.Add(Body);

    Body.GetComponent<MeshRenderer>().material = SnakeMat;
  }

  private void Move() {
    // moving forward
    transform.Translate(0, 0, Time.deltaTime * CurrentSpeed);

    // move body
    for (int i = 1; i < BodyParts.Count; i++) {
      BodyParts[i].transform.position = BodyParts[i - 1].transform.position + (BodyParts[i].transform.position - BodyParts[i - 1].transform.position).normalized * BodyPartDistance;
      BodyParts[i].transform.LookAt(BodyParts[i - 1].transform.position);
    }
  }

  private IEnumerator Boost() {
    while (true) {
      CurrentSpeed = NormalSpeed;
      if (StartBoost) {
        if (BodyParts.Count > MinLength) {
          CurrentSpeed = BoostSpeed;
          Vector3 SpawnPosition = BodyParts[BodyParts.Count - 1].transform.position;
          Quaternion SpawnRotation = BodyParts[BodyParts.Count - 1].transform.rotation;
          Destroy(BodyParts[BodyParts.Count - 1]);
          BodyParts.RemoveAt(BodyParts.Count - 1);

          yield return new WaitForSeconds(0.2f);
          GameObject Food = Instantiate(FoodPrefab, SpawnPosition, SpawnRotation);
          Food.GetComponent<MeshRenderer>().material = SnakeMat;
          _replaceFoodScript.NumOfFood++;
        }
      }
      yield return new WaitForSeconds(0.2f);
    }
  }
}
