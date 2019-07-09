using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour { 

    public int maxHealth = 10;
    public int currentHealth;
    public int maxMana = 10;
    public int currentMana;
    public Sprite northSprite;
    public Sprite eastSprite;
    public Sprite southSprite;
    public Sprite westSprite;
    public Sprite portrait;

    // Start is called before the first frame update
    void Start() {
        currentHealth = maxHealth;
        currentMana = maxMana;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Animator>().enabled = false;
        print(gameObject.name + " loaded");
    }

    // Update is called once per frame
    void Update() {
        
    }
}
