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

	// number of zombies of each needed
	private int easy;
	private int hard;

	// number of zombies of each present
	private int easy_spawned;
	private int hard_spawned;

	private Object hardLock = new Object();
	private Object easyLock = new Object();

	private int firstWaveCounter; // used to make sure the first wave of zombies are all easy

	// Use this for initialization
	void Start () 
	{
		firstWaveCounter = n;
		easy_spawned = 0;
		hard_spawned = 0;

		hard = (int) ( r * n );
		easy = n - hard;

		// Determine movement direction
		if( Random.Range(0, 2) == 1)
		{
			Direction = ZombieMovementDirection.Clockwise;
		}
		else Direction = ZombieMovementDirection.CounterClockwise;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(firstWaveCounter > 0)
		{
			SpawnEasyZombie();
		}
		else SpawnZombie ();
	}


	// spawns easy zombies at a random available location
	private void SpawnEasyZombie()
	{

		firstWaveCounter--;
		Vector3 spawnpoint = OuterTrack[0].position;

		Transform spawnedZombie = Instantiate (HardZombies [1], spawnpoint, Quaternion.identity) as Transform;
		Zombie properties = spawnedZombie.GetComponent<Zombie> ();
		properties.Track = OuterTrack;
		properties.TrackIndex = 0;
		properties.TrackNumber = 2;
	}

	// spawns hard zombies at a random available location
	private void SpawnHardZombie()
	{
		// TODO
	}

	// spawns zombies while maintaining the ratio
	private void SpawnZombie()
	{
		if( easy_spawned + hard_spawned < n)
		{
			int diceroll = Random.Range(0, 2);

			if( diceroll == 0 ) // spawn easy zombie
			{

			}
		}
	}
}
