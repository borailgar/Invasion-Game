using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class WeaponManager : MonoBehaviour{

public int selectedWeapon = 0;

void Start(){
	SelectWeapon();
 }

 void Update(){

	 int oncekiSelectedWeapon = selectedWeapon;

	 if(Input.GetAxis("Mouse ScrollWheel") > 0f){
		 if(selectedWeapon >= transform.childCount - 1){
			 selectedWeapon = 0; //eger mouse wheela gore deger ayni ise silahi degistirme
		 }
		 else
		 	selectedWeapon++;
	 }
	 if(Input.GetAxis("Mouse ScrollWheel") < 0f){
		 
		 if(selectedWeapon <= 0)
		 	selectedWeapon = transform.childCount -1;
		else
			selectedWeapon--;
	 }
	 
	 if(oncekiSelectedWeapon != selectedWeapon){
		 SelectWeapon(); 
	 }

 }
	void SelectWeapon(){

		int i = 0;
		foreach(Transform weapon in transform){
			
			if(i==selectedWeapon){
				weapon.gameObject.SetActive(true); //pistol(Police9mm) varsayilan silah olarak atandi.  
			}
			else
				weapon.gameObject.SetActive(false);

			i++;

		}
	}


}








/*


 
public enum WeaponA{
	Police9mm,
	SMGun,
}

public class WeaponManager : MonoBehaviour {

	public static WeaponManager instance;
	//public Weapon currentWeapon = Weapon.Police9mm;
	//public WeaponA currentWeapon = WeaponA.Police9mm;
 	private WeaponA[] weapons = {WeaponA.Police9mm, WeaponA.SMGun};
	private int currentWeaponIndex = 0; 
	void Awake(){
		if(instance == null){
			instance = null;
		}
	}

void Start(){
	SwitchToCurrentWeapon();
}
	
	void Update(){
		CheckWeaponSwitch();
	}

	void SwitchToCurrentWeapon(){
		for(int i = 0; i<transform.childCount; i++){
			transform.GetChild(i).gameObject.SetActive(false);
		}
	
		transform.Find(weapons[currentWeaponIndex].ToString()).gameObject.SetActive(true);

	}
	void CheckWeaponSwitch(){
		if(Input.GetKeyDown(KeyCode.Q)){
			SwitchWeapon();
		}
		
	}	

	void SwitchWeapon(){
		if(currentWeaponIndex == weapons.Length -1 ){
			currentWeaponIndex--;
			SwitchToCurrentWeapon();
		}
		else if(currentWeaponIndex == 0){
			currentWeaponIndex++;
			//bla
			SwitchToCurrentWeapon();

		}

	}
}
*/

