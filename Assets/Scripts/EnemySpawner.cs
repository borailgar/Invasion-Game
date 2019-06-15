using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Photon.MonoBehaviour {

//	 public GameObject zombie;
	//TODO: Yeni prefab ekleme
	public string zombiePrefabName;
	 public GameObject SpawnAt(Vector3 pos, Quaternion rot ){
			
			//GameObject enemy = Instantiate(zombie, pos, rot);
	 		GameObject enemy = PhotonNetwork.Instantiate(zombiePrefabName, pos, rot, 0);
			return enemy;
	 	}	


 }
