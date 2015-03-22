using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Initializer : MonoBehaviour 
{
	public enum ZombieMovementDirection { Clockwise, CounterClockwise };

	public float v;
	public int n;
	public float p; // % chance of zombies despawning/respawning
	public float r; // number of hard zombies per easy zombies

	public Transform Survivor;
	public bool SurvivorSpotted;

	public Transform[] EasyZombies;
	public Transform[] HardZombies;

	public Transform[] OuterTrack;
	public Transform[] MiddleTrack;
	public Transform[] InnerTrack;

	public ZombieMovementDirection Direction;

	// last spawn spot
	private int lastWaypoint;

	// number of zombies of each needed
	private int easy;
	private int hard;

	// number of zombies of each present
	private int easy_spawned;
	private int hard_spawned;

	private Object hardLock = new Object();
	private Object easyLock = new Object();
	private Object queueLock = new Object();

	private Queue<Transform> SpawnQueue;
	private Transform[][] tracks;

	// Use this for initialization
	void Start () 
	{
		SpawnQueue = new Queue<Transform> ();
		easy_spawned = 0;
		hard_spawned = 0;

		if (r > 1) 
			hard = (int)Mathf.Round((1 - 1 / r) * n);
		else 
			hard = (int)Mathf.Round(r * n);

		easy = n - hard;

		// Determine movement direction
		Direction = ZombieMovementDirection.CounterClockwise;
		if( Random.Range(0, 2) == 1)
		{
			Direction = ZombieMovementDirection.Clockwise;
		}

		// Fill the spawn queue for the first wave
		for( int i = 0; i < n; i++)
		{
			int diceroll = Random.Range(0, EasyZombies.Length);
			SpawnQueue.Enqueue( EasyZombies[diceroll] );
			easy_spawned++;
		}

		tracks = new Transform[3][];
		tracks [0] = OuterTrack;
		tracks [1] = MiddleTrack;
		tracks [2] = InnerTrack;
	}
	
	// Update is called once per frame
	void Update () 
	{
		MaintainZombies ();

		int trackSelected = Random.Range(0, 3);
		SpawnZombie(tracks[trackSelected], trackSelected);
	}

	// Adds zombies to the spawn queue while maintaining the ratio
	private void MaintainZombies()
	{
		if( easy_spawned + hard_spawned < n)
		{
			int diceroll = Random.Range(0, 2);
			if( easy_spawned >= easy )
			{
				diceroll = 0;
			}
			else if( hard_spawned >= hard )
			{
				diceroll = 1;
			}

			if( diceroll == 1 )
			{
				lock( easyLock )
				{
					easy_spawned++;

					lock( queueLock )
					{
						int zombietype = Random.Range(0, EasyZombies.Length);
						SpawnQueue.Enqueue( EasyZombies[zombietype] );
					}
				}
			}
			if( diceroll == 0 )
			{
				lock( hardLock )
				{
					hard_spawned++;

					lock( queueLock )
					{
						int zombietype = Random.Range(0, HardZombies.Length);
						SpawnQueue.Enqueue( HardZombies[zombietype] );
					}
				}
			}
		}
	}

	// spawns easy zombies at a random available location
	private void SpawnZombie(Transform[] track, int trackNumber)
	{
		bool emptyQueue = false;

		lock(queueLock)
		{
			if(SpawnQueue.Count == 0)
				emptyQueue = true;
		}

		if( !emptyQueue )
		{
			int selectedWayPoint = Random.Range (0, 4);

			if( selectedWayPoint == lastWaypoint)
				selectedWayPoint = (lastWaypoint + 1) % 4;

			// Don't spawn anything if there is already a zombie there
			if( track[selectedWayPoint].GetComponent<SpawnAreaCounter>().Counter > 0 )
				return;

			Vector3 spawnpoint = track[selectedWayPoint].position;

			Transform zombietospawn;
			lock(queueLock)
			{
				zombietospawn = SpawnQueue.Dequeue();
			}
			Transform spawnedZombie = Instantiate (zombietospawn, spawnpoint, Quaternion.identity) as Transform;
			Zombie properties = spawnedZombie.GetComponent<Zombie> ();
			properties.Track = track;
			properties.TrackIndex = selectedWayPoint;
			properties.TrackNumber = trackNumber;

			lastWaypoint = selectedWayPoint;
		}
	}

	// used by the zombies to decrement when they die
	// they pass their despawn index so the new zombie isn't
	// spawned in the same spot
	public void DecrementEasy(int index)
	{
		lastWaypoint = index;
		lock(easyLock)
		{
			easy_spawned--;
		}
	}

	public void DecrementHard(int index)
	{
		lastWaypoint = index;
		lock(hardLock)
		{
			hard_spawned--;
		}
	}
}