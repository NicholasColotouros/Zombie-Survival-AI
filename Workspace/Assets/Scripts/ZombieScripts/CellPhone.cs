using UnityEngine;
using System.Collections;

// inherits shambler because shambler already has the switch lanes function
// and cell phone zombies react the same when another zombie gets too close
public class CellPhone : Shambler 
{
	protected override void AdditionalSetup ()
	{
		InvokeRepeating ("ChangeTracks", 0f, 2f);
		InvokeRepeating ("ChangeDirection", 0f, 2f);
	}

	protected void ChangeDirection()
	{
		if( SurvivorSpotted )
		{
			CancelInvoke("ChangeDirection");
			return;
		}


		int diceroll = Random.Range (0, 2);

		if( diceroll == 1)
		{
			if(direction == Initializer.ZombieMovementDirection.Clockwise)
				direction = Initializer.ZombieMovementDirection.CounterClockwise;
			else
				direction = Initializer.ZombieMovementDirection.Clockwise;
		}

		if( stopped )
		{
			Nav.Resume();
			stopped = false;
		}
	}
}
