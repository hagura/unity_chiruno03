using UnityEngine;
using System.Collections;

public class game : MonoBehaviour {

	public class PlayerInfo {

		public string player_id;
		public GameObject player_object;

		public PlayerInfo (string player_id,GameObject player_object) {

			this.player_id	= player_id;
			this.player_object	= player_object;
		}
	}

	PlayerInfo m_player_self = null;
	ArrayList m_list_player = null;


	void Awake () {

	}

	// Use this for initialization
	void Start () {
	
		m_list_player	= new ArrayList();

		//DUMMY
		// create player_self
//		mp_player_self	= new PlayerInfo(m_player_id_self,null);

		/*
		//DUMMY spawn charOther
		for (int i=0;i<10;i++) {
			GetComponent<spawnChar>().SpawnRandom(10f);
		}
		*/
	}
	
	// Update is called once per frame
	void Update () {

	
	}


	public void InPlayer (string player_id) {

		// check exist
		if (true) {
			m_list_player.Add(player_id);
		}
	}

	public void OutPlayer (string player_id) {

		// check exist
		if (true) {
		}
	}


}
