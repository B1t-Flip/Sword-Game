using UnityEngine;

public class Door : MonoBehaviour {
  private static readonly int Open = Animator.StringToHash("Open");
  private Animator anim;
  private Collider2D col;

  private void Start() {
    anim = GetComponent<Animator>();
    col = GetComponent<Collider2D>();
  }

  public void OpenDoor() {
    anim.SetTrigger(Open);
    col.enabled = false;
  }
}
