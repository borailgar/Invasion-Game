using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashController : MonoBehaviour {

	public ParticleSystem shotgunMuzzleflash;
	public ParticleSystem UMP45Muzzleflash;
	public ParticleSystem StovRifleMuzzleflash;

	public void PlayMuzzleflash(string identifier){
		switch(identifier){
			case "Shotgun!":
				shotgunMuzzleflash.Stop();
				shotgunMuzzleflash.Play();
				break;
			case "Stov Rifle":
				StovRifleMuzzleflash.Stop();
				StovRifleMuzzleflash.Play();
				break;
			case "UMP45":
				UMP45Muzzleflash.Stop();
				UMP45Muzzleflash.Play();
				break;
		}
	}
}
