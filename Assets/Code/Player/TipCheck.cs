using UnityEngine;

public class TipCheck : MonoBehaviour {
  private Rigidbody2D playerRB;
  private void OnTriggerStay2D(Collider2D other) {
    if (PlayerController.stuckToWall) return;
    if(Vector2.Dot(transform.up, Vector2.down) > .75f)
      DownHit(other.gameObject);
    if (ThrowHandle.holding) return;
    if(Vector2.Dot(transform.up, Vector2.left) > .9f)
      SideHit(other.gameObject, false);
    if(Vector2.Dot(transform.up, Vector2.right) > .9f)
      SideHit(other.gameObject, true);
  }

  private void DownHit(GameObject other) {
    switch (other.tag) {
      case "Ground": PlayerController.instance.Bonk(); break;
      case "Stone": PlayerController.instance.PickupEnemy(other.GetComponent<SpriteRenderer>()); break;
    }
  }

  private void SideHit(GameObject other, bool side) {
    switch (other.tag) {
      case "Ground": PlayerController.instance.WallStick(); break;
    }
  }
}
