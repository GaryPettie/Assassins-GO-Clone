using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
	public delegate void OnCharacterMove ();
	public static OnCharacterMove notifyCharacterMoveObservers;

	private bool isMoving;
	public bool IsMoving { get { return isMoving; } set { isMoving = value; } }

	[SerializeField] Vector3 destination;
	[SerializeField] float minTargetDistance = 0.01f;

	[SerializeField] float moveSpeed = 1.5f;
	[SerializeField] iTween.EaseType easeType = iTween.EaseType.easeInOutSine;
	[SerializeField] float iTweenDelay = 0f;


	public void Move (Vector3 destinationPos, float delayTime = 0f) {
		StartCoroutine(MoveRoutine(destinationPos, delayTime));
	}
		
	IEnumerator MoveRoutine (Vector3 destinationPos, float delayTime = 0f) {
		IsMoving = true;
		destination = destinationPos;
		yield return new WaitForSeconds(delayTime);

		iTween.MoveTo(gameObject, iTween.Hash(
			"x", destinationPos.x,
			"y", destinationPos.y,
			"z", destinationPos.z,
			"delay", iTweenDelay,
			"easetype", easeType,
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
