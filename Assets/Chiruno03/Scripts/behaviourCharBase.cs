using UnityEngine;
using System.Collections;

public class behaviourCharBase : MonoBehaviour {

	public float Speed				= 1.0f;
	public float SpeedITweenMove	= 1f;

	public float RandomRangeInitPos	= 10f;

	public float RandomRangeIdleMoveOffset	= 1f;
	public float RandomRangeWaitIdle = 10f;//sec

	public float WaitMessage	= 10f;//sec

	public GameObject ScriptsGame;
	public GameObject TargetTextMessage;
	public GameObject TargetTextID;
	public Camera TargetCamera;

	protected string m_message	= "";
	protected string m_id		= "";

	protected float timer_idle;
	protected float wait_idle;

	protected float timer_message;

//	protected string server_id;

	
	protected virtual void Awake () {

		if (true) {
//		if (Debug.isDebugBuild) {
			ScriptsGame		= GameObject.Find("ScriptsGame");
			TargetCamera	= GameObject.Find("Main Camera").camera;
			TargetTextMessage	= this.transform.FindChild("text_message").gameObject;
			TargetTextID	= this.transform.FindChild("text_id").gameObject;
		}
	}
	
	// Use this for initialization
	protected virtual void Start () {
		
		m_message	= "";
		m_id		= "";
		
		/*
		// move by iTween at setting first
		Vector3[] movePath = new Vector3[5];
		for (int i=0;i<4;i++) {
			movePath[i].Set(Random.Range(-5f,5f),Random.Range(0f,10f),0f);
		}
		movePath[4].Set(0f,0f,0f);
		iTween.MoveTo(gameObject,iTween.Hash("path",movePath,"time",4,"easetype",iTween.EaseType.easeOutSine));
		*/

		clearMessage();

		//DUMMY
//		InitRandom("offline...");
	}
	
	protected virtual void FixedUpdate () {
		
		/*
		// move by cursor-key press
		Vector3 d	= new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"),0);
		d.Normalize();
		d	= d.normalized * SPEED * Time.deltaTime;
		transform.position	+= d;
		*/
	}

	// Update is called once per frame
	protected virtual void Update () {

		/*
		// move direct on mouse click
		if (Input.GetMouseButtonDown(0)) {
			Vector3 pos_mouse	= camera.ScreenToWorldPoint(Input.mousePosition);
			pos_mouse.z	= 0;
			transform.position	= pos_mouse;
		}
		*/

		if (Time.time - timer_idle > wait_idle) {
			MoveOffsetRandom(RandomRangeIdleMoveOffset);
		}

		if (Time.time - timer_message > WaitMessage) {
			clearMessage();
		}
	}

	protected void clearMessage () {
		TargetTextMessage.guiText.text	= "";
	}
	
	protected virtual void updateMessage () {
		
		// update text-chat
		if (TargetTextMessage) {
			if (ScriptsGame) {
				ScriptsGame.GetComponent<server>().SendMessage_Chat(m_message);
			}

			TargetTextMessage.guiText.text = m_message;
			m_message	= "";
			timer_message	= Time.time;
		}
	}

	public void Init (string id,Vector2 pos) {

		SetID(id);
		SetPos(pos);
	}

	public void InitRandom (string id) {

		SetID(id);
		SetPosRandom(RandomRangeInitPos);
	}

	public void SetID (string id) {

		m_id	= id;
		TargetTextID.guiText.text	= m_id;
	}

	public void SetPosRandom (float random_range) {

		Vector2 pos	= new Vector2(Random.Range(-random_range/2f,random_range/2f),
		                          Random.Range(-random_range/2f,random_range/2f));
		SetPos(pos);
	}

	public void SetPos (Vector2 pos) {

		timer_idle	= Time.time;
		wait_idle	= Random.Range(0f,RandomRangeWaitIdle);

		Vector3 pos_target	= new Vector3(pos.x,pos.y,0f);
		pos_target.z	= 0;

		this.transform.position	= pos_target;
	}

	public void MovePos (Vector2 pos) {

		timer_idle	= Time.time;
		wait_idle	= Random.Range(0f,RandomRangeWaitIdle);

		Vector3 pos_target	= new Vector3(pos.x,pos.y,0f);
		pos_target.z	= 0;
		Vector3[] path = new Vector3[2];
		path[0] = transform.position;
		path[1] = pos_target;
		iTween.Init(this.gameObject);
		iTween.MoveTo(this.gameObject,
		              iTween.Hash("path",path,"time",SpeedITweenMove,"oncomplete","complete","oncompletetarget",this.gameObject,"easetype",iTween.EaseType.easeOutSine));
		
		if (path[0].x - path[1].x >= 0) {
			transform.localScale = new Vector3(1,-1,1);
		} else {
			transform.localScale = new Vector3(-1,-1,1);
		}
	}

	public void MovePosRandom (float random_range) {

		Vector2 pos	= new Vector2(Random.Range(-random_range/2f,random_range/2f),
		                          Random.Range(-random_range/2f,random_range/2f));
		MovePos(pos);
	}
	
	public void MoveOffset (Vector2 offset) {
		
		Vector2 target	= new Vector2(this.transform.position.x + offset.x,
		                             this.transform.position.y + offset.y);
		MovePos(target);
	}
	
	public void MoveOffsetRandom (float random_range) {
		
		Vector2 offset	= new Vector2(Random.Range(-random_range/2f,random_range/2f),
		                             Random.Range(-random_range/2f,random_range/2f));
		MoveOffset(offset);
	}
}
