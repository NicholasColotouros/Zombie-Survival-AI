using UnityEngine;
using System.Collections;

// used to test the game by allowing a human to play as the survivor
public class SurvivorController : MonoBehaviour 
{
	private float MovementSpeed;
	public Transform[] Collectables;
	public Transform goal;

	public bool EnableHumanInput; // for debug purposes, false disables controls

	private int NextCollectable = 0;
	private NavMeshAgent Nav;

	// Controls -- debugging purposes only
	private KeyCode UP 		= KeyCode.UpArrow;
	private KeyCode DOWN 	= KeyCode.DownArrow;
	private KeyCode LEFT	= KeyCode.LeftArrow;
	private KeyCode RIGHT 	= KeyCode.RightArrow;

	void Start()
	{
		MovementSpeed = GameObject.Find ("Level").GetComponent<Initializer> ().v * 1.5f;
		Nav = gameObject.transform.GetComponent<NavMeshAgent> ();
		Nav.speed = MovementSpeed;

		if(!EnableHumanInput) Nav.SetDestination( Collectables[0].position );
	}
	// Update is called once per frame
	void Update () 
	{
		if( EnableHumanInput )
			GetInput();
		else
			AIMove();

	}

	// this version isn't much of a survivor...
	// just goes to each waypoint
	void AIMove()
	{
		Vector3 pos = gameObject.transform.position;
		if( pos.x == Nav.destination.x && pos.z == Nav.destination.z)
		{
			NextCollectable++;
			if( NextCollectable < Collectables.Length )
			{
				Nav.SetDestination( Collectables[NextCollectable].position );
			}
			else Nav.SetDestination( goal.position );
		}
	}

	void GetInput()
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
