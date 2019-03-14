using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour {

	private Health health;
	public Text healthText;
	public Animator darbeAnimation;

	void Start(){

		Transform inGameUITransform = GameObject.Find("/Canvas/InGame").transform;
		healthText = inGameUITransform.Find("Saglik").GetComponent<Text>();
		darbeAnimation = inGameUITransform.Find("Darbe").GetComponent<Animator>();

		health = GetComponent<Health>();
		health.onHit.AddListener(() => { //darbe akinan noktada "Darbe" animasyonunu cagir
			darbeAnimation.SetTrigger("Darbe");
		UpdateHealthText();
    });

		UpdateHealthText();
	}

 	void UpdateHealthText(){ //her darbede saglik durumunu guncelle
		healthText.text = "Saglik: " + health.val;
		darbeAnimation.SetTrigger("Darbe");
	}

}
