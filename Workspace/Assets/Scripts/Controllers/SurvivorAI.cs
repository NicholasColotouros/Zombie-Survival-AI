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

	private bool RunningAway = false;

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
		if( TimeOut > 0 )
		{
			if(nextWaypoint < Waypoints.Length)
			{
				if( ZombieSpotted() )
				{
					RunningAway = true;
					GoToClosestSafeSpot();
				}
				else if(RunningAway)
				{
					if(DestinationReached())
						RunningAway = false;
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
			else Debug.Log(300 - TimeOut);
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
	private bool ZombieSpotted()
	{
		// check if zombie is in line of sight
		for( int i = 0; i < Zombies.childCount; i++ )
		{
			Transform Z = Zombies.GetChild(i);
			if( CanSeeZombie(Z) )
				return true;
		}
		return false;
	}

	private void GoToClosestSafeSpot()
	{
		Vector3 position = gameObject.transform.position;

		// find the closest safe point and go there
		Vector3 closestSafeSpot = Vector3.one;
		float safeSpotDistance = 999999.9f;

		for(int i = 1; i < SafeSpots.Length; i++)
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
