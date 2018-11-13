using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	// Use this for initialization
	public GameObject ChatView;
	public GameObject Container;

	public SocketManager socketManager; 

	public GameObject ElementMessage;
	public InputField inputTextFild;
	void Start () {
		deActiveChatView();
		socketManager.callbackMessage+= addMessageToUI;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void activeChatView() {
		ChatView.SetActive(true);
	}

	public void deActiveChatView() {
		ChatView.SetActive(false);
	}

	public void SendMessage() {		
		socketManager.sendWSMessage(inputTextFild.text);
		inputTextFild.text = "";
	}

	public void addMessageToUI(string message) {
		GameObject chatMessage = Instantiate(ElementMessage, Container.transform);
		chatMessage.GetComponentInChildren<Text>().text = message;
	}
}
