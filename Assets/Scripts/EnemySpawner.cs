using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	//TODO: Yeni prefab ekleme
	 public GameObject zombie;
	 [HideInInspector] public Transform[] spawnPoints;

	//Zaman araliklari
	 public float spawnDuration = 5f;

	 void Start(){
		 spawnPoints = new Transform[transform.childCount];

		 for(int i = 0; i < transform.childCount; i++){
			 spawnPoints[i] = transform.GetChild(i); 
		 }
		 StartCoroutine(StartSpawnings()); //TODO : instantiate var, gameobject yerine tag kullan
	 }

	 IEnumerator StartSpawnings(){
		 while(true){
			 for(int i = 0; i<spawnPoints.Length; i++){
				 Transform spawnPoint = spawnPoints[i];
				 Instantiate(zombie, spawnPoint.position, spawnPoint.rotation);
			 }
			 yield return new WaitForSeconds(spawnDuration);

		 }
	 }

	 void isDied(){
		 
	 }
 }
