using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
  	public Transform target;

	void LateUpdate () {
			if (target.position.y > transform.position.y)
		{
			Vector3 newPos = new Vector3(transform.position.x, target.position.y, transform.position.z);
			transform.position = newPos;
		}
		// nếu nhân vật rơi xuống dưới quá xa (điều kiến chênh lệnh dưới 6 đơn vị) thì sẽ game over
		
		else if( transform.position.y-target.position.y  > 5.53f){
			GamePlayManager.instance.GameOver();
		}	
	}
}
