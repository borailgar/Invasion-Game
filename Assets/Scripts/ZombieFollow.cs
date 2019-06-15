using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.Events;

 
public class ZombieFollow : Photon.MonoBehaviour {
   
   private GameObject target;
   private NavMeshAgent agent;
   [HideInInspector]public bool isDead = false;
   [HideInInspector] public bool isAttacking = false;
   private Health targetHealth;
  // private Player player;

   public float speed = 1.0f;
   public float angularSpeed = 120.0f; //TODO: donus hizini degistir.


   private Health enemy_health;
   private Animator animator;
   private Collider collider_; //zombinin darbe alip olmesi icin collide-trigger
   public float damage = 20f;

   public GameObject kanEffect;
   [HideInInspector] public UnityEvent onDead;

   
   private Vector3 syncPos = Vector3.zero;
   private Quaternion syncRot = Quaternion.identity;

   //[HideInInspector] public Player [] players;

	void Awake(){
		
		syncPos = transform.position;
	    syncRot = transform.rotation;
	}

   void Start(){
		agent = GetComponent<NavMeshAgent>(); //TODO : yeni assetler icin tekrar Bake et.
		enemy_health = GetComponent<Health> ();
		animator = GetComponent<Animator>();
		collider_ = GetComponent<Collider>();

		if(PhotonNetwork.isMasterClient){
			Retargeting();
	
		}
		else{
			agent.enabled = false;
		}



   }
   void Update(){

		if(!PhotonNetwork.isMasterClient){
			SyncTransform();
		}
	
		CheckHealth ();
		Follow ();
		CheckAttack();
   }

	float GetDistanceFromTarget(GameObject player){
		return Vector3.Distance(player.transform.position, transform.position);
	}

	GameObject GetClosestTarget(){

		Player[] players = GameManager.instance.players;

		for(int i = 0; i<players.Length; i++){
			if(players[i] == null){
				GameManager.instance.RefreshCurrentPlayers();
				return null;
			}
		}

		GameObject closestTarget = players[0].gameObject;
		float minDist = 9999999;

		for(int i = 0; i<players.Length; i++){
			float dist_ = GetDistanceFromTarget(players[i].gameObject);
			Health playerHealt = players[i].GetComponent<Health>();

			if(dist_ < minDist && playerHealt.val > 0){
				minDist = dist_;
				closestTarget = players[i].gameObject;
			}
		}

		return closestTarget;
	}


	void SyncTransform(){
		transform.position = Vector3.Lerp(transform.position, syncPos, 0.1f);
		transform.rotation = Quaternion.Lerp(transform.rotation, syncRot, 0.1f);
	}
//zombi saglik kontrolu
	void CheckHealth(){
		if (isDead)
			return;
		
		if (isAttacking)
			return;

		if (enemy_health.val <= 0) {
			onDead.Invoke();

			isDead = true;
			agent.isStopped = true;
			collider_.enabled = false; 
			//Destroy (gameObject);
			animator.CrossFadeInFixedTime("Dead", 0.1f);

			BroadcastDead();
			DestroyAfterTime(gameObject, 3);
		}
	}

	void DestroyAfterTime(GameObject obj, float time){
		StartCoroutine(CoDestroyAfterTime(obj, time));
	}

	IEnumerator CoDestroyAfterTime(GameObject obj, float time){
		yield return new WaitForSeconds(time);
		PhotonNetwork.Destroy(obj);
	}

	private bool isLateUpdating = false;
	IEnumerator CoLateUpdateDestination(float latency){
		
		isLateUpdating = true;

		yield return new WaitForSeconds(latency);

		if(target == null){
			Retargeting();
		}
		else{
			agent.destination = target.transform.position;
		}

		isLateUpdating = false;
	}
	void Follow(){
		if (isDead)
			return;

		GameObject closestTarget = GetClosestTarget();
		if(closestTarget == null){
			return;
		}
		Retargeting();


		float dis = GetDistanceFromTarget(target);

		if(dis >= 20 ){
			if(!isLateUpdating){
				agent.destination = target.transform.position; //navmesh icin player takibi
				StartCoroutine(CoLateUpdateDestination(0.1f));
			}
		}
		else{
			if(dis <= 40){
				StartCoroutine(CoLateUpdateDestination(0.5f));
			}
			else if( dis<=60 ){
				StartCoroutine(CoLateUpdateDestination(1.0f));
			}
			else{
				StartCoroutine(CoLateUpdateDestination(2.0f));
			}
		}
	}


	void CheckAttack(){
		if(isDead)
			return;
		else if(isAttacking)
			return;
		else if(targetHealth.val <= 0){
			Retargeting();
			return;
		}

		float distanceFromTarget = Vector3.Distance(target.transform.position, transform.position);
		if(distanceFromTarget <= 2.0f){ //2.0f'e kadar takip et sonra saldir!
			Attack();
		}

	}

	void Retargeting(){
		GameObject closestTarget = GetClosestTarget();

		if(closestTarget == null){
			return;
		}

			target = closestTarget;
			targetHealth = target.GetComponent<Health>();
	}


	void Attack(){

		targetHealth.TakeDamage(damage); //player'a saldir

		agent.speed = 0;
		agent.angularSpeed = 0;
		isAttacking = true;

		animator.SetTrigger("isAttack");
		BroadcastAttackAnimation();
		Invoke("ResetAttack", 1.2f);
	}

	void BroadcastAttackAnimation(){
		photonView.RPC("RPCBroadcastAttackAnimation", PhotonTargets.Others);
	}
	[PunRPC]
	void RPCBroadcastAttackAnimation(){
		animator.SetTrigger("isAttack");

	}

	void BroadcastDead(){
		photonView.RPC("RPCBroadcastDead", PhotonTargets.Others);
	}
	[PunRPC]
	void RPCBroadcastDead(){
		animator.CrossFadeInFixedTime("Dead", 0.1f);
		isDead = true;
		collider_.enabled = false;
	}
 	void ResetAttack(){ //uzaklasma oldugunda (2.0f<) saldiriyi durdur, takip etmeye devam et.
		isAttacking = false;
		agent.speed = speed;
		agent.angularSpeed = angularSpeed;
	}

	public void CreateBloodEffect(Vector3 pos, Quaternion rot){
		
		GameObject blood = Instantiate(kanEffect, pos, rot); 
		Destroy(blood, 1f);

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
