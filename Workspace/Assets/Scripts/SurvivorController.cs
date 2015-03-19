using UnityEngine;
using System.Collections;

// used to test the game by allowing a human to play as the survivor
public class SurvivorController : MonoBehaviour 
{
	public float MovementSpeed;
	public float ZoomSpeed;
	
	// Controls
	private KeyCode UP 		= KeyCode.UpArrow;
	private KeyCode DOWN 	= KeyCode.DownArrow;
	private KeyCode LEFT	= KeyCode.LeftArrow;
	private KeyCode RIGHT 	= KeyCode.RightArrow;

	// Update is called once per frame
	void Update () 
	{
		// up/down
		if( Input.GetKey(UP) )
		{
			gameObject.transform.position += Vector3.forward * Time.deltaTime * MovementSpeed;
		}
		else if( Input.GetKey(DOWN) )
		{
			gameObject.transform.position += Vector3.back * Time.deltaTime * MovementSpeed;
		}
		
		// left/right
		if( Input.GetKey(LEFT))
		{
			gameObject.transform.position += Vector3.left * Time.deltaTime * MovementSpeed;
		}
		else if( Input.GetKey(RIGHT))
		{
			gameObject.transform.position += Vector3.right * Time.deltaTime * MovementSpeed;
		}		
	}
}
