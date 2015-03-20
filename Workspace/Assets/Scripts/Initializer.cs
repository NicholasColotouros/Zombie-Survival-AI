using UnityEngine;
using System.Collections;

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
	public int easy;
	public int hard;

	// number of zombies of each present
	public int easy_spawned;
	public int hard_spawned;

	private Object hardLock = new Object();
	private Object easyLock = new Object();

	public int firstWaveCounter; // used to make sure the first wave of zombies are all easy
	private bool firstWave = true;

	// Use this for initialization
	void Start () 
	{
		firstWaveCounter = n;
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
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(firstWaveCounter > 0 && firstWave)
		{
			SpawnZombie(EasyZombies);
			firstWaveCounter--;
			easy_spawned++;

			if( firstWaveCounter == 0 ) firstWave = false;
		}
		else SpawnZombies ();
	}

	// spawns zombies while maintaining the ratio
	private void SpawnZombies()
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
					SpawnZombie(EasyZombies);
					easy_spawned++;
				}
			}
			if( diceroll == 0 )
			{
				lock( hardLock )
				{
					SpawnZombie(HardZombies);
					hard_spawned++;
				}
			}
		}
	}

	// spawns easy zombies at a random available location
	private void SpawnZombie(Transform[] zombiePrefabs)
	{
		int selectedTrack = Random.Range (0, 3);
		int selectedWayPoint = Random.Range (0, 4);

		if( selectedWayPoint == lastWaypoint)
			selectedWayPoint = (lastWaypoint + 1) % 4;

		// get the track
		Transform[] track = InnerTrack;
		if( selectedTrack == 1 ) track = MiddleTrack;
		if( selectedTrack == 0 ) track = OuterTrack;


		Vector3 spawnpoint = track[selectedWayPoint].position;

		int selectedZombie = Random.Range (0, 2);
		Transform spawnedZombie = Instantiate (zombiePrefabs [selectedZombie], spawnpoint, Quaternion.identity) as Transform;

		Zombie properties = spawnedZombie.GetComponent<Zombie> ();
		properties.Track = track;
		properties.TrackIndex = selectedWayPoint;
		properties.TrackNumber = selectedTrack;

		lastWaypoint = selectedWayPoint;
	}

	// used by the zombies to decrement when they die
	// they pass their despawn index so the new zombie isn't
	// spawned in the same spot
	public void DecrementEasy(int index)
	{
		lastWaypoint = index;
		lock(easyLock)
		{
			Debug.Log("ho");
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