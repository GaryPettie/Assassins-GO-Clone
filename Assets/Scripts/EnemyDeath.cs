using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour 
{
	[SerializeField] Vector3 offscreenOffset = new Vector3(0f, 10f, 0f);
	[SerializeField] float deathDelay = 0f;
	[SerializeField] float offscreenDelay = 1f;

	[SerializeField] float iTweenDelay;
	[SerializeField] float moveTime = 0.5f;
	[SerializeField] iTween.EaseType easeType = iTween.EaseType.easeInOutSine;
	

	Board board;

	[SerializeField] Animator anim;
	string animDeathTrigger = "isDead";

	void OnEnable () {
		EnemyManager.notifyEnemyDeathObservers += Die;
	}

	void OnDisable () {
		EnemyManager.notifyEnemyDeathObservers -= Die;
	}

	void Awake () {
		board = FindObjectOfType<Board>();
	}

	void MoveOffBoard(Vector3 target) {
		iTween.MoveTo(gameObject, iTween.Hash(
			"position", target,
			"time", moveTime,
			"delay", iTweenDelay,
			"easetype", easeType
		));
	}

	public void Die (GameObject enemy) {
		if (enemy == gameObject) {
			StartCoroutine(DieRoutine());
		}
	}

	IEnumerator DieRoutine () {
		yield return new WaitForSeconds(deathDelay);
		Vector3 offscreenPos = transform.position + offscreenOffset;
		MoveOffBoard(offscreenPos);
		yield return new WaitForSeconds(moveTime + offscreenDelay);

		if (board != null) {
			if (board.CapturePositions.Count != 0 && board.CurrentCaptureIndex < board.CapturePositions.Count) {
				Vector3 capturePos = board.CapturePositions[board.CurrentCaptureIndex].position;
				transform.position = capturePos + offscreenOffset;
				transform.rotation = Quaternion.Euler(0f , 180f, 0f);
				MoveOffBoard(capturePos);
				yield return new WaitForSeconds(moveTime);
				board.CurrentCaptureIndex++;
				board.CurrentCaptureIndex = Mathf.Clamp(board.CurrentCaptureIndex, 0, board.CapturePositions.Count - 1);
			}
		}
	}
}
