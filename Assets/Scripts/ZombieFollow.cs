using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.Events;

 
public class ZombieFollow : MonoBehaviour {
   
   private GameObject target;
   private NavMeshAgent agent;
   [HideInInspector]public bool isDead = false;
   [HideInInspector] public bool isAttacking = false;
   private Health targetHealth;
   private Player player;

   public float speed = 1.0f;
   public float angularSpeed = 120.0f; //TODO: donus hizini degistir.


   private Health enemy_health;
   private Animator animator;
   private Collider collider_; //zombinin darbe alip olmesi icin collide-trigger
   public float damage = 20f;

   public GameObject kanEffect;
   [HideInInspector] public UnityEvent onDead;

   void Start(){
		target = GameObject.Find("Player");
		 targetHealth = target.GetComponent<Health>();
		if(targetHealth == null){
			throw new System.Exception("Target'de Player Component'i yok");
		}

		player = target.GetComponent<Player>();

			if(player == null){
			throw new System.Exception("Target'de Player Component'i yok");
		}

		agent = GetComponent<NavMeshAgent>(); //TODO : yeni assetler icin tekrar Bake et.
		enemy_health = GetComponent<Health> ();
		animator = GetComponent<Animator>();
		collider_ = GetComponent<Collider>();
   }
   void Update(){
		CheckHealth ();
		Follow ();
		CheckAttack();
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
			Destroy(gameObject, 3f);
		}
	}

	void Follow(){
		if (isDead)
			return;
		else if(player.isDead)
			return;
		agent.destination = target.transform.position; //navmesh icin player takibi
	}

	void CheckAttack(){
		if(isDead)
			return;
		else if(isAttacking)
			return;
		else if(player.isDead)
			return;

		float distanceFromTarget = Vector3.Distance(target.transform.position, transform.position);
		if(distanceFromTarget <= 2.0f){ //2.0f'e kadar takip et sonra saldir!
			Attack();
		}

	}
	void Attack(){

		targetHealth.TakeDamage(damage); //player'a saldir

		agent.speed = 0;
		agent.angularSpeed = 0;
		isAttacking = true;

			//animator.CrossFadeInFixedTime("Attack",0.1f );
		animator.SetTrigger("isAttack");
		Invoke("ResetAttack", 1.2f);
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
}
