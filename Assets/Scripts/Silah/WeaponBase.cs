using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum FireMode{
	SemiAuto,
	FullAuto
}

public class WeaponBase : MonoBehaviour {

	protected Animator animator;
	protected bool isReloading = false; 

	//Zombileri oldurme karsiliginda x10 odul
	protected Odul odulSistemi;

	public GameObject sparklolPrefab;
	protected AudioSource audioSource;
	[Header("Sound References")] 
	protected bool fireLock = false;
	protected bool canShoot = false;

	[Header("Object References")]
	public ParticleSystem muzzle;
	public Transform shootPoint;

	[Header("UI References")]
	public Text SilahIsmi;
	public Text MermiSayisi;


	public AudioClip fireSound;
	public AudioClip fireSound_2;
	[Header("Weapon Attributes")]
	public FireMode fireMode = FireMode.FullAuto;

	public int clipSize = 12;

	public float damage = 20f;
	public float fireRate = 1.0f;
	public int bulletsInClip; // toplam mermi
	public int bulletKalan;
	public int maxMermi = 100;

	public AudioClip magOutSound;
	public AudioClip drawPistolSound;
	public AudioClip magInSound;
	public AudioClip boltSound;

	public float spread = 0.1f;




	void Start(){

		bulletsInClip = clipSize;
		bulletKalan = maxMermi;

		GameObject player = GameObject.Find("Player");
		animator = GetComponent<Animator> ();
		audioSource = GetComponent<AudioSource> (); 	
		odulSistemi = player.GetComponent<Odul>();

	 Invoke ("EnableWeapon", 1f); // eger mermi durumu iyiyse ates et!
	 UpdateTExt();
	}

	public void UpdateTExt(){
		SilahIsmi.text = GetWeaponName();
		MermiSayisi.text = "Mermi: " + bulletsInClip + " / " + bulletKalan;
	}
	string GetWeaponName(){
		string weaponName = "";

	//	if(this is SlideStopWeapon){
	//		weaponName = "Police 9mm";
	//	}
		if(this is UMP45Gun){
			weaponName = "UMP45";
		}
		else if(this is Stov){
			weaponName = "Stov Rifle";
		}
		else if(this is Shotgun){
			weaponName = "Shotgun!";
		}

		return weaponName;
	}

	void EnableWeapon(){
		canShoot = true;
	}

	void Update(){

		if (fireMode == FireMode.FullAuto && Input.GetButton ("Fire1")) {
			CheckFire ();
		}

		else if(fireMode == FireMode.SemiAuto && Input.GetButtonDown("Fire1")){
			CheckFire ();
		}

		if (Input.GetButtonDown ("Reload")) {
			CheckInReload ();
		}
	}

//silah ates etme durumlari
	void CheckFire(){
		if (!canShoot)
			return;
		if (isReloading) {
			return;
		}
		if (fireLock)
			return;
		
		if (bulletsInClip > 0) {
			Fire ();
		} else {
			DryFire ();
		}
	}

	void Fire(){
		audioSource.PlayOneShot (fireSound); 
		fireLock = true;
		DetectHit();

		muzzle.Stop ();
		muzzle.Play ();

		PlayFiredAnimation ();	

	    bulletsInClip--;
		UpdateTExt(); //mermi sayisini guncelle
		StartCoroutine (ResetFireLock ());	
	}

	//TODO : Raycast icin kullan!!
	//TODO: Z axisi surekli kontrol et, local degil "global" pos.
	void DetectHit(){
		RaycastHit hit; 
		if(Physics.Raycast(shootPoint.position, CalculateSpread(spread, shootPoint), out hit)){
			//print("Hit : " + hit.transform.name);
			if(hit.transform.CompareTag("Enemy")){
				Health target_health = hit.transform.GetComponent<Health>();
				ZombieFollow enemy = hit.transform.GetComponent<ZombieFollow>();
				if(target_health == null){
					throw new System.Exception("Enemy Compenenti bulunamadi");
				}
				else if(enemy == null){
					throw new System.Exception("Enemy Componenti bulunamadi");
				}
				else{
					target_health.TakeDamage(damage);
					// vurus noktasindan kan efekti cikar
					enemy.CreateBloodEffect(hit.point, hit.transform.rotation); 

					if(target_health.val <= 0){ //zombi saglik durumu
						KillZombiOdul killOdul = hit.transform.GetComponent<KillZombiOdul>();

						if(killOdul == null){
							throw new System.Exception("Odul sistemi zombiler icin bulunamadi!");
						}

						odulSistemi.odulGet += killOdul.odul_miktar;
					}
				}
			}
			else{
				GameObject spark = Instantiate(sparklolPrefab, hit.point, hit.transform.rotation);
				Destroy(spark, 1);
			}
		}
	}
	 
	Vector3 CalculateSpread(float spread, Transform shootPoint){
		return Vector3.Lerp(shootPoint.TransformDirection(Vector3.forward * 100), Random.onUnitSphere, spread);
	}


	public virtual void PlayFiredAnimation(){
		//isReloading = true;
		animator.CrossFadeInFixedTime ("Fire", 0.1f);
	}

	IEnumerator ResetFireLock(){
		yield return new WaitForSeconds (fireRate);
		fireLock = false;
	}

	void DryFire(){ //mermisiz ates etme sesi
		audioSource.PlayOneShot (fireSound_2); 
		fireLock = true;	

		StartCoroutine (ResetFireLock ());	
	}

	void CheckInReload(){

		if (bulletKalan > 0 && (bulletsInClip < clipSize)) {
			print ("SARJOR!!!"); //TODO: SIL
			Reload(); 
		} 
	}

	void Reload(){
		if (isReloading) { //isReloading = false
			return;
		}
		isReloading = true;
		animator.CrossFadeInFixedTime ("Reload", 0.1f);
	}
	protected virtual void ReloadAmmo(){
		// kalan mermileri clipSize'a gore olustur.
		int bulletYukle = clipSize - bulletsInClip;
		//eger kalan mermi yuklenen mermiden azsa yuklenen mermiyi cikar
		int bulletStop = (bulletKalan >= bulletYukle) ? bulletYukle : bulletKalan; 

		bulletKalan -= bulletStop;
		bulletsInClip += bulletStop;
		
		Debug.Log("Police");
		UpdateTExt(); 
	}

	public int countAnim = 0;
//ses efektleri
	public void OnDraw(){
		audioSource.PlayOneShot (drawPistolSound);
		Debug.Log("PoliceDraw");

	}

	public void OnMagOut(){
		isReloading = false;
		audioSource.PlayOneShot (magOutSound);
		countAnim++;

		Debug.Log(countAnim);

	}
	public void OnMagIn(){
		ReloadAmmo ();
		audioSource.PlayOneShot (magInSound);
	}

	public void OnBoltForwarded(){
		audioSource.PlayOneShot (boltSound);
		Invoke ("ResetIsReloading", 1f);
	}

	void ResetIsReloading(){
		isReloading = false;
	}
}
