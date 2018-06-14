using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

	public float attackSpeed = 0.67f; //Enemy attack time (seconds)
	public int attackDamage = 1; //Hitpoints that the attack takes away

	GameObject player;
	PlayerHealth playerHealth;
	//EnemyHealth enemyHealth;
	bool playerInRange;
	float timer;

	void Awake(){
		player = GameObject.FindGameObjectWithTag ("Player");
		playerHealth = player.GetComponent <PlayerHealth> ();
		//enemyHealth = GetComponent<EnemyHealth> ();
		//animation get components from Animator

		}

	void OnTriggerEnter(Collider other){
		if (other.gameObject == player) {
			playerInRange = true;
			}
		}

	void OnTriggerExit(Collider other){
		if (other.gameObject == player) {
			playerInRange = false;
			}
		}


	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime; 
//		if (timer >= attackSpeed && playerInRange && enemyHealth.currentHealth > 0) //Don't need to getcomponent Enemy's HP b/c its updated in other script
			Attack ();
		
	}
	void Attack() {
		if (playerHealth.currentHealth > 0)
			playerHealth.TakeDamage (attackDamage);
		
	}
}
