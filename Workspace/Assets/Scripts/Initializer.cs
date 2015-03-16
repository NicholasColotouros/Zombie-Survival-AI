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

	private int easy;
	private int hard;

	// Use this for initialization
	void Start () 
	{
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
	
	}	
}
