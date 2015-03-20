using UnityEngine;
using System.Collections;

public class Classic : Zombie 
{
	protected float CloseDist = 10f; // distance that a zombie is considered too close

	protected override void AdditionalSetup ()
	{
		Nav.speed = speed;
	}

	// Raycast forward to see if a zombie is too close
	// stop if the raycast hits something (as it will be too close)
	protected override void ZombieMovement ()
	{
		bool detectedZombie = false;
		Vector3 direction = gameObject.transform.forward;
		
		RaycastHit hit;
		Vector3 origin = gameObject.transform.position + gameObject.transform.up;
		LayerMask mask = 1 << LayerMask.NameToLayer ("Zombies");
		if(Physics.Raycast(origin, direction, out hit, CloseDist, mask))
		{
			if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Zombies"))
			{
				Debug.Log(hit.transform.name);
			
				detectedZombie = true;
				Nav.Stop();
			}
		}
		if( ! detectedZombie)
		{
			Nav.Resume();
		}
	}

	void OnDestroy()
	{
		BlackBoard.DecrementEasy (TrackIndex);
	}
}
