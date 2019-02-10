using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopAlgilama : MonoBehaviour {

	public Transform shootPoint;
	public float algilamaRange = 2f;
	public Text shopText;

	void Update(){
		RaycastHit hit;

		if(Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, algilamaRange)){
				//TODO: Tekrar raycast cagir 
				ShopBase shopBase = hit.transform.GetComponent<ShopBase>();

				if(shopBase == null) {
					shopText.text = " ";
				}

				if(shopBase is MermiShop){
					shopText.text = "Mermi Almak Icin 'F'.";
					Debug.Log("Mermi");
				}
		}
	}
}
