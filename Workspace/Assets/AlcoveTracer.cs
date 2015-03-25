using UnityEngine;
using System.Collections;

// Used to check if a zombie sees inside this alcove (places on objects in alcoves) using raycasting.
// If a zombie can see in the alcove, another ray cast is shot to see if the survivor is present in the alcove.
// If the survivor is present in the alcove then he has been spotted
public class AlcoveTracer : MonoBehaviour 
{
	Initializer BlackBoard;
	Transform survivor;
	private float AlcoveDistance = 7f;


	// Use this for initialization
	void Start () 
	{
		BlackBoard = GameObject.Find ("Level").transform.GetComponent<Initializer> ();
		survivor = BlackBoard.Survivor;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( InZombieVision())
		{
			TryToSpotSurvivor();
		}
	}

	public bool InZombieVision()
	{
		RaycastHit hit;
		Vector3 origin = gameObject.transform.position;
		Vector3 direction = gameObject.transform.forward;
		LayerMask mask = 1 << LayerMask.NameToLayer ("Vision");

		if(Physics.Raycast(origin, direction, out hit, 100f, mask))
		{
			return true;
		}
		return false;
	}

	public void TryToSpotSurvivor()
	{
		RaycastHit hit;
		Vector3 origin = gameObject.transform.position;

		if(Physics.Raycast(origin, survivor.position - origin, out hit, AlcoveDistance, -1))
		{
			if( hit.transform.tag == "Player")
				BlackBoard.SurvivorSpotted = true;
		}
	}
}
