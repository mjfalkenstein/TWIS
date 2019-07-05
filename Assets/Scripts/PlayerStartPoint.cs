using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartPoint : MonoBehaviour {

    private PlayerController player;
    private CameraController cameraController;

    // Start is called before the first frame update
    void Start() {
        player = FindObjectOfType<PlayerController>();
        cameraController = FindObjectOfType<CameraController>();

        player.teleportToPosition(transform.position);
        cameraController.transform.position = new Vector3(transform.position.x, 
                                                          transform.position.y,
                                                          cameraController.transform.position.z);
    }
}
