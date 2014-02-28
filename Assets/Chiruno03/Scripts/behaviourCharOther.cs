using UnityEngine;
using System.Collections;

public class behaviourCharOther : behaviourCharBase {


	public void SyncMessage (string message) {

		m_message	= message;
		updateMessage();
	}

	public void SyncMove (Vector2 pos) {

		MovePos(pos);
	}

}
