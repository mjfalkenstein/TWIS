using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour{

    public GameObject followTarget;
    private Vector3 targetPos;
    public float moveSpeed;

    // Start is called before the first frame update
    void Start() {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame
    void FixedUpdate() {
        targetPos = new Vector3(followTarget.transform.position.x,
                                followTarget.transform.position.y,
                                transform.position.z);

        // Smoothly follow the target (usually the player but can be anything)
        float dist = (targetPos - transform.position).magnitude;
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }
}