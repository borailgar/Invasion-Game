using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {

	public float val = 100f;
	public Health parentRef;
	public float headShoot = 1.0f; 
	[HideInInspector]public UnityEvent onHit; //darbe alinan nokta
	public void TakeDamage(float damage){

		damage *= headShoot;

		if(parentRef != null){ //TODO : farkli silahlar icin farkli script olustururken kullan
			parentRef.TakeDamage(damage);
			return;
		}

		onHit.Invoke();
		val -= damage;

		if(val < 0){
			val = 0;
		}
	}
	
}
