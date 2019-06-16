using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{

    //public float moveSpeed;
    //public float deadZone;
    //private Animator animator;
    //private Rigidbody2D rigidBody;
    //public float animationSpeed;
    //private float oldDX = 0;
    //private float oldDY = 0;

    //// Start is called before the first frame update
    //void Start(){
    //    animator = gameObject.GetComponent<Animator>();
    //    rigidBody = gameObject.GetComponent<Rigidbody2D>();
    //    animator.speed = animationSpeed;

    //    DontDestroyOnLoad(transform.gameObject);
    //}

    //// Update is called once per frame
    //void Update(){
    //    float horizontal = Input.GetAxisRaw("Horizontal");
    //    float vertical = Input.GetAxisRaw("Vertical");
    //    float dX = Mathf.Abs(horizontal) > deadZone ? horizontal : 0.0f;
    //    float dY = Mathf.Abs(vertical) > deadZone ? vertical : 0.0f;
    //    Vector3 velocity = new Vector2(dX, dY).normalized * moveSpeed;
    //    rigidBody.velocity = velocity;

    //    animator.SetFloat("dX", oldDX);
    //    animator.SetFloat("dY", oldDY);

    //    oldDX = dX;
    //    oldDY = dY;
    //}

    Direction currentDir;
    Vector2 input;
    bool isMoving = false;
    Vector3 startPos;
    Vector3 endPos;
    float t;
    public float walkSpeed = 3.0f;
    public Sprite northSprite;
    public Sprite eastSprite;
    public Sprite southSprite;
    public Sprite westSprite;
    public Collider2D walls;
    bool m_Started;

    void Start() {

        // have to give startPos some initial value in order for the movement to work properly
        startPos = gameObject.transform.position;
    }

    void FixedUpdate() {
        if (!isMoving) {
            handleMovement();
            handleMovementSprite();
        }
    }

    // Set the walking animation for the correct direction
    private void handleMovementSprite() {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        switch (currentDir) {
            case Direction.West:
                sr.sprite = westSprite;
                break;
            case Direction.East:
                sr.sprite = eastSprite;
                break;
            case Direction.North:
                sr.sprite = northSprite;
                break;
            case Direction.South:
                sr.sprite = southSprite;
                break;
        }
    }

    // Handle actually moving the player
    private void handleMovement() {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // don't allow for diagonal movement
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y)) {
            input.y = 0;
        } else {
            input.x = 0;
        }

        if (input != Vector2.zero) {
            if (input.x < 0) {
                currentDir = Direction.West;
                endPos = new Vector2(gameObject.transform.position.x - 1, gameObject.transform.position.y);
            } else if (input.x > 0) {
                currentDir = Direction.East;
                endPos = new Vector2(gameObject.transform.position.x + 1, gameObject.transform.position.y);
            }

            if (input.y < 0) {
                currentDir = Direction.South;
                endPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 1);
            } else if (input.y > 0) {
                currentDir = Direction.North;
                endPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1);
            }

            StartCoroutine(Move(gameObject.transform));
        }
    }

    // Moves the player 1 tile at a time
    public IEnumerator Move(Transform entity) {
        isMoving = true;
        startPos = entity.position;
        t = 0.0f;

        // Check if there is something in the way. If so, kill the subroutine (don't move)
        if (walls.OverlapPoint(new Vector2(endPos.x, endPos.y))) {
            isMoving = false;
            yield break;
        }

        // Move for exactly 1 tile
        while ( t < 1.0f) {
            t += Time.deltaTime * walkSpeed;
            entity.position = Vector3.Lerp(startPos, endPos, t > 1.0f ? 1.0f : t);
            yield return null;
        }

        isMoving = false;
        yield return 0;
    }
}


enum Direction {
    North, East, South, West
}