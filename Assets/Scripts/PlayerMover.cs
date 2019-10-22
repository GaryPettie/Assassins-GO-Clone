using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherits from Mover.
/// In addition to moving the player, the player compass is also updated.
/// </summary>
public class PlayerMover : Mover
{
	public delegate void OnCharacterMove ();
	public static OnCharacterMove notifyPlayerMoveObservers;

	Compass compass;

	void Awake () {
		compass = FindObjectOfType<Compass>();
	}

	protected override IEnumerator MoveRoutine (Vector3 destinationPos, float delayTime = 0) {
		if (compass != null) {
			compass.ShowArrows(false);
		}

		yield return StartCoroutine(base.MoveRoutine(destinationPos, delayTime));

		if (notifyPlayerMoveObservers != null) {
			notifyPlayerMoveObservers();
		}

		if (compass != null) {
			compass.ShowArrows(true);
		}

		yield return null;
	}
}
