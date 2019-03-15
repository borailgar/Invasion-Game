using UnityEngine;
using UnityEngine.UI;


public delegate void CallbackFn();

public class NetworkManager : Photon.PunBehaviour {

	public static NetworkManager instance;

	public const string version = "1.94";
	public Text status;

	private CallbackFn onConnected;
	private CallbackFn onConnectionFailed;
	private CallbackFn onRoomJoined;

	void Awake(){
		instance = this;
	}

	void Update(){
		status.text = PhotonNetwork.connectionStateDetailed.ToString();
	}

	public void Connect(CallbackFn onConnected, CallbackFn onConnectionFailed){
		this.onConnected = onConnected;
		this.onConnectionFailed = onConnectionFailed;
		PhotonNetwork.ConnectUsingSettings(version);
	}

	public override void OnConnectionFail(DisconnectCause cause){
		Debug.Log(cause.ToString());
		onConnectionFailed();
		onConnectionFailed = null;
	}

	public override void OnJoinedLobby(){
		onConnected();
		onConnected = null;
	}

	public void JoinOrCreateRoom(CallbackFn onRoomJoined){
		RoomOptions roomOptions = new RoomOptions(){
			IsVisible = true, MaxPlayers = 4
		};
		PhotonNetwork.JoinOrCreateRoom("TEST", roomOptions, TypedLobby.Default);

		this.onRoomJoined = onRoomJoined;
	}

	public override void OnJoinedRoom(){
		//Debug.Log("Connected");
		onRoomJoined();
		onRoomJoined = null;
	}
}
