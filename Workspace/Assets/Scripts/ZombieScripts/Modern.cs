using UnityEngine;
using System.Collections;

public class Modern : Zombie 
{
	protected override void AdditionalSetup ()
	{
		Nav.speed = speed * 2;
	}

	// The navmesh will take care of overtaking
	protected override void ZombieMovement (){}
}
