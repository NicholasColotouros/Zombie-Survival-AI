using UnityEngine;
using System.Collections;

public class Classic : Zombie 
{
	protected override void AdditionalSetup (){}
	
	protected override void ZombieMovement ()
	{
		// TODO: stop when another zombie is too close -- use raycasting forward
	}
}
