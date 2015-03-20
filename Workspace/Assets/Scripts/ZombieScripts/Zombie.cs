using UnityEngine;
using System.Collections;

public abstract class Zombie : MonoBehaviour 
{
	// the following 5 are set upon spawning
	public Transform[] Track;
	public int TrackNumber; // 0 for outer, 1 for middle, 2 for inner
	public int TrackIndex; // index in the track
	public Initializer.ZombieMovementDirection direction;

	protected Transform survivor;
	protected Initializer BlackBoard;
	protected NavMeshAgent Nav;
	protected float speed;

	protected bool SurvivorSpotted = false;
	private bool JustSpawned = true; // to prevent immediate despawn

	// Use this for initialization
	void Start () 
	{
		BlackBoard = GameObject.Find ("Level").GetComponent<Initializer> ();
		survivor = BlackBoard.Survivor;
		direction = BlackBoard.Direction;
		Nav = gameObject.GetComponent<NavMeshAgent> ();
		speed = BlackBoard.v;

		Nav.speed = speed;
		Nav.acceleration = 9999999f; // want intantaneous speed adjustment
		Nav.SetDestination(Track [TrackIndex].position);

		AdditionalSetup ();
	}

	protected abstract void AdditionalSetup();
	
	// Update is called once per frame
	void Update () 
	{
		if( ! SurvivorSpotted )
		{
			// check to make sure there is a destination
			SeenByPlayer (); // highlights the zombie if the player can see it
			ZombieMovement();
			LookForSurvivor ();

			// Check to see if arrived
			Vector3 pos = gameObject.transform.position;
			if( pos.x == Track[TrackIndex].position.x && pos.z == Track[TrackIndex].position.z)
			{
				// see if the zombie should be despawned
				int d100 = Random.Range(1,101);
				if( d100 <= BlackBoard.p && !JustSpawned )
				{
					// the zombies themselves should implement ondestroy to ensure the count is maintained
					Destroy(gameObject);
				}
				else
				{
					AssignNextWayPoint();
					Nav.SetDestination( Track[TrackIndex].position);
					JustSpawned = false;
				}
			}

		}
		else 
		{
			// chase the survivor
			Nav.SetDestination(survivor.position);
		}
	}

	public void SeenByPlayer ()
	{
		Vector3 playerPos = survivor.position;
		Vector3 guardPos = gameObject.transform.position + Vector3.up * 3;
		
		Vector3 playerDirection = playerPos - guardPos;

		RaycastHit hit;
		LayerMask mask = ~(1 << LayerMask.NameToLayer ("Zombies"));
		if(Physics.Raycast(guardPos, playerDirection, out hit, 200f, mask))
		{
			bool seen = false;
			if(hit.transform.tag == "Player")
			{
				seen = true;
			}

			Transform Base = gameObject.transform.FindChild("Base");
			if( seen )
			{
				Base.renderer.enabled = true;
			}
			else
			{
				Base.renderer.enabled = false;
			}
		}
	}

	public void LookForSurvivor ()
	{
		if( BlackBoard.SurvivorSpotted )
		{
			SurvivorSpotted = true;
		}

		else
		{
			bool spotted = gameObject.transform.FindChild("VisionRectangle").GetComponent<VisionTrigger>().SurvivorEntered;
			if( spotted )
			{
				BlackBoard.SurvivorSpotted = true;
				SurvivorSpotted = true;
			}

			// TODO: check alcoves
		}
	}

	protected abstract void ZombieMovement();

	private void AssignNextWayPoint()
	{
		if(direction == Initializer.ZombieMovementDirection.Clockwise)
			TrackIndex = (TrackIndex + 1) % Track.Length;
		else
		{
			TrackIndex--;
			if(TrackIndex < 0) TrackIndex = Track.Length - 1;
		}
	}
}