using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapons{

		UMP45,
		StovRifle,
		DefenderShotgun,
	}


public class WeaponDegis : MonoBehaviour {

	public static WeaponDegis instance;
	public Weapons currentOne = Weapons.UMP45;
	private int currentWeaponIndex = 0;

	/*
	
	 */
	private Weapons[] WeaponArray = {
									 Weapons.UMP45,
									 Weapons.StovRifle,
									 Weapons.DefenderShotgun}; 
	void Awake(){
		if(instance == null){
			instance = this;
		}
		else{
			Destroy(gameObject);
		}
	}
	void Start(){
		Switch();
	}
	void Update(){
		CheckWeaponSwitch();
	}

	void Switch(){
		for(int i = 0; i<transform.childCount; i++){
		transform.GetChild(i).gameObject.SetActive(false);	
		}

		GameObject currWeapon =  transform.Find(WeaponArray[currentWeaponIndex].ToString()).gameObject;
		currWeapon.SetActive(true);

		currWeapon.GetComponent<WeaponBase>().UpdateTExt();
	}

	void CheckWeaponSwitch(){
		float mouse = Input.GetAxis("Mouse ScrollWheel");

		if(mouse > 0){
			SelectPrev();
		}
		else if(mouse < 0){
			SelectNext();
		}

	}

	void SelectPrev(){
		if(currentWeaponIndex == 0){
			currentWeaponIndex = WeaponArray.Length - 1;
		}
		else{
			currentWeaponIndex--;
		}
		Switch();
	}
		void SelectNext(){
		if(currentWeaponIndex >= (WeaponArray.Length - 1)){
			currentWeaponIndex = 0;
		}
		else{
			currentWeaponIndex++;
		}
			Switch();
	}
}
