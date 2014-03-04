using UnityEngine;
using System.Collections;

public class spawnEnemy : MonoBehaviour {

	public GameObject TargetEnemy;

	
	public int TimeSpawn = 10;
	public int NumberSpawn = 3;
	
	
	void Awake () {
		
	}
	
	// Use this for initialization
	void Start () {
		
		InvokeRepeating("SpawnMulti",1,TimeSpawn);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void SpawnMulti() {
		
		for (int i=0; i<NumberSpawn; i++) {
			Spawn();
		}
	}
	
	void Spawn () {
		
		Vector3 _pos = new Vector3(Random.Range(-7f,7f),Random.Range(-4f,4f),0f);
		Instantiate(TargetEnemy,_pos,Quaternion.identity);
	}
}
