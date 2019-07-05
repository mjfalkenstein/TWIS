﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour { 

    public int maxHealth = 10;
    public int currentHealth;
    public Sprite northSprite;
    public Sprite eastSprite;
    public Sprite southSprite;
    public Sprite westSprite;

    // Start is called before the first frame update
    void Start() {
        currentHealth = maxHealth;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Animator>().enabled = false;
    }

    // Update is called once per frame
    void Update() {
        
    }
}
