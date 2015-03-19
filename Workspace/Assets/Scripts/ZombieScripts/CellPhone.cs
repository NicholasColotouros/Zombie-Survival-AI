using UnityEngine;
using System.Collections;

// inherits shambler because shambler already has the switch lanes function
// and cell phone zombies react the same when another zombie gets too close
public class CellPhone : Shambler 
{
	protected override void AdditionalSetup ()
	{
		//InvokeRepeating ("ChangeTracks", 2f, 2f);
		InvokeRepeating ("ChangeDirection", 2f, 2f);
		InvokeRepeating ("LookAtCellPhone", 5f, 5f);

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

	protected void LookAtCellPhone()
	{
		if( SurvivorSpotted )
		{
			CancelInvoke("LookAtCellPhone");
			return;
		}

		int diceroll = Random.Range (0, 2);
		if( diceroll == 1 )
		{
			Nav.Stop();
			Invoke( "PutAwayCellPhone", 3f);
		}
	}

	protected void PutAwayCellPhone()
	{
		Nav.Resume ();
	}
}
