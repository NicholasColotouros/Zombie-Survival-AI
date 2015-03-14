using UnityEngine;
using System.Collections;

public class Initializer : MonoBehaviour 
{
	public enum ZombieMovementDirection { NONE, Clockwise, CounterClockwise };

	public float v;
	public int n;
	public float p; // % chance of zombies despawning/respawning
	public float r; // number of hard zombies per easy zombies

	public Transform[] Zombies; // first two indexes are easy, last two are hard zombies
	public ZombieMovementDirection Direction;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
