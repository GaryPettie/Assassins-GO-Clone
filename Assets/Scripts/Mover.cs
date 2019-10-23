using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Responsible for moving a character.
/// </summary>
public class Mover : MonoBehaviour
{
	public delegate void OnCharacterMove ();
	public static OnCharacterMove notifyCharacterMoveObservers;
	public UnityEvent FinishMovementEvent;

	private bool isMoving;
	public bool IsMoving { get { return isMoving; } set { isMoving = value; } }

	Vector3 destination;
	[SerializeField] bool enableLookRotation = false;
	[SerializeField] float minTargetDistance = 0.01f;

	[SerializeField] float moveSpeed = 1.5f;
	[SerializeField] float turnSpeed = 0.25f;
	[SerializeField] iTween.EaseType moveEaseType = iTween.EaseType.easeInOutSine;
	[SerializeField] iTween.EaseType turnEaseType = iTween.EaseType.easeInOutSine;
	[SerializeField] float moveDelay = 0f;
	[SerializeField] float turnDelay = 0f;

	/// <summary>
	/// Moves the player to the provided destination after a specified delay.
	/// </summary>
	/// <param name="destinationPos"></param>
	/// <param name="delayTime"></param>
	public void Move (Vector3 destinationPos, float delayTime = 0f) {
		if (IsMoving) {
			return;
		}
		StartCoroutine(MoveRoutine(destinationPos, delayTime));
	}

	protected virtual IEnumerator MoveRoutine (Vector3 destinationPos, float delayTime = 0f) {
		IsMoving = true;

		destination = destinationPos;
		yield return new WaitForSeconds(delayTime);

		if (enableLookRotation) {
			iTween.LookTo(gameObject, iTween.Hash(
				"looktarget", destinationPos,
				"delay", turnDelay,
				"easetype", turnEaseType,
				"time", turnSpeed
			));
		}

		iTween.MoveTo(gameObject, iTween.Hash(
			"position", destinationPos,
			"delay", moveDelay,
			"easetype", moveEaseType,
			"speed", moveSpeed
		));

		while (Vector3.Distance(destinationPos, transform.position) > minTargetDistance) {
			yield return null;
		}

		iTween.Stop(gameObject);
		transform.position = destinationPos;
		IsMoving = false;

		FinishMovementEvent.Invoke();
		if (notifyCharacterMoveObservers != null) {
			notifyCharacterMoveObservers();
		}
	}
}
