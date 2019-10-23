using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : Mover
{
	[SerializeField] float standTime = 1f;

	public void MoveOneTurn () {
		Stand();
	}

	void Stand () {
		StartCoroutine(StandRoutine());
	}

	IEnumerator StandRoutine () {
		yield return new WaitForSeconds(standTime);
		FinishMovementEvent.Invoke();
	}
}
