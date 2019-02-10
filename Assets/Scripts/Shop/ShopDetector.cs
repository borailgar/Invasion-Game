using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopDetector : MonoBehaviour {

	public Transform shootPoint;
	public float detectRange = 2f;
	public Text shopText;

	void Update(){
		RaycastHit hit; 

		if(Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, detectRange)){
			ShopBase shopBase = hit.transform.GetComponent<ShopBase>();
			Debug.Log("DEGDI");

			if(shopBase == null) {
				shopText.text = "";
			}

			if(shopBase is AmmoShop){
				shopText.text = "Mermi almak icin F tusuna basin(" + shopBase.cost + "TL)" ;
				Debug.Log("DEGDI!!!!!!");

			}
			else if(shopBase is Police9mmShop){
				shopText.text = "Tabanca almak icin F tusuna basin(" + shopBase.cost + "TL)" ;
				Debug.Log("DEGDI tabanca");
			}

			//shopText.text += " (" + shopBase.cost + "TL)";
		}
	} 
}
