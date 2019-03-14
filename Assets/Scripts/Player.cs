using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement ;


public class Player : MonoBehaviour {

	private Health player_health;

	[SerializeField]private Animator resetEkraniAnim;

   [HideInInspector]public bool isDead = false;
   
   void Start(){
	 	Transform inGameUITransform = GameObject.Find("/Canvas/InGame").transform;
		resetEkraniAnim = inGameUITransform.Find("Death").GetComponent<Animator>();
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
			//print("is dead");
			resetEkraniAnim.SetTrigger("reset");
			GameManager.instance.GameOver();

			Invoke("RestartGame", 1);
		}
	}
	void RestartGame(){
		resetEkraniAnim.SetTrigger("resetTrigger");
		GameManager.instance.ResetGame();
		Destroy(gameObject);
	}
}
