using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls character movement and 
/// </summary>
public class CharacterMover : MonoBehaviour
{
	public delegate void OnCharacterMove ();
	public static OnCharacterMove notifyCharacterMoveObservers;

	private bool isMoving;
	public bool IsMoving { get { return isMoving; } set { isMoving = value; } }

	[SerializeField] Vector3 destination;
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
		StartCoroutine(MoveRoutine(destinationPos, delayTime));
	}
		
	IEnumerator MoveRoutine (Vector3 destinationPos, float delayTime = 0f) {
		IsMoving = true;
		destination = destinationPos;
		yield return new WaitForSeconds(delayTime);

		iTween.LookTo(gameObject, iTween.Hash(
			"looktarget", destinationPos,
			"delay", turnDelay,
			"easetype", turnEaseType,
			"time", turnSpeed
		));

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

		if (notifyCharacterMoveObservers != null) {
			notifyCharacterMoveObservers();
		}
	}
}
