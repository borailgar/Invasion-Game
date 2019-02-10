using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class ControlHouse : MonoBehaviour{

public GameObject Bedroom2WindowObj;
public bool Bedroom2WindowScan = true;
[Range(0f,1.0f)]
public float Bedroom2Window = 0;

public GameObject Bedroom3WindowObj;
public bool Bedroom3WindowScan= true;
[Range(0f,1.0f)]
public float Bedroom3Window = 0;

public GameObject Bathroom1WindowObj;
public bool Bathroom1WindowScan = true;
[Range(0f,1.0f)]
public float Bathroom1Window = 0;

public GameObject GarageDoorObj;
public bool GarageDoorScan = true;
[Range(0f,1.0f)]
public float GarageDoor = 0;
GameObject ScanWindows(string LookFor)
{
	Animation[] ScanWindow = FindObjectsOfType <Animation>();
	for (int i=0;i<ScanWindow.Length;i++)
	{
		ScanWindow[i].Play();
		if (ScanWindow[i][LookFor]!=null)
		{
			return ScanWindow[i].gameObject;
		}
	}
	return null;
}
void SetAnimation(GameObject Obj,string Anim,float Count)
{
	if (Obj!=null)
	{
		AnimationState state = (Obj.GetComponent<Animation>() as Animation)[Anim];
		state.enabled = true;
		state.weight = 1;
		state.normalizedTime = Count;
		Obj.GetComponent<Animation>().Sample();
	}
}
public void Update()
{
	if (Bedroom2WindowScan==true)
	{
		Bedroom2WindowScan=false;
		Bedroom2WindowObj = ScanWindows("Bedroom2WindowUp");
	}
	if (Bedroom3WindowScan==true)
	{
		Bedroom3WindowScan=false;
		Bedroom3WindowObj = ScanWindows("Bedroom3WindowUp");
	}	
	if (Bathroom1WindowScan==true)
	{
		Bathroom1WindowScan=false;
		Bathroom1WindowObj = ScanWindows("Bathroom1WindowUp");
	}	
	if (GarageDoorScan==true)
	{
		GarageDoorScan=false;
		GarageDoorObj = ScanWindows("GarageDoorUp");
	}		
	SetAnimation(Bedroom2WindowObj,"Bedroom2WindowUp",Bedroom2Window);
	SetAnimation(Bathroom1WindowObj,"Bathroom1WindowUp",Bathroom1Window);
	SetAnimation(Bedroom3WindowObj,"Bedroom3WindowUp",Bedroom3Window);
	SetAnimation(GarageDoorObj,"GarageDoorUp",GarageDoor);
}
}