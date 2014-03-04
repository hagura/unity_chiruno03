using UnityEngine;
using System.Collections;

public class managePlayer : MonoBehaviour {


	public class Player : Object {

		public string id;
		public GameObject player;

		public Player (string id,GameObject player) {

			this.id		= id;
			this.player	= player;
		}
	}


	spawnChar	ScriptSpawnChar;


	ArrayList	list_player	= new ArrayList();


	void Awake () {

		ScriptSpawnChar	= gameObject.GetComponent<spawnChar>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Add (string id, Vector3 pos) {

		GameObject player	= ScriptSpawnChar.Spawn(pos, Quaternion.identity);
		list_player.Add(new Player(id, player));

		player.GetComponent<behaviourCharBase>().SetID(id);
//		player.GetComponent<behaviourCharBase>().SetPos(new Vector2(pos.x,pos.y));//TEST
	}

	public void Remove (string id) {

		for (int index=0; index<list_player.Count; index++) {
			Player player	= list_player[index] as Player;
			if (player.id == id) {
				Destroy(player.player);
				list_player.Remove(player);
				break;
			}
		}
	}

	public void Chat (string id, string message) {

		for (int index=0; index<list_player.Count; index++) {
			Player player	= list_player[index] as Player;
			if (player.id == id) {
				player.player.GetComponent<behaviourCharOther>().SyncMessage(message);
				break;
			}
		}
	}

	public void Move (string id, Vector3 pos) {

		for (int index=0; index<list_player.Count; index++) {
			Player player	= list_player[index] as Player;
			if (player.id == id) {
				player.player.GetComponent<behaviourCharOther>().SyncMove(new Vector2(pos.x,pos.y));
				break;
			}
		}
	}

	public void Shoot (string id, Vector3 force) {
		
		for (int index=0; index<list_player.Count; index++) {
			Player player	= list_player[index] as Player;
			if (player.id == id) {
				player.player.GetComponent<behaviourCharOther>().SyncShoot(new Vector2(force.x,force.y));
				break;
			}
		}
	}

}
