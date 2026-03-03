using System;
using UnityEngine;

public class StoneThrowable : MonoBehaviour {
  public GameObject reenable;
  [SerializeField] private GameObject explosion;
  public StoneType type;
  private void OnCollisionEnter2D(Collision2D other) {
    if (!other.gameObject.CompareTag("Ground")) return;
    if (type == StoneType.LAVA) Instantiate(explosion, transform.position, transform.rotation);
    reenable.SetActive(true);
    Destroy(gameObject);
  }
}
