using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement ;
using UnityStandardAssets.Characters.FirstPerson;


public class Player : Photon.PunBehaviour {

	private Health player_health;
	public Animator characterAnimator;

	[SerializeField]private Animator resetEkraniAnim;


   [HideInInspector]public bool isDead = false;

   private Vector3 syncPos = Vector3.zero;
   private Quaternion syncRot = Quaternion.identity;
   private FirstPersonController firstPersonController;

   void Awake(){
	   syncPos = transform.position;
	   syncRot = transform.rotation;
   }
   
   void Start(){
	   	if(!photonView.isMine){

			MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
			for (int i = 0; i < scripts.Length; i++){
				MonoBehaviour script = scripts[i];

				if(script == this){
					continue;
				}
				else if(script is PhotonView){
					continue;
				}
				script.enabled = false;
			}
				
			

 			 Destroy(transform.Find("FirstPersonCharacter").gameObject);
			 transform.Find("PlayerCharacter").gameObject.SetActive(true);
			 return;
		   }

		firstPersonController = GetComponent<FirstPersonController>();

	 	Transform inGameUITransform = GameObject.Find("/Canvas/InGame").transform;
		resetEkraniAnim = inGameUITransform.Find("Death").GetComponent<Animator>();
		player_health = GetComponent<Health> (); 
	
   }

   void Update(){
	   if(!photonView.isMine){
		   transform.position = Vector3.Lerp(transform.position, syncPos, 0.1f);
		   transform.rotation = Quaternion.Lerp(transform.rotation, syncRot, 0.1f);

		   return;
	   }
	   CheckHealth();
	   UpdateAnimator();
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

	void UpdateAnimator(){
		photonView.RPC("RPCSyncAnimator", PhotonTargets.Others, firstPersonController.controller.velocity.magnitude != 0);
	}

	[PunRPC]
	void RPCSyncAnimator(bool isMoving){
		characterAnimator.SetBool("IsWalking", isMoving);
	}

	void RestartGame(){
		resetEkraniAnim.SetTrigger("resetTrigger");
		GameManager.instance.ResetGame();
		Destroy(gameObject);
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if(stream.isWriting){
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		}
		else{
			syncPos = (Vector3) stream.ReceiveNext();
			syncRot = (Quaternion) stream.ReceiveNext();
		}
	}
}
