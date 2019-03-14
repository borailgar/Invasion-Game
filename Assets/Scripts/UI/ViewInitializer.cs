using UnityEngine;

public class ViewInitializer : MonoBehaviour {

	public ViewBase entryView;

	void Awake(){
		ViewBase[] views = GameObject.FindObjectsOfType<ViewBase>();
		for (int i = 0; i < views.Length; i++){
			ViewBase view = views[i];

			//Initialiaze view icin nesneleri aktif hale getir! (force)
			view.viewObject.SetActive(true);

			if(view == entryView){
				continue;
			}

			view.viewObject.SetActive(false);

			
		}
	}
}
