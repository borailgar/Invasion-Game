using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement ;


public class Player : MonoBehaviour {

	private Health player_health;

	[SerializeField]private Animator resetEkraniAnim;

   [HideInInspector]public bool isDead = false;
   
   void Start(){
	 
		player_health = GetComponent<Health> (); 
	
   }

   void Update(){
	   CheckHealth();
   }


	void CheckHealth(){
		if (isDead)
			return;
		

		if (player_health.val <= 0) {
			isDead = true;
			print("is dead");
			resetEkraniAnim.SetTrigger("reset");
			Invoke("RestartGame", 2f); //bir sonraki method
		}
	}

	void RestartGame(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		
	}
}
