using System;
using System.Collections;
using System.Collections.Generic;
using NativeWebSocket;
using UnityEngine;

public class SocketManager : MonoBehaviour {
	public delegate void CallbackMessage (string message);

	public CallbackMessage callbackMessage;
	public string ipAddress = "ws://localhost/ws";
	private int countMessage = 0;
	WebSocket ws;
	void Awake () {
		callbackMessage = defaultReceiveMessage;
	}
	// Use this for initialization	
	async void Start () {
		ws = new WebSocket (ipAddress);
		ws.OnOpen += this.handleOpen;
		ws.OnMessage += this.handleMessage;
		ws.OnError += this.handleError;
		ws.OnClose += this.handleClose;

		InvokeRepeating ("ping", 0.0f, 10.0f);
		await ws.Connect ();

	}

	void handleOpen () {
		Debug.Log ("socket open");
	}

	void handleMessage (byte[] data) {
		string revMessage = System.Text.Encoding.UTF8.GetString (data);
		callbackMessage (revMessage);
		countMessage++;
	}

	void handleClose (WebSocketCloseCode closeCode) {
		callbackMessage ("Disconnected");
		Debug.Log ("socket close: " + closeCode);
	}

	void handleError (string error) {
		callbackMessage ("Connect Error");
		Debug.Log ("socket error");
	}

	public async void sendWSMessage (string message) {
		if (ws != null && ws.State == WebSocketState.Open) {
			ws.SendText (message);
		}
	}

	public async void sendBinary (byte[] data) {
		if (ws != null && ws.State == WebSocketState.Open) {
			ws.Send (data);
		}
	}

	async void ping () {
		this.sendBinary (new byte[] { 1, 2, 3 });
	}

	private void defaultReceiveMessage (string message) {
		Debug.Log ("callback is default!");
	}

	private async void OnApplicationQuit () {
		await ws.Close ();
	}
}