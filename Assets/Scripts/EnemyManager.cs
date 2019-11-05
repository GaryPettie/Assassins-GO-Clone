using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOG.Utilities;


[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemySensor))]
[RequireComponent(typeof(EnemyAttack))]
public class EnemyManager : CharacterManager {

	enum EnemyType { Stationary, Patrol }

	protected EnemyMover enemyMover;
	public EnemyMover EnemyMover { get { return enemyMover; } }

	[SerializeField] float endTurnDelay = 0.5f;

	protected EnemySensor enemySensor;
	public EnemySensor EnemySensor { get { return enemySensor; } }

	protected EnemyAttack enemyAttack;
	public EnemyAttack EnemyAttack { get { return enemyAttack; } }

	protected override void Awake () {
		base.Awake();
		enemyMover = GetComponent<EnemyMover>();
		enemySensor = GetComponent<EnemySensor>();
		enemyAttack = GetComponent<EnemyAttack>();
	}

	public void PlayTurn () {
		StartCoroutine(PlayTurnRoutine());
	}

	IEnumerator PlayTurnRoutine () {
		if (gameManager != null && !gameManager.IsGameOver) {
			enemySensor.UpdateSensor();

			yield return new WaitForSeconds(endTurnDelay);

			if (enemySensor.FoundPlayer) {
				gameManager.LoseLevel();

				Vector3 playerPos = board.PlayerNode.transform.position;
				enemyMover.Move(playerPos, 0f);
				while (enemyMover.IsMoving) {
					yield return null;
				}
			
				enemyAttack.Attack();
			}
			else {
				enemyMover.MoveOneTurn();
			}
		}
	}
}
