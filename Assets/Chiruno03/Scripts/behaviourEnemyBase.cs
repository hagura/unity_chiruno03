using UnityEngine;
using System.Collections;

public class behaviourEnemyBase : MonoBehaviour {

	// reference
	public GameObject TragetGame;

	// define
	const int WAIT_CHANGE_MODE	= 100;
	const int WAIT_DESTROY		= 20;
	public int ScoreEnemy		= 100;
	public int StrongDefault	= 3;
//	public int NUMBER_RUBBLE	= 16;
//	public int POWER_MOVE		= 1;

	enum MODE {
		MODE_IDLE,
		MODE_WALK,
		MODE_DESTROY,
		
		MODE_MAX
	}

	// params
	private int m_strong;
	private int m_waitChangeMode;
	protected int m_waitDestroy;

	private MODE m_mode;


	void Awake () {

		TragetGame	= GameObject.Find("ScriptsGame");
	}

	// Use this for initialization
	void Start () {
	
		m_strong		= StrongDefault;
		m_mode			= MODE.MODE_WALK;
		
		m_waitChangeMode	= WAIT_CHANGE_MODE;
		m_waitDestroy		= WAIT_DESTROY;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (m_waitChangeMode > 0) {
			m_waitChangeMode--;
		}

		if (m_waitChangeMode <= 0) {
			m_waitChangeMode	= WAIT_CHANGE_MODE;
			int _mode			= Random.Range(0,2);
			m_mode				= (MODE)_mode;
		}
		
		switch (m_mode) {
		case MODE.MODE_IDLE:
			break;
		case MODE.MODE_WALK:
//			Vector3 _direction		= (player.transform.position - transform.position).normalized;
//			this.rigidbody.velocity	= _direction * POWER_MOVE;
			break;
		case MODE.MODE_DESTROY:
			if (m_waitDestroy > 0) {
				m_waitDestroy--;
			}
			
			if (m_waitDestroy <= 0) {
//				spawnerRubble.GetComponent<spawnRubble>().SpawnRubbleMulti(NUMBER_RUBBLE,transform.position);
//				scene.GetComponent<CGame>().AddScore(SCORE_ENEMY);
				
				Destroy(gameObject);
			}
			break;
		}
	}

	void OnCollisionEnter (Collision _col) {
		
		if (_col.gameObject.tag == "bulletSelf") {
			m_strong--;
			if (m_strong <= 0) {
				m_mode			= MODE.MODE_DESTROY;
				m_waitDestroy	= WAIT_DESTROY;
			}
		}
		else if (_col.gameObject.tag == "bulletOther") {
			m_strong--;
			if (m_strong <= 0) {
				m_mode			= MODE.MODE_DESTROY;
				m_waitDestroy	= WAIT_DESTROY;
			}
		}
	}

}
