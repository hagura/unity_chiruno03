using UnityEngine;
using System.Collections;

public class game : MonoBehaviour {

	string m_player_id_self;


	public class PlayerInfo {

		public string player_id;
		public GameObject player_object;

		public PlayerInfo (string player_id,GameObject player_object) {

			this.player_id	= player_id;
			this.player_object	= player_object;
		}

		public void Init (float px, float py) {

			this.player_object.transform.position	= new Vector3(px,py,0);
		}

		public void Direction (float sx) {

			this.player_object.transform.localScale	= new Vector3(sx,-1,1);
		}

		public void DirectionR () {

			Direction(-1);
		}

		public void DirectionL () {

			Direction(1);
		}

		public void Move (float px, float py) {

		}

	}

	ArrayList m_list_player = null;

	PlayerInfo mp_player_self = null;


	// Use this for initialization
	void Start () {
	
		m_list_player	= new ArrayList();

		// create player_self
		mp_player_self	= new PlayerInfo(m_player_id_self,null);

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
