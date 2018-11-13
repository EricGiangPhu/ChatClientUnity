using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SocketManager : MonoBehaviour {
	public delegate void CallbackMessage(string message);

	public CallbackMessage callbackMessage;
	public string ipAddress = "ws://localhost/ws";

	WebSocket ws;


	void Awake() {
		callbackMessage = defaultReciveMessage;
	}
	// Use this for initialization
	IEnumerator Start () {
		int countMessage = 0;
		ws = new WebSocket(new System.Uri(ipAddress));
		yield return StartCoroutine(ws.Connect());

		while (true)
		{
			string revMessage = ws.RecvString();
			if(revMessage != null) {				
				callbackMessage(revMessage);
				countMessage++;
			}

			if(ws.error !=null) {				
				callbackMessage("Connect Error");
				callbackMessage(ws.error);
				Debug.Log("Error connect!");
				break;
			}

			yield return 0;
		}

		ws.Close();
	}
	
	public void sendWSMessage(string message) {
		if(ws != null) {
			ws.SendString(message);
		}
	}
	private void defaultReciveMessage(string message) {
		Debug.Log("callback is default!");
	}
}
