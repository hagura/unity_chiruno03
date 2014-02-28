using UnityEngine;
using System.Collections;

public class behaviourCharSelf : behaviourCharBase {

	// params for double-click
	bool one_click	= false;
//	bool timer_running;
	float timer_for_double_click;
	float delay	= 0.2f;


	string sha256 (string planeStr, string key) {
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] planeBytes = ue.GetBytes(planeStr);
		byte[] keyBytes = ue.GetBytes(key);
		
		System.Security.Cryptography.HMACSHA256 sha256 = new System.Security.Cryptography.HMACSHA256(keyBytes);
		byte[] hashBytes = sha256.ComputeHash(planeBytes);
		string hashStr = "";
		foreach(byte b in hashBytes) {
			hashStr += string.Format("{0,0:x2}", b);
		}
		return hashStr;
	}

	protected override void Start () {
		base.Start();

		//DUMMY
		InitRandom("self...");
	}

	protected override void Update () {
		base.Update ();

		// move iTween on mouse double-click
		if (Input.GetMouseButtonDown(0)) {
			if (!one_click) {
				one_click	= true;
				timer_for_double_click	= Time.time;
			} else {
				one_click	= false;
				// do double click

				timer_idle	= Time.time;

				Vector3 pos_mouse	= TargetCamera.ScreenToWorldPoint(Input.mousePosition);
				pos_mouse.z	= 0;
				Vector3[] path = new Vector3[2];
				path[0] = transform.position;
				path[1] = pos_mouse;
				iTween.Init(this.gameObject);
				iTween.MoveTo(this.gameObject,
				              iTween.Hash("path",path,"time",SpeedITweenMove,"oncomplete","complete","oncompletetarget",this.gameObject,"easetype",iTween.EaseType.easeOutSine));
				
				if (path[0].x - path[1].x >= 0) {
					transform.localScale = new Vector3(1,-1,1);
				} else {
					transform.localScale = new Vector3(-1,-1,1);
				}

				if (ScriptsGame) {
					ScriptsGame.GetComponent<server>().SendMessage_Move(m_id,pos_mouse);
				}
			}
		}
		if (one_click) {
			if ((Time.time - timer_for_double_click) > delay) {
				one_click	= false;
			}
		}
	}

	protected void complete () {
		
		iTween.Stop(this.gameObject, "move");
	}
	
	protected virtual void OnGUI () {
		
		m_message	= GUI.TextField(new Rect(10, 10, Screen.width - 100, 20), m_message, 64);
		bool isSend	= GUI.Button(new Rect(Screen.width - 80, 10, 60, 20), "send");
		
		if (isSend) {
			updateMessage();
		}
	}

}
