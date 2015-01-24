using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public GameObject targetObject;

	void LateUpdate () 
	{
		float targetObjectX = targetObject.transform.position.x;
		float targetObjectY = targetObject.transform.position.y;

		Vector3 newCameraPosition = transform.position;
		newCameraPosition.x = targetObjectX;
		newCameraPosition.y = targetObjectY;
		transform.position = newCameraPosition;
	}
}
