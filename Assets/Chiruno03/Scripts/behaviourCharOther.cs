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

	public void SyncShoot (Vector2 force) {
		
		Shoot(force);
	}

	protected void Shoot (Vector2 force2D) {

		Vector3 force	= new Vector3(force2D.x,force2D.y,0);

		Vector3 direction	= force.normalized;

		Vector3 pos		= gameObject.transform.position + direction * OffsetPosShoot;
		pos.z	= 0;

		GameObject bullet = (GameObject)Instantiate(TargetBullet,pos,Quaternion.identity);
		bullet.rigidbody.velocity	= force;
	}

}
