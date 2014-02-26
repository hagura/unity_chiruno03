using UnityEngine;
using System.Collections;

public class behaviourCharText : MonoBehaviour {

	public Transform target;

	// Use this for initialization
	void Start () {
	
		guiText.pixelOffset	= new Vector2(16f,0f);//FIX
	}

	void FixedUpdate () {

	}

	// Update is called once per frame
	void Update () {

	}

	void LateUpdate () {

		Vector3 posScreen	= Camera.main.WorldToScreenPoint(target.position);
		posScreen.x /= Screen.width;
		posScreen.y /= Screen.height;
		transform.position = posScreen;
		guiText.enabled	= (posScreen.z >= 0);
	}

}
