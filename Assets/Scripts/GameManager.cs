using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public Transform[] spawnPoints;
	 public EnemySpawner enemySpawner;

	//Zaman araliklari
	 public float spawnDuration = 5f;
	 public int maxZombies = 20;
	 public int zombiesSpawned = 0;

	 public float upgradeDuration = 20f;
	 public int baseZombieHP = 100;
	 public float baseZombieSpeed = 1.0f;

	 public int baseKillReward = 10;

	 public float maxZombieSpeed = 5.0f;

	[SerializeField]private int zombieHP;
	[SerializeField]private float zombieSpeed;
	[SerializeField]private int killReward;



	 void Start(){
		zombieHP = baseZombieHP;
		zombieSpeed = baseZombieSpeed;
		killReward = baseKillReward;

		 StartCoroutine(CoSpawnEnemies());
		 StartCoroutine(CoEnhanceZombieStatus());
	 }
	 
	 IEnumerator CoSpawnEnemies(){
		while(true){
			for(int i = 0; i< spawnPoints.Length; i++){
				if(zombiesSpawned >= maxZombies ) continue;

				GameObject enemyObj = enemySpawner.SpawnAt(spawnPoints[i].position, spawnPoints[i].rotation);
				ZombieFollow enemyZombie = enemyObj.GetComponent<ZombieFollow>();
				Health enemyHealth  = enemyObj.GetComponent<Health>();
				KillZombiOdul enemyKillReward = enemyObj.GetComponent<KillZombiOdul>();

				enemyZombie.speed = zombieSpeed;
				enemyHealth.val = zombieHP;
				enemyKillReward.odul_miktar = killReward;

				enemyZombie.onDead.AddListener(() => {
					Debug.Log("Zombie DEAD!");
					zombiesSpawned--;
				});

				zombiesSpawned++;
			}
			yield return new WaitForSeconds(spawnDuration);
		}
	 }

	 IEnumerator CoEnhanceZombieStatus(){
		 	while(true){
				 yield return new WaitForSeconds(upgradeDuration);

				 Debug.Log("Upgrading Enemy!");

				 zombieHP += 20;
				 zombieSpeed += 0.25f;
				 killReward += 20; 

				 if(zombieSpeed > maxZombieSpeed){
					 zombieSpeed = maxZombieSpeed;
				 }
			 }
	 }

}
