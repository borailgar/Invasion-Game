using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement ;


public class PlayerDeath : MonoBehaviour {

	public float player_e = 100f;
	public float damage_ = 20.0f;

	   [HideInInspector]public bool isDead = false;
	   
	   public void takeDamage(float damage_){
		   player_e -= damage_;
		   if(player_e <= 0){
			   isDead = true;
			   player_e = 0f;
			   print("PLAYER DEAD!!");
			   Invoke("Reset", 2f);
		   }

	   }

	   void Reset(){
		   		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	   }
}
