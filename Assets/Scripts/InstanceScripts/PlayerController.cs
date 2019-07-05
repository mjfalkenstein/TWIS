using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour{

    private Animator animator;
    private static bool exists = false;
    private static int partyIndex = 99;
    private CharacterController currentCharacter;
    private SpriteRenderer sr;

    Direction currentDir = Direction.South;
    Vector2 input;
    bool isMoving = false;
    Vector3 startPos;
    Vector3 endPos;
    float t;

    public float walkSpeed = 3.0f;
    public float animationSpeed;
    public Collider2D walls;

    void Start() {

        // have to give startPos some initial value in order for the movement to work properly
        startPos = gameObject.transform.position;
        SetNextCharacterController();
        sr = gameObject.GetComponent<SpriteRenderer>();

        // prevents the game from accidentally creating multiple players
        if (!exists) {
            exists = true;
            DontDestroyOnLoad(transform.gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void FixedUpdate() {
        if (!walls) walls = GetWalls();

        // If we're not moving, accept movement input and update the sprite
        if (!isMoving) {
            HandleMovement();
            HandleMovementSprite();
            HandleInput();

            // Only reset the animation if the player has stopped providing movement input
            if (input.magnitude == 0.0f) {
                animator.speed = 0.0f;
                animator.Play(CurrentAnimationName(), 0, 0.0f);
            }

        // if we are moving, make sure the walking animation is playing
        } else {
            animator.speed = animationSpeed;
        }
    }

    // Get the name of the currently running animation, needed to set the animation back
    // to the first frame so we can pause it when not moving
    string CurrentAnimationName() {
        var currAnimName = "";
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips) {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(clip.name)) {
                currAnimName = clip.name.ToString();
            }
        }
        return currAnimName;
    }

    // do something on new level load
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        walls = GetWalls();
    }

    // we need to dynamically load the walls every time the level changes
    private Collider2D GetWalls() {
        GameObject obj = GameObject.Find("Collision");
        if (obj) {
            return obj.GetComponent<Collider2D>();
        }
        return null;
    }

    private void HandleInput() {
        if (Input.GetKeyUp(KeyCode.Tab)) {
            SetNextCharacterController();
        }
    }

    // Set the walking animation for the correct direction
    private void HandleMovementSprite() {
        int dX = 0;
        int dY = 0;
        switch (currentDir) {
            case Direction.West:
                sr.sprite = currentCharacter.westSprite;
                dX = -1;
                break;
            case Direction.East:
                sr.sprite = currentCharacter.eastSprite;
                dX = 1;
                break;
            case Direction.North:
                sr.sprite = currentCharacter.northSprite;
                dY = 1;
                break;
            case Direction.South:
                sr.sprite = currentCharacter.southSprite;
                dY = -1;
                break;
        }
        animator.SetFloat("dX", dX);
        animator.SetFloat("dY", dY);
    }

    // Handle actually moving the player
    private void HandleMovement() {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // don't allow for diagonal movement
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y)) {
            input.y = 0;
        } else {
            input.x = 0;
        }

        // Set endPos, which will be used for movement and collision detection
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

            StartCoroutine(Move());
        }
    }

    // Moves the player 1 tile at a time
    public IEnumerator Move() {
        isMoving = true;
        Transform self = gameObject.transform;
        startPos = self.position;
        t = 0.0f;

        // Check if there is something in the way. If so, kill the subroutine (don't move)
        Collider2D collision = Physics2D.OverlapPoint(new Vector2(endPos.x, endPos.y));
        if (collision) {
            if (collision.name.ToLower() == "collision") { 
                isMoving = false;
                yield break;
                }
        }

        // Move for exactly 1 tile
        while ( t < 1.0f) {
            t += Time.deltaTime * walkSpeed;
            self.position = Vector3.Lerp(startPos, endPos, t > 1.0f ? 1.0f : t);
            currentCharacter.transform.position = self.position;
            yield return null;
        }

        // If by some reason the player's position becomes slightly not-perfect, just round to the nearest tile
        // This happened a couple times while testing where the position would be off by an extremely small amount
        // which was not visible, but could cause issues down the line
        self.position = new Vector3(Mathf.RoundToInt(self.position.x), Mathf.RoundToInt(self.position.y), self.position.z);
        isMoving = false;
        yield return 0;
    }

    // Get the next characterController in the Party
    private void SetNextCharacterController() {
        GameObject partyObject = GameObject.Find("Party");
        int partyChildCount = partyObject.transform.childCount;
        partyIndex = partyIndex >= partyChildCount - 1 ? 0 : partyIndex + 1;

        if (currentCharacter != null) {
            currentCharacter.GetComponent<SpriteRenderer>().enabled = false;
            currentCharacter.GetComponent<Animator>().enabled = false;
        }
        currentCharacter = partyObject.transform.GetChild(partyIndex).GetComponent<CharacterController>();
        currentCharacter.transform.position = gameObject.transform.position;
        currentCharacter.GetComponent<SpriteRenderer>().enabled = true;
        currentCharacter.GetComponent<Animator>().enabled = true;

        animator = currentCharacter.GetComponent<Animator>();
        CopyComponent(currentCharacter.GetComponent<SpriteRenderer>(), gameObject);
        CopyComponent(animator, gameObject);
    }

    T CopyComponent<T>(T original, GameObject destination) where T : Component {
        System.Type type = original.GetType();
        Component copy = destination.GetComponent<T>();
        if (copy == null) {
            copy = destination.AddComponent(type);
        }
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields) {
            print(field.Name);
            field.SetValue(copy, field.GetValue(original));
        }
        return copy as T;
    }

    public void teleportToPosition(Vector3 dest) {
        gameObject.transform.position = dest;
        currentCharacter.transform.position = dest;
    }

    // Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
    void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    // Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. 
    // Remember to always have an unsubscription for every delegate you subscribe to!
    void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
}


enum Direction {
    North, East, South, West
}