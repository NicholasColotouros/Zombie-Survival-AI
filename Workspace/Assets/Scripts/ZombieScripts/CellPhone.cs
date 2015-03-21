using UnityEngine;
using System.Collections;

// inherits shambler because shambler already has the switch lanes function
// and cell phone zombies react the same when another zombie gets too close
public class CellPhone : Shambler 
{
	protected override void AdditionalSetup ()
	{
		InvokeRepeating ("ChangeTracks", 5f, 5f);
//		InvokeRepeating ("ChangeDirection", 2f, 2f);
//		InvokeRepeating ("LookAtCellPhone", 10f, 10f);
//		InvokeRepeating ("ChangeSpeed", 1f, 1f);
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
			{
				direction = Initializer.ZombieMovementDirection.CounterClockwise;

			}
			else
			{
				direction = Initializer.ZombieMovementDirection.Clockwise;
			}
			AssignNextWayPoint();
			Nav.SetDestination( Track[TrackIndex].position);

			gameObject.transform.Rotate( new Vector3(0, 180, 0));
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
			Nav.SetDestination(transform.position);
			Invoke( "PutAwayCellPhone", 3f);
		}
	}

	protected void ChangeSpeed()
	{
		float newSpeed = Random.Range (speed / 2, 2 * speed);
		Nav.speed = newSpeed;
	}

	protected void PutAwayCellPhone()
	{
		Nav.SetDestination (Track [TrackIndex].position);
	}

	void OnDestroy()
	{
		BlackBoard.DecrementHard (TrackIndex);
	}
}
