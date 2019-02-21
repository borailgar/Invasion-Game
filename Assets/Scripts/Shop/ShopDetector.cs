using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopDetector : MonoBehaviour {

	private WeaponDegis weaponDegis;

	private Odul odulSistemi;

	private AudioSource audioSource;

	public Transform shootPoint;
	public float detectRange = 2f;
	public Text shopText;
	public AudioClip buySound;

	public AudioClip errorSound;

	void Start(){
		audioSource = GetComponent<AudioSource>();
		weaponDegis = GetComponentInChildren<WeaponDegis>();
		odulSistemi = GetComponent<Odul>();
	}

	void Update(){
		RaycastHit hit; 

		if(Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, detectRange)){
			ShopBase shopBase = hit.transform.GetComponent<ShopBase>();
			//Debug.Log("DEGDI");

			if(shopBase == null) {
				shopText.text = "";
				return;
			}

			if(shopBase is AmmoShop){
				shopText.text = "Mermi almak icin F tusuna basin(" + shopBase.cost + "TL)" ;
			//	Debug.Log("DEGDI!!!!!!");

			}
			else if(shopBase is StovRifleShop){
				shopText.text = "Tabanca almak icin F tusuna basin(" + shopBase.cost + "TL)" ;
				//Debug.Log("DEGDI tabanca");
			}

			//shopText.text += " (" + shopBase.cost + "TL)";

			if(Input.GetKeyDown(KeyCode.F)){

				if(odulSistemi.m_odul < shopBase.cost){
					Debug.Log("Yeterli para yok!");
					audioSource.PlayOneShot(errorSound);
					return;
				}

				bool purchased = false;

				if(shopBase is AmmoShop){
					purchased = BuyAmmo();
				}
				else if(shopBase is StovRifleShop){
					purchased = BuyWeapon(Weapons.StovRifle);
				}

				if(purchased){
					audioSource.PlayOneShot(buySound);
					odulSistemi.m_odul -= shopBase.cost;

				}else{
					audioSource.PlayOneShot(errorSound);
				}
			}


		}
		else{
			
				shopText.text = "";
				return;
			
		}
	
	} 

	bool BuyAmmo(){
		WeaponBase currentWeaponBase = weaponDegis.GetCurrentWeaponObject().GetComponent<WeaponBase>();

		if(currentWeaponBase.GetCurrentAmmo() < currentWeaponBase.GetTotalAmmo()){
			currentWeaponBase.RefillAmmo();

			return true;
		}
		else{
			Debug.Log("HALI HAZIRDA MERMI DOLU!");
			return false;
		}

	//	audioSource.PlayOneShot(buySound);
	}

	bool BuyWeapon(Weapons weapon){
		//print("BUY: " + weapon);
		if(weaponDegis.hasWeapon(weapon)){
			Debug.Log("Su anda zaten " + weapon + " mevcut!");
			return false;
		}

		if(!weaponDegis.hasPrimaryWeapon()){
			weaponDegis.SetPrimaryWeapon(weapon);
		}
		else{
			weaponDegis.ReplaceCurrentWeapon(weapon);
		}

		return true;

		//audioSource.PlayOneShot(buySound);
	}
}
