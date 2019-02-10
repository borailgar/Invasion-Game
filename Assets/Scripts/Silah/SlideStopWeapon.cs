using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideStopWeapon : WeaponBase{

	//son mermide silah durumu 
	public override void PlayFiredAnimation ()
	{
		if (bulletsInClip > 1) {
			animator.CrossFadeInFixedTime ("Fire", 0.1f);
		} else {
			animator.CrossFadeInFixedTime ("FireLast", 0.1f);
		}

	}
}
