using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

	//[SerializeField] private string scene;
	 public int sceneIndex;
	public GameObject loadGUI;
	
	
	


	void Start(){
		loadGUI.SetActive(false);
	}

	void OnTriggerEnter(Collider col ){
		if(col.CompareTag("Player")){
			loadGUI.SetActive(true);
			if(loadGUI.activeInHierarchy == true  ){
				Debug.Log("KKKK");
				LoadLevel(sceneIndex);
			}
 		}
	}
void OnTriggerExit(Collider col ){
		if(col.CompareTag("Player")){
			loadGUI.SetActive(false);
			
 		}
	}

	public void LoadLevel(int sceneIndex){
		 SceneManager.LoadSceneAsync(sceneIndex);
		//Application.LoadLevel(sceneIndex);
	}

	
}
