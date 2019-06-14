using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;
	public AudioClip dryFire;

	public AudioClip UMP45Fire;
	public AudioClip UMP45Draw;
	public AudioClip UMP45MagOut;
	public AudioClip UMP45MagIn;
	public AudioClip UMP45_BoltForwarded;


	public AudioClip Stov_RifleFire;
	public AudioClip Stov_RifleDraw;
	public AudioClip Stov_RifleMagOut;
	public AudioClip Stov_RifleMagIn;
	public AudioClip Stov_Rifle_BoltForwarded;

	public AudioClip ShotgunFire;
	public AudioClip ShotgunDraw;
	public AudioClip ShotgunMagOut;
	public AudioClip ShotgunMagIn;
	public AudioClip Shotgun_BoltForwarded;
	void Awake(){
		instance = this;
	}
}
