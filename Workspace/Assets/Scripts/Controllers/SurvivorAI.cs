using UnityEngine;
using System.Collections;

// originally used when i thought zombies could see in alcoves
public class SurvivorAI : MonoBehaviour 
{
	public float TimeOut = 300f;

	public Transform[] Waypoints; // Ordered set of places to go, which includes the goal at the very end
	public Transform[] SafeSpots; // all alcoves, goal and start point

	private int nextWaypoint;
	private NavMeshAgent Nav;
	private Transform Zombies;
	private Initializer blackboard;

	private const float SAFE_DISTANCE = 20f;

	// Use this for initialization
	void Start () 
	{
		nextWaypoint = 0;
		Nav = gameObject.GetComponent<NavMeshAgent> ();
		Zombies = GameObject.Find ("Zombies").transform;
		blackboard = GameObject.Find ("Level").GetComponent<Initializer> ();
		Nav.speed = blackboard.v * 1.5f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( TimeOut > 0 && ! blackboard.SurvivorSpotted )
		{
			if(nextWaypoint < Waypoints.Length)
			{
				if( InDanger() )
				{
					Nav.Stop();
				}
				else
				{
					if(Waypoints[nextWaypoint] == null ) nextWaypoint++;
					Nav.SetDestination(Waypoints[nextWaypoint].position);

					if(DestinationReached())
					{
						nextWaypoint++;
					}
				}
				TimeOut -= Time.deltaTime;
			}
		}
		else Nav.Stop();
	}

	private bool DestinationReached()
	{
		Vector3 pos = gameObject.transform.position;
		Vector3 dest = Nav.destination;

		if( pos.x == dest.x && pos.z == dest.z )
			return true;

		return false;
	}

	public bool CanSeeZombie(Transform Z)
	{
		Classic c = Z.GetComponent<Classic> ();
		Shambler s = Z.GetComponent<Shambler> ();
		Modern m = Z.GetComponent<Modern> ();
		CellPhone cp = Z.GetComponent<CellPhone> ();

		if( c != null )
		{
			return c.seenBySurvivor;
		}
		if( s != null )
		{
			return s.seenBySurvivor;
		}
		if( m != null )
		{
			return m.seenBySurvivor;
		}
		if( cp != null )
		{
			return cp.seenBySurvivor;
		}
		return false;
	}

	public int GetTrackNumber(Transform Z)
	{
		Classic c = Z.GetComponent<Classic> ();
		Shambler s = Z.GetComponent<Shambler> ();
		Modern m = Z.GetComponent<Modern> ();
		CellPhone cp = Z.GetComponent<CellPhone> ();
		
		if( c != null )
		{
			return c.TrackNumber;
		}
		if( s != null )
		{
			return s.TrackNumber;
		}
		if( m != null )
		{
			return m.TrackNumber;
		}
		if( cp != null )
		{
			return cp.TrackNumber;
		}
		return -2112;
	}

	// returns true if the survivor was successful
	public bool CheckSuccess()
	{
		Transform goal = Waypoints [Waypoints.Length - 1];
		Vector3 pos = gameObject.transform.position;
		if( pos.x == goal.position.x && pos.z == goal.position.z && nextWaypoint == Waypoints.Length)
		{
			return true;
		}
		return false;
	}

	// returns the distance to the closest zombie
	// returns a negative number if none are spotted
	private bool InDanger()
	{
		// check if zombie is in line of sight
		for( int i = 0; i < Zombies.childCount; i++ )
		{
			Vector3 pos = gameObject.transform.position;
			Transform Z = Zombies.GetChild(i);
			if( CanSeeZombie(Z) )
			{
				if( Incoming(Z) && !InAdjacentLanes(Z) )
					if( Vector3.Distance( Z.position, pos) < SAFE_DISTANCE)
						return true;
			}
		}
		return false;
	}

	// TODO: return true if the survivor is in the same lane or an adjacent lane to the zombie
	private bool InAdjacentLanes(Transform z)
	{
		Vector3 origin = gameObject.transform.position;
		Vector3 direction = gameObject.transform.up * -1;
		RaycastHit hit;

		if(Physics.Raycast (origin, direction, out hit, 10f))
		{
			string name = hit.transform.name;
			int survivorTrackNumber;
			int zombieTrackNumber = GetTrackNumber(z);

			// figure out where the survivor is
			if( name.Equals("Main Floor"))
				survivorTrackNumber = -1;
			else if( name.Equals("Outer Track"))
				survivorTrackNumber = 0;
			else if( name.Equals("Middle Track"))
				survivorTrackNumber = 1;
			else if( name.Equals("Inner Track"))
				survivorTrackNumber = 2;
			else if( name.Equals("Center Area"))
				survivorTrackNumber = 3;
			else if( name.Equals("Alcoves"))
				survivorTrackNumber = 4;
			else
			{
				Debug.Log(name);
				return false;
			}


			Debug.Log("Survivor: " + survivorTrackNumber + " Zombie: " + zombieTrackNumber);
			return (survivorTrackNumber == zombieTrackNumber + 1 
			        || survivorTrackNumber == zombieTrackNumber - 1
			        || survivorTrackNumber == zombieTrackNumber);
		}
		Debug.Log ("hi");
		return true;
	}

	// checks if a zombie is coming towards the survivor
	private bool Incoming(Transform z)
	{
		Vector3 pos = gameObject.transform.position;
		Vector3 zVel = z.GetComponent<NavMeshAgent>().velocity;
		Vector3 zPos = z.position;
		Vector3 zNextPos = zPos + zVel;

		if( Vector3.Distance( pos, zPos ) >= Vector3.Distance( pos, zNextPos ))
			return true;

		return false;
	}

	private void GoToClosestSafeSpot()
	{
		Vector3 position = gameObject.transform.position;

		// find the closest safe point and go there
		Vector3 closestSafeSpot = Vector3.one;
		float safeSpotDistance = 999999.9f;

		for(int i = 0; i < SafeSpots.Length; i++)
		{
			float dist = Vector3.Distance( position, SafeSpots[i].position);
			if( dist  < safeSpotDistance )
			{
				closestSafeSpot = SafeSpots[i].position;
				safeSpotDistance = dist;
			}
		}
		Nav.SetDestination(closestSafeSpot);
	}
}
