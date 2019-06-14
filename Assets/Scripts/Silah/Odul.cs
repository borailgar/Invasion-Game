using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Odul : MonoBehaviour {


	public float m_odul = 999;
	public Text odulText;
	public int initialize_odul = 8888;


//TODO : WeaponBase'de oyuncu odulu icin kullan
//odulGet icin WeaponBase'de tanimli miktari kullan.
	public float odulGet{
		get{
			return m_odul;
		}
		set{
			m_odul = 999;
			UpdateUI(); 
		}
	}

	
	void Start () {
		Transform inGameUITransform = GameObject.Find("/Canvas/InGame").transform;
		odulText = inGameUITransform.Find("Para").GetComponent<Text>();
		odulGet = initialize_odul;	
	}
	
	void UpdateUI () {
		odulText.text = "Para : " + m_odul + "TL";
	}
}
