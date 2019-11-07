using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOG.Utilities;


[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemySensor))]
[RequireComponent(typeof(EnemyAttack))]
[RequireComponent(typeof(EnemyDeath))]
public class EnemyManager : CharacterManager {

	enum EnemyType { Stationary, Patrol }

	public delegate void OnEnemyDeath (GameObject enemy);
	public static OnEnemyDeath notifyEnemyDeathObservers;

	protected EnemyMover enemyMover;
	public EnemyMover EnemyMover { get { return enemyMover; } }

	[SerializeField] float endTurnDelay = 0.5f;

	protected EnemySensor enemySensor;
	public EnemySensor EnemySensor { get { return enemySensor; } }

	protected EnemyAttack enemyAttack;
	public EnemyAttack EnemyAttack { get { return enemyAttack; } }

	bool isDead = false;
	public bool IsDead { get { return isDead; } }

	protected override void Awake () {
		base.Awake();
		enemyMover = GetComponent<EnemyMover>();
		enemySensor = GetComponent<EnemySensor>();
		enemyAttack = GetComponent<EnemyAttack>();
	}

	public void PlayTurn () {
		if (isDead) {
			FinishTurn();
			return;
		}

		StartCoroutine(PlayTurnRoutine());
		
	}
	

	IEnumerator PlayTurnRoutine () {
		if (gameManager != null && !gameManager.IsGameOver) {
			enemySensor.UpdateSensor(CurrentNode);

			yield return new WaitForSeconds(endTurnDelay);
			Debug.Log(enemySensor.FoundPlayer);
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

	public void Die () {
		if (isDead) {
			return;
		}
		isDead = true;
		if (notifyEnemyDeathObservers != null) {
			notifyEnemyDeathObservers(gameObject);
		}
	}
}
