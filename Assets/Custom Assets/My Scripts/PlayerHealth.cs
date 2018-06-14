using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public int startingHealth = 5; //Health at the start of a round
	public int currentHealth; 
	bool isDead;
	bool damaged; //true when the player gets damaged, false by default

	void Awake() {
		currentHealth = startingHealth;
	}


	void Start() {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

public void TakeDamage (int amount) {

		damaged = true;

		currentHealth -= amount;

		//UI Elements would go Here along with sound and visual damage

		if (currentHealth <= 0 && !isDead) {
			Death ();

		}

	}

void Death () {
	
		isDead = true;

		//Varies based on variable names for these variables
		//playerMovement.enabled = false; //Disables players ability to move
		//playerShooting.enabled = false; //Disables players ability to cast spells
		print("You died.");
			


	}
}