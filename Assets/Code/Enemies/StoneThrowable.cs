using UnityEngine;

public class StoneThrowable : MonoBehaviour {
  public GameObject reenable;
  private void OnCollisionEnter2D(Collision2D other) {
    if (!other.gameObject.CompareTag("Ground")) return;
    reenable.SetActive(true);
    Destroy(gameObject);
  }
}
