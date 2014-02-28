using UnityEngine;
using System.Collections;

public class spawnChar : MonoBehaviour {

	public GameObject CharOther;

	void Awake () {

		if (Debug.isDebugBuild) {
			CharOther	= Resources.LoadAssetAtPath("Assets/Chiruno03/Prefabs/charOther.prefab", typeof(GameObject)) as GameObject;
			if (CharOther == null) {
				Debug.Log("Load error, CharOther");
			}
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject Spawn (Vector3 pos, Quaternion rot) {

		return (GameObject)Instantiate(CharOther,pos,rot);
	}

	public GameObject SpawnRandom (float random_range) {

		Vector3 pos	= new Vector3(Random.Range(-random_range/2f,random_range/2f),
		                          Random.Range(-random_range/2f,random_range/2f),
		                          0f);

		return Spawn(pos, Quaternion.identity);
	}

}
