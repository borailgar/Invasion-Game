using UnityEngine;
using UnityEngine.UI;

public class StartView : ViewBase {

	[Header("View Refs")]

	public ViewBase optionsView;
	public GameObject lobbyCam;
//	public GameObject player;

	public GameObject inGameUI;
	public GameObject mainUI;
	public Button StartButton;	
	public Button OptionsButton;
	public Button ExitButton;

	protected override void OnInit(){
		StartButton.onClick.AddListener(()=> {
			Debug.Log("Start Game");
			lobbyCam.SetActive(false);
			inGameUI.SetActive(true);

		//	player.SetActive(true);
			mainUI.SetActive(false);

			GameManager.instance.StartGame();
		});

		OptionsButton.onClick.AddListener(()=> {
		//	Debug.Log("Move to Options View");
			this.Hide();
			optionsView.Show();
		});

		ExitButton.onClick.AddListener(()=> {
			//Debug.Log("Exit Game!");
			Application.Quit();
		});
	}
	protected override void OnShow(){
		lobbyCam.SetActive(true);
		mainUI.SetActive(true);
		inGameUI.SetActive(false);
	}
}
