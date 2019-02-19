using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeMagazineWeapon : WeaponBase {
	[Header("Tube Magazine Weapon Sound Ref")]
	public AudioClip ammoInsertSound;
//	public AudioClip boltSound;

		protected override void Reload(){
		if (isReloading) { //isReloading = false
			return;
		}
		isReloading = true;

		if(bulletsInClip <= 0){
			animator.CrossFadeInFixedTime ("ReloadStartEmpty", 0.1f);
		}
		else{
			animator.CrossFadeInFixedTime ("ReloadStart", 0.1f);
		}
	}

	protected override void ReloadAmmo(){
		bulletKalan--;
		bulletsInClip++;

		UpdateTExt();
	}
	public void CheckNextReload(){

		isReloading = true;
		bool stopIntsering = false;
		if(bulletKalan <= 0){
			stopIntsering = true;
		}
		else if(bulletsInClip >= clipSize){
			stopIntsering = true;
		}

		if(stopIntsering){
			animator.CrossFadeInFixedTime ("ReloadEnd", 0.1f);

		}
		else{
			animator.CrossFadeInFixedTime ("ReloadInsert", 0.1f);

		}
	}

	public void OnAmmoInserted(){
		isReloading = false;
		audioSource.PlayOneShot(ammoInsertSound);
		ReloadAmmo();
	}
}
