using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public float MovementSpeed;
	public float ZoomSpeed;

	// Maximum
	public Vector2 MaxCameraBounds;
	public Vector2 MinCameraBounds;
	public float MaxCameraZoom;
	public float MinCameraZoom;

	// Controls
	private KeyCode UP 		= KeyCode.W;
	private KeyCode DOWN 	= KeyCode.S;
	private KeyCode LEFT	= KeyCode.A;
	private KeyCode RIGHT 	= KeyCode.D;

	private KeyCode ZOOM_IN = KeyCode.E;
	private KeyCode ZOOM_OUT= KeyCode.Q;

	// Update is called once per frame
	void Update () 
	{
		// up/down
		if( Input.GetKey(UP) && gameObject.transform.position.z < MaxCameraBounds.y )
		{
			gameObject.transform.position += Vector3.forward * Time.deltaTime * MovementSpeed;
		}
		else if( Input.GetKey(DOWN) && gameObject.transform.position.z > MinCameraBounds.y )
		{
			gameObject.transform.position += Vector3.back * Time.deltaTime * MovementSpeed;
		}

		// left/right
		if( Input.GetKey(LEFT) && gameObject.transform.position.x > MinCameraBounds.x )
		{
			gameObject.transform.position += Vector3.left * Time.deltaTime * MovementSpeed;
		}
		else if( Input.GetKey(RIGHT) && gameObject.transform.position.x < MaxCameraBounds.x )
		{
			gameObject.transform.position += Vector3.right * Time.deltaTime * MovementSpeed;
		}

		// zoom in/out
		Camera cam = gameObject.GetComponent<Camera> ();
		if( Input.GetKey(ZOOM_IN) && cam.orthographicSize > MinCameraZoom)
		{
			cam.orthographicSize -= Time.deltaTime * MovementSpeed;
		}
		else if( Input.GetKey(ZOOM_OUT) && cam.orthographicSize < MaxCameraZoom)
		{
			cam.orthographicSize += Time.deltaTime * MovementSpeed;
		}
	}
}
