using UnityEngine;
using UnityEngine.Audio;

public class TipCheck : MonoBehaviour {
  private Rigidbody2D playerRB;
  [SerializeField] private AudioResource bonkSound, stabSound;
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
      case "Ground": 
        PlayerController.instance.Bonk(); 
        SoundManager.PlaySound(bonkSound);
        break;
      case "Stone": 
        PlayerController.instance.PickupEnemy(other.GetComponent<SpriteRenderer>()); 
        SoundManager.PlaySound(stabSound);
        break;
    }
  }

  private void SideHit(GameObject other, bool side) {
    switch (other.tag) {
      case "Ground": 
        PlayerController.instance.WallStick(); 
        SoundManager.PlaySound(stabSound);
        break;
    }
  }
}
