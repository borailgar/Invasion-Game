using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement ;
using UnityStandardAssets.Characters.FirstPerson;


public class Player : Photon.PunBehaviour {

	public static Player instance;

	private Health player_health;

	private AudioSource audioSource;


	public Animator characterAnimator;

	[SerializeField]private Animator resetEkraniAnim;


   [HideInInspector]public bool isDead = false;

   private Vector3 syncPos = Vector3.zero;
   private Quaternion syncRot = Quaternion.identity;
   private FirstPersonController firstPersonController;

	public Transform characterWeapons;

	public MuzzleFlashController muzzleflashController;
   void Awake(){
	   if(photonView.isMine){
		   instance = this;
	   }

	   syncPos = transform.position;
	   syncRot = transform.rotation;
   }
   
   void Start(){
		audioSource = GetComponents<AudioSource>()[1];
		muzzleflashController = GetComponent<MuzzleFlashController>();

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
				else if(script is Health){
					continue;
				}
				script.enabled = false;
			}
				
			

 			 Destroy(transform.Find("FirstPersonCharacter").gameObject);
			 transform.Find("PlayerCharacter").gameObject.SetActive(true);
			 return;
		   }
		   else{
			   Destroy(transform.Find("PlayerCharacter").gameObject);
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



	public void PlaySoundThroughNetwork(string identifier){
		photonView.RPC("RPCPlaySoundThroughNetwork", PhotonTargets.Others, identifier);
	}
	[PunRPC]
	public void RPCPlaySoundThroughNetwork(string identifier){
		switch(identifier){
			case "DryFire":
				audioSource.PlayOneShot(AudioManager.instance.dryFire);
				break;
			case "UMP45_Fire":
				audioSource.PlayOneShot(AudioManager.instance.UMP45Fire);
				break;
			case "UMP45_Draw":
				audioSource.PlayOneShot(AudioManager.instance.UMP45Draw);
				break;
			case "UMP45_MagIn":
				audioSource.PlayOneShot(AudioManager.instance.UMP45MagIn);
				break;
			case "UMP45_MagOut":
				audioSource.PlayOneShot(AudioManager.instance.UMP45MagOut);
				break;
			case "UMP45_BoltForwarded":
				audioSource.PlayOneShot(AudioManager.instance.UMP45_BoltForwarded);
				break;

			case "Stov Rifle_Fire":
				audioSource.PlayOneShot(AudioManager.instance.Stov_RifleFire);
				break;
			case "Stov Rifle_Draw":
				audioSource.PlayOneShot(AudioManager.instance.Stov_RifleDraw);
				break;
			case "Stov Rifle_MagIn":
				audioSource.PlayOneShot(AudioManager.instance.Stov_RifleMagIn);
				break;
			case "Stov Rifle_MagOut":
				audioSource.PlayOneShot(AudioManager.instance.Stov_RifleMagOut);
				break;
			case "Stov Rifle_BoltForwarded":
				audioSource.PlayOneShot(AudioManager.instance.Stov_Rifle_BoltForwarded);
				break;

			case "Shotgun!_Fire":
				audioSource.PlayOneShot(AudioManager.instance.ShotgunFire);
				break;
			case "Shotgun!_Draw":
				audioSource.PlayOneShot(AudioManager.instance.ShotgunDraw);
				break;
			case "Shotgun!_MagIn":
				audioSource.PlayOneShot(AudioManager.instance.ShotgunMagIn);
				break;
			case "Shotgun!_MagOut":
				audioSource.PlayOneShot(AudioManager.instance.ShotgunMagOut);
				break;
			case "Shotgun!_BoltForwarded":
				audioSource.PlayOneShot(AudioManager.instance.Shotgun_BoltForwarded);
				break;
		
		


		}
	}

	public void PlayMuzzleflashThroughNetwork(string identifier){
		photonView.RPC("RPCPlayMuzzleflashThroughNetwork", PhotonTargets.Others, identifier);
	}

	[PunRPC]
	public void RPCPlayMuzzleflashThroughNetwork(string identifier){
		muzzleflashController.PlayMuzzleflash(identifier);
	}
   public void SetWeapon(Weapons weapon){
	   photonView.RPC("RPCSetWeapon", PhotonTargets.Others, weapon);
   }

	[PunRPC]
	void RPCSetWeapon(Weapons weapon){
		characterAnimator.SetBool("IsUMP45", false);
   		characterAnimator.SetBool("isStovRifle", false);
		characterAnimator.SetBool("isShotgun", false);

	   switch(weapon){
		   case Weapons.UMP45:
		   		characterAnimator.SetBool("IsUMP45", true);
				break;
			case Weapons.StovRifle:
		   		characterAnimator.SetBool("isStovRifle", true);
				break;
			case Weapons.DefenderShotgun:
		   		characterAnimator.SetBool("isShotgun", true);
				break;
	   }

		for(int i = 0 ;i<characterWeapons.childCount; i++ ){
			characterWeapons.GetChild(i).gameObject.SetActive(false);
		}
	   characterWeapons.Find(weapon.ToString()).gameObject.SetActive(true);
	}
	
	public void PlayerFireAnimation(){
	   photonView.RPC("RPCPlayerFireAnimation", PhotonTargets.Others);

	}
	[PunRPC]
	void RPCPlayerFireAnimation(){
		characterAnimator.SetTrigger("Firing");
	}

	public void PlayerReloadAnimation(){
	   photonView.RPC("RPCPlayerReloadAnimation", PhotonTargets.Others);

	}
	[PunRPC]
	void RPCPlayerReloadAnimation(){
		characterAnimator.SetTrigger("Reloading");

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

	public void newOnPhotonPlayerConnected(PhotonPlayer newPlayer){
		SetWeapon(WeaponDegis.instance.GetCurrentWeapon());
	}

}
