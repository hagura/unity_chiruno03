using UnityEngine;
using System.Collections;

public class behaviorCharTextBase : MonoBehaviour {

	protected Transform Target;
	public Vector2 PixelOffset;

	protected virtual void Awake () {

		Target	= this.transform.parent;
	}

	// Use this for initialization
	protected virtual void Start () {
//		Debug.Log(this.name+":Start()");
		
		guiText.pixelOffset	= PixelOffset;
	}
	
	protected virtual void FixedUpdate () {
		
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
	}
	
	protected virtual void LateUpdate () {

		if (Target) {
			Vector3 posScreen	= Camera.main.WorldToScreenPoint(Target.position);
			posScreen.x /= Screen.width;
			posScreen.y /= Screen.height;
			transform.position = posScreen;
			guiText.enabled	= (posScreen.z >= 0);
		}
	}
}
