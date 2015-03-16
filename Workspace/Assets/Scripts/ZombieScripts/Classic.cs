using UnityEngine;
using System.Collections;

public class Classic : Zombie 
{
	protected float CloseDist = 5f; // distance that a zombie is considered too close
	protected bool stopped = false;

	protected override void AdditionalSetup ()
	{
		Nav.speed = speed;
	}

	// Raycast forward to see if a zombie is too close
	// stop if the raycast hits something (as it will be too close)
	protected override void ZombieMovement ()
	{
		Vector3 direction = gameObject.transform.forward;
		
		RaycastHit hit;
		if(Physics.Raycast(gameObject.transform.position, direction, out hit, CloseDist, 100))
		{
			if(hit.transform.tag == "Zombie")
			{
				Nav.Stop();
				stopped = true;
			}
			else if (stopped)
			{
				Nav.Resume();
				stopped = false;
			}
		}
	}
}
