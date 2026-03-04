using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour {
  [SerializeField] private UnityEvent OnPressed;

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.TryGetComponent(out StoneThrowable stone)) {
      OnPressed.Invoke();
      stone.reenable.SetActive(true);
      Destroy(stone.gameObject);
      Destroy(gameObject);
    }
  }
}
