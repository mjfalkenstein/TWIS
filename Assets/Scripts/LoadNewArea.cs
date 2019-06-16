using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/**
 * 
 * This class is used to load into a new scene when something collides with it.
 * Ideal for creating a door or loading trigger
 * 
 **/
public class LoadNewArea : MonoBehaviour {

    public string levelToLoad;
    private bool readyToLoad = false;
    private GameObject entering;

    // Start is called before the first frame update
    void Start() { }
    
    // Works with OnTrigger2D to load into a new level only when the object inside has 
    // fully entered the new tile.
    void FixedUpdate() {
        if (readyToLoad) {
            Vector3 otherPos = entering.transform.position;

            // We include the "epsilon" as a pre-defined small value to avoid floating-point rounding issues
            if (otherPos.x % 1 <= (float.Epsilon * 100) && otherPos.y % 1 <= (float.Epsilon * 100)) {
                SceneManager.LoadScene(levelToLoad);
                entering = null;
                readyToLoad = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "Player") {
            entering = other.gameObject;
            readyToLoad = true;
        }
    }
}
