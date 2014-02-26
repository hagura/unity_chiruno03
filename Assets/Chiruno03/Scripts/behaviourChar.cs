using UnityEngine;
using System.Collections;

public class behaviourChar : MonoBehaviour {

	public float SPEED	= 1.0f;

	public float SPEED_ITWEEN = 1f;


	private string m_message = "";

	public GameObject textChar;

	public Camera camera;


	// Use this for initialization
	void Start () {
	
//		m_message	= "test";

		/*
		// move by iTween at setting first
		Vector3[] movePath = new Vector3[5];
		for (int i=0;i<4;i++) {
			movePath[i].Set(Random.Range(-5f,5f),Random.Range(0f,10f),0f);
		}
		movePath[4].Set(0f,0f,0f);
		iTween.MoveTo(gameObject,iTween.Hash("path",movePath,"time",4,"easetype",iTween.EaseType.easeOutSine));
		*/

		//




	}

	void FixedUpdate () {



		/*
		// move by cursor-key press
		Vector3 d	= new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"),0);
		d.Normalize();
		d	= d.normalized * SPEED * Time.deltaTime;
		transform.position	+= d;
		*/
	}
	
	// Update is called once per frame
	void Update () {
	
		// move by iTween on mouse click
		if (Input.GetMouseButtonDown(0)) {
			Vector3 pos_mouse	= camera.ScreenToWorldPoint(Input.mousePosition);
			pos_mouse.z	= 0;
			Vector3[] path = new Vector3[2];
			path[0] = transform.position;
			path[1] = pos_mouse;
			iTween.Init(this.gameObject);
			iTween.MoveTo(this.gameObject,iTween.Hash("path",path,"time",SPEED_ITWEEN,"oncomplete","complete","oncompletetarget",this.gameObject,"easetype",iTween.EaseType.easeOutSine));

			if (path[0].x - path[1].x >= 0) {
				transform.localScale = new Vector3(1,-1,1);
			} else {
				transform.localScale = new Vector3(-1,-1,1);
			}
		}

		/*
		// move direct by mouse click
		if (Input.GetMouseButtonDown(0)) {
			Vector3 pos_mouse	= camera.ScreenToWorldPoint(Input.mousePosition);
			pos_mouse.z	= 0;
			transform.position	= pos_mouse;
		}
		*/

		// update text-chat
		textChar.guiText.text = m_message;
	}

	public void complete () {

		iTween.Stop(this.gameObject,"move");
	}

	void OnGUI () {
		
		m_message	= GUI.TextField(new Rect(10, 10, Screen.width - 100, 20), m_message, 64);

		bool isSend	= GUI.Button(new Rect(Screen.width - 80, 10, 60, 20), "send");

		if (isSend) {

		}
	}

}

