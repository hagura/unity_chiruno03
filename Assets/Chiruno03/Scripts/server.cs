using UnityEngine;
using System.Collections;
using WebSocketSharp;
using System.Collections.Generic;

using MiniJSON;

public class server : MonoBehaviour {


	GameObject	TargetChar;
	behaviourCharBase	ScriptBehaviourCharBase;


	WebSocket ws;

	string	server_id;
	Vector3 pos_self;

	ArrayList	list_message	= new ArrayList();



	/*
	void Awake () {
		Debug.Log(this.name+":Awake()");

//		Security.PrefetchSocketPolicy("192.168.1.44",8080);

		ws =  new WebSocket("ws://192.168.1.44:8080/");
		messageQueue	= Queue.Synchronized(new Queue());
		ws.OnOpen += (sender, e) => {
			Debug.Log("OnOpen");

			ws.Send("Hi, all!");
		};
		// called when websocket close
		ws.OnClose += (sender, e) => {
			Debug.Log("OnClose");
			
		};
		// called when websocket error
		ws.OnError += (sender, e) => {
			Debug.Log("OnError");
			Debug.Log(" "+sender.ToString());
			Debug.Log(" "+e.ToString());
			Debug.Log(" "+e.Message);

		};
		ws.OnMessage	+= (sender, e) => {
			Debug.Log("OnMessage");

			messageQueue.Enqueue(e);
		};

		ws.Connect();
		Debug.Log(ws.Url);
	}
	*/

	void Awake () {

		TargetChar	= GameObject.Find("charSelf");
		ScriptBehaviourCharBase	= TargetChar.GetComponent<behaviourCharBase>();

		pos_self	= TargetChar.transform.position;

		Connect();
	}

	// Use this for initialization
	void Start () {

	}
	
	void OnApplicationQuit () {
		Debug.Log(this.name+":OnApplicationQuit()");
		
		if (ws != null) {
			ws.Close();
		}
	}

	void FixedUpdate () {

		if (list_message.Count > 0) {
			string s = list_message[0] as string;
			list_message.RemoveAt(0);
			Debug.Log(s);

			{
				IDictionary messageList	= (IDictionary)Json.Deserialize(s);
				string type	= (string)messageList["type"];
				Debug.Log(type);
				
				switch (type) {
				case "init":
				{
					string id	= (string)messageList["id"];
					Debug.Log(id);
					
					Message_Init(id);
					break;
				}
				case "add":
				{
					string id	= (string)messageList["id"];
					Debug.Log(id);
					IList list_pos	= (IList)messageList["pos"];
					Debug.Log(list_pos.ToString());
					Vector3 pos	= new Vector3(0,0,0);
					float.TryParse((string)list_pos[0], out pos.x);
					float.TryParse((string)list_pos[1], out pos.y);
					float.TryParse((string)list_pos[2], out pos.z);
//					Vector3 pos	= new Vector3((float)((double)list_pos[0]),
//					                          (float)((double)list_pos[1]),
//					                          (float)((double)list_pos[2]));
//					Vector3 pos	= new Vector3((float)((double)list_pos[0]),
//					                          (float)((double)list_pos[1]),
//					                          0.0f);
					Message_Add(id,pos);
					break;
				}
				case "move":
				{
					string id	= (string)messageList["id"];
					Debug.Log(id);
					IList list_pos	= (IList)messageList["pos"];
					Debug.Log(list_pos.ToString());
					Vector3 pos	= new Vector3(0,0,0);
					float.TryParse((string)list_pos[0], out pos.x);
					float.TryParse((string)list_pos[1], out pos.y);
					float.TryParse((string)list_pos[2], out pos.z);
//					Vector3 pos	= new Vector3((float)((double)list_pos[0]),
//					                          (float)((double)list_pos[1]),
//					                          (float)((double)list_pos[2]));
//					Vector3 pos	= new Vector3((float)((double)list_pos[0]),
//					                          (float)((double)list_pos[1]),
//					                          0.0f);
					Message_Move(id,pos);
					break;
				}
				case "remove":
				{
					string id	= (string)messageList["id"];
					Debug.Log(id);
					
					Message_Remove(id);
					break;
				}
				case "chat":
				{
					string id	= (string)messageList["id"];
					string msg	= (string)messageList["message"];
					
					Message_Chat(id,msg);
					break;
				}
				}
			}



		}



	}


	/*
	// Update is called once per frame
	void Update () {

		lock(messageQueue.SyncRoot) {

			//get queue message

		}
		// do message
	
	}
	*/

	void Connect () {

		ws =  new WebSocket("ws://192.168.1.44:8080/");
		Debug.Log("Connect to " + ws.Url);

		// called when websocket connect
		ws.OnOpen += (sender, e) =>
		{
			Debug.Log("OnOpen");

			SendMessage_Init();
		};

		// called when websocket close
		ws.OnClose += (sender, e) =>
		{
			Debug.Log("OnClose");

		};

		// called when websocket error
		ws.OnError += (sender, e) =>
		{
			Debug.Log("OnError"+"/"+e.Message);

		};

		// called when websocket messages come.
		ws.OnMessage += (sender, e) =>
		{
			Debug.Log("OnMessage");

			string s = e.Data;
			Debug.Log(string.Format( "Receive {0}",s));

			list_message.Add(s);
		};


		bool isOk = Security.PrefetchSocketPolicy("192.168.1.44", 8081, 500);//TEST
		if (isOk) {
			Debug.Log("isOk");
		}

		ws.Connect();
	}

	protected void Message_Init (string id) {

		server_id	= id;
//		Debug.Log(pos_self.ToString());

		ScriptBehaviourCharBase.SetID(server_id);
		//ERROR not main thread

		SendMessage_Add(server_id, pos_self);
	}
	
	protected void Message_Add (string id, Vector3 pos) {

		if (server_id==id) {
			
		}
	}

	protected void Message_Chat (string id, string message) {
		
		if (server_id==id) {
			
		}
	}

	protected void Message_Remove (string id) {
		
		if (server_id==id) {
			
		}
	}
	
	protected void Message_Move (string id, Vector3 pos) {
		
		if (server_id==id) {
			
		}
	}

	public void SendMessage_Init () {
		Debug.Log("SendMessage_Init() ");
		
		Dictionary<string,object> jsonSrc	= new Dictionary<string, object>();
		jsonSrc.Add("type","init");
		
		if (ws != null) {
			ws.Send(Json.Serialize(jsonSrc));
		}
	}

	public void SendMessage_Chat (string message) {
		Debug.Log("SendMessageChat() " + message);

		Dictionary<string,object> jsonSrc	= new Dictionary<string, object>();
		jsonSrc.Add("type","chat");
		jsonSrc.Add("id","dummychatid");
		jsonSrc.Add("message",message);

		if (ws != null) {
			ws.Send(Json.Serialize(jsonSrc));
		}
	}

	public void SendMessage_Add (string id, Vector3 pos) {
		Debug.Log("SendMessage_Add() ");
		
		Dictionary<string,object> jsonSrc	= new Dictionary<string, object>();
		jsonSrc.Add("type","add");
		jsonSrc.Add("id",id);
		string[] list_pos = new string[3];
		list_pos[0] = pos.x.ToString();
		list_pos[1] = pos.y.ToString();
		list_pos[2] = pos.z.ToString();
		jsonSrc.Add("pos",list_pos);

		if (ws != null) {
			ws.Send(Json.Serialize(jsonSrc));
		}
	}

	public void SendMessage_Remove (string id) {
		Debug.Log("SendMessage_Remove() "+id);
		
		Dictionary<string,object> jsonSrc	= new Dictionary<string, object>();
		jsonSrc.Add("type","remove");
		jsonSrc.Add("id",id);
		
		if (ws != null) {
			ws.Send(Json.Serialize(jsonSrc));
		}

		//NOTE とありあえず、接続切断で代用出来るはず
	}

	public void SendMessage_Move (string id, Vector3 pos) {
		Debug.Log("SendMessage_Move() "+id);
		
		Dictionary<string,object> jsonSrc	= new Dictionary<string, object>();
		jsonSrc.Add("type","move");
		jsonSrc.Add("id",id);
		string[] list_pos = new string[3];
		list_pos[0] = pos.x.ToString();
		list_pos[1] = pos.y.ToString();
		list_pos[2] = pos.z.ToString();
		jsonSrc.Add("pos",list_pos);
		
		if (ws != null) {
			ws.Send(Json.Serialize(jsonSrc));
		}
	}

}
