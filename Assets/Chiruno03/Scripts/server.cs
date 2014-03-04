using UnityEngine;
using System.Collections;
using WebSocketSharp;
using System.Collections.Generic;

using MiniJSON;

public class server : MonoBehaviour {


	GameObject	TargetChar;
	behaviourCharBase	ScriptBehaviourCharBase;
	managePlayer	ScriptManagePlayer;


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
		ScriptManagePlayer	= gameObject.GetComponent<managePlayer>();

		pos_self	= TargetChar.transform.position;

		Connect();
	}

	// Use this for initialization
	void Start () {

	}
	
	void OnApplicationQuit () {
		Debug.Log(this.name+":OnApplicationQuit()");

		SendMessage_Remove(server_id);

		if (ws != null) {
			ws.Close();
		}
	}

	void FixedUpdate () {

		if (list_message.Count > 0) {
			string s = list_message[0] as string;
			list_message.RemoveAt(0);

			IDictionary param	= (IDictionary)Json.Deserialize(s);
			string type	= (string)param["type"];
			switch (type) {
			case "init":
			{
				string id	= (string)param["id"];

//				ArrayList list_id	= new ArrayList();
//				ArrayList list_pos	= new ArrayList();
				List<string> list_id	= new List<string>();
				List<Vector3> list_pos	= new List<Vector3>();

//				Debug.Log(param["others"]);

				if (param["others"] != null) {
					IList list_other = (IList)param["others"];
					foreach (IDictionary param_other in list_other) {
						string id_other = (string)param_other["id"];
						IList list_pos_other	= (IList)param_other["pos"];
						Vector3 pos_other = new Vector3(0,0,0);
						float.TryParse((string)list_pos_other[0], out pos_other.x);
						float.TryParse((string)list_pos_other[1], out pos_other.y);
						float.TryParse((string)list_pos_other[2], out pos_other.z);

						list_id.Add(id_other);
						list_pos.Add(pos_other);
					}
				}

				Message_Init(id,list_id,list_pos);
				break;
			}
			case "add":
			{
				string id	= (string)param["id"];
				IList list_pos	= (IList)param["pos"];
				Vector3 pos	= new Vector3(0,0,0);
				float.TryParse((string)list_pos[0], out pos.x);
				float.TryParse((string)list_pos[1], out pos.y);
				float.TryParse((string)list_pos[2], out pos.z);
				Message_Add(id,pos);
				break;
			}
			case "move":
			{
				string id	= (string)param["id"];
				IList list_pos	= (IList)param["pos"];
				Vector3 pos	= new Vector3(0,0,0);
				float.TryParse((string)list_pos[0], out pos.x);
				float.TryParse((string)list_pos[1], out pos.y);
				float.TryParse((string)list_pos[2], out pos.z);
				Message_Move(id,pos);
				break;
			}
			case "remove":
			{
				string id	= (string)param["id"];
				Message_Remove(id);
				break;
			}
			case "chat":
			{
				string id	= (string)param["id"];
				string msg	= (string)param["message"];
				Message_Chat(id,msg);
				break;
			}
			case "shoot":
			{
				string id	= (string)param["id"];
				IList list_force	= (IList)param["force"];
				Vector3 force	= new Vector3(0,0,0);
				float.TryParse((string)list_force[0], out force.x);
				float.TryParse((string)list_force[1], out force.y);
				float.TryParse((string)list_force[2], out force.z);
				Message_Shoot(id,force);
				break;
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

	protected void Message_Init (string id, List<string> list_id = null, List<Vector3> list_pos = null) {
		Debug.Log("Init "+id);

		server_id	= id;
//		Debug.Log(pos_self.ToString());

		ScriptBehaviourCharBase.SetID(server_id);
		//ERROR not main thread

		SendMessage_Add(server_id, pos_self);

		if (list_id.Count > 0) {
			for (int index=0; index < list_id.Count; index++) {
//				string id_other = list_id[index] as string;
//				Vector3 pos_other = new Vector3( (float)list_pos[index].x,list_pos[index].y,list_pos[index].z);
				Message_Add(list_id[index], list_pos[index]);
			}
		}
	}
	
	protected void Message_Add (string id, Vector3 pos) {
		Debug.Log("Add "+id+" "+pos.ToString());

		if (server_id!=id) {
			ScriptManagePlayer.Add(id,pos);
//			SendMessage_Add(server_id, pos_self);//TEST 無限！
		} else {
		}

//		SendMessage_Add(server_id, pos_self);//TEST 無限！
	}

	protected void Message_Chat (string id, string message) {
		Debug.Log("Chat "+id+" "+message);

		if (server_id!=id) {
			ScriptManagePlayer.Chat(id,message);
		}
	}

	protected void Message_Remove (string id) {
		Debug.Log("Remove "+id);

		if (server_id!=id) {
			ScriptManagePlayer.Remove(id);
		}
	}
	
	protected void Message_Move (string id, Vector3 pos) {
		Debug.Log("Move "+id+" "+pos.ToString());

		if (server_id!=id) {
			ScriptManagePlayer.Move(id,pos);
		}
	}

	protected void Message_Shoot (string id, Vector3 force) {
		Debug.Log("Shoot "+id+" "+force.ToString());
		
		if (server_id!=id) {
			ScriptManagePlayer.Shoot(id,force);
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
		jsonSrc.Add("id",server_id);
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

	public void SendMessage_Shoot (string id, Vector3 force) {
		Debug.Log("SendMessage_Shoot() "+id);

		Dictionary<string,object> jsonSrc	= new Dictionary<string, object>();
		jsonSrc.Add("type","shoot");
		jsonSrc.Add("id",id);
		string[] list_force = new string[3];
		list_force[0]	= force.x.ToString();
		list_force[1]	= force.y.ToString();
		list_force[2]	= force.z.ToString();
		jsonSrc.Add("force",list_force);

		if (ws != null) {
			ws.Send(Json.Serialize(jsonSrc));
		}
	}

}
