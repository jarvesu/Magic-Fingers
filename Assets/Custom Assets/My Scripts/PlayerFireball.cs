using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireball : MonoBehaviour {

	//Attach this script to the aiming hand and it will shoot fireballs using hand gestures from the casting hand. 
	[SerializeField]
	int blastTimer = 0; //counts the number of frames that the hand is pinching.
	Hi5HandInteraction hi5HandInteraction;
	PlayerHealth playerHealth;
	GameObject player;
	GameObject castingHand; //Hand which gestures to create spells
	GameObject aimingHand; //Hand which is used to aim the spells 
	//Will need another script to determine which hand is which via player input (for lefties and righties)

	public GameObject fireballPrefab; //The fireball Game Object

	public Transform fireballSpawn; //The object from which the fireball spawns.




	void Awake(){
		
		player = GameObject.FindGameObjectWithTag ("Player");
		aimingHand = GameObject.FindGameObjectWithTag ("aimingHand");
		castingHand = GameObject.FindGameObjectWithTag ("castingHand");
		hi5HandInteraction = castingHand.GetComponent <Hi5HandInteraction>(); //Don't need to specify if its on the same object

	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (hi5HandInteraction.m_IsPinching == true) { //If the gesture is performed...
			blastTimer++; 
			if (blastTimer == 1) { 
				Blast (); //Fireballs are launched during the first frame.		
			}
		}
		if (hi5HandInteraction.m_IsPinching == false) { //Resets the amount of time the hand has been pinching.
			blastTimer = 0;
		}	
	}
	void Blast(){
		var fireball = (GameObject)Instantiate (
			               fireballPrefab,
			               fireballSpawn.position,
			               fireballSpawn.rotation);

		fireball.GetComponent<Rigidbody> ().velocity = fireball.transform.right * 8; //projectile velocity
		Destroy(fireball, 8.0f); //After 8 seconds, destroy the fireball. Add condition to destroy if it collides with an object before then.

	}
}