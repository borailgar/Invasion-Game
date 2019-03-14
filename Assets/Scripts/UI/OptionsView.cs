using UnityEngine;
using UnityEngine.UI;

public class OptionsView : ViewBase {

	[Header("View Refs")]

	public ViewBase startView;
	public Button BackToStartButton;
	protected override void OnInit(){
		BackToStartButton.onClick.AddListener(()=>{
			this.Hide();
			startView.Show();
		});
	}
	
}
