using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
  [SerializeField, Header("Physics")] private float speed; 
  [SerializeField] private float drag, jumpPower, jumpTurn;
  [SerializeField] private Transform sprite;
  private SpriteRenderer spr;
  private int dir, dirAirborne;
  private float speedAirborne;

  [SerializeField] private ThrowHandle pickup;
  [SerializeField] private LatchHandle latch;
  [SerializeField] private Sprite[] sprites;
  public static bool stuckToWall;
  private Vector2 stuckPosition;
  
  public static Rigidbody2D rb;

  public static PlayerController instance;
  public static bool grounded;
  private void Awake() {
    if (!instance) instance = this;
    else Destroy(gameObject);
  }

  public static int i;
  public static bool flip;

  #region Input

  [SerializeField, Header("Input")] private InputActionReference leftRightInput;
  [SerializeField] private InputActionReference jumpInput;
  [SerializeField] private float deadZone;

  private void OnEnable() {
    leftRightInput.action.performed += OnLeftRight;
    leftRightInput.action.canceled += OnLeftRight;
    jumpInput.action.canceled += OnJump;
  }
  
  private void OnDisable() {
    leftRightInput.action.performed -= OnLeftRight;
    leftRightInput.action.canceled -= OnLeftRight;
    jumpInput.action.canceled -= OnJump;
  }

  private void OnLeftRight(InputAction.CallbackContext ctx) {
    float val = ctx.ReadValue<float>();
    if (Mathf.Abs(val) < deadZone) dir = 0;
    else dir = val > 0 ? 1 : -1;
  }

  private void OnJump(InputAction.CallbackContext ctx) {
    if (!grounded || stuckToWall || ThrowHandle.holding) return;
    rb.linearVelocity += Vector2.up * jumpPower;
    grounded = false;
  }
  #endregion
  
  private void Start() {
    rb = GetComponent<Rigidbody2D>();
    spr = sprite.GetComponent<SpriteRenderer>();
  }

  private void FixedUpdate() {
    if (stuckToWall) {
      rb.linearVelocity = Vector2.zero;
      return;
    }
    Movement();
    Animation();
  }


  private void Movement() {
    if (grounded) {
      speedAirborne = speed;
      if(dir != 0) dirAirborne = dir;
    }
    Vector2 targetVel = rb.linearVelocity;
    targetVel.x += dir * (grounded ? speed : speedAirborne);
    targetVel.x *= drag;
    
    rb.linearVelocity = targetVel;
  }

  private void Animation() {
    float turnPoint = (Mathf.Clamp(rb.linearVelocity.y, 
      -jumpTurn, 0)) / jumpTurn;
    turnPoint *= 180 * dirAirborne;

    spr.sprite = sprites[i];
    spr.flipX = flip;
    
    sprite.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(sprite.eulerAngles.z, turnPoint, .25f));
  }

  public void Bonk() {
    rb.linearVelocity = Vector2.up * jumpPower / 1.5f;
    speedAirborne /= 2;
    grounded = false;
  }

  public void WallStick() {
    stuckPosition = transform.position;
    stuckToWall = true;
    rb.gravityScale = 0;
    latch.gameObject.SetActive(true);
  }
  
  public void WallUnstick() {
    stuckToWall = false;
    rb.gravityScale = 1;
    dirAirborne = -dirAirborne;
  }

  public void PickupEnemy(SpriteRenderer enemySprite) {
    pickup.PickSomethingUp(enemySprite.GetComponent<StoneRoamer>().type, enemySprite);
    enemySprite.gameObject.SetActive(false);
  }
}
