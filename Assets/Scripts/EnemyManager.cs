using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOG.Utilities;


[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemySensor))]
public class EnemyManager : CharacterManager {

	enum EnemyType { Stationary, Patrol }

	protected EnemyMover enemyMover;
	public EnemyMover EnemyMover { get { return enemyMover; } }

	[SerializeField] float endTurnDelay = 0.5f;

	protected EnemySensor enemySensor;
	public EnemySensor EnemySensor { get { return enemySensor; } }

	protected override void Awake () {
		base.Awake();
		enemyMover = GetComponent<EnemyMover>();
		enemySensor = GetComponent<EnemySensor>();
	}

	public void PlayTurn () {
		StartCoroutine(PlayTurnRoutine());
	}

	IEnumerator PlayTurnRoutine () {
		enemySensor.UpdateSensor();

		//attack

		//move
		EnemyManager enemy = GetComponent<EnemyManager>();
		yield return new WaitForSeconds(endTurnDelay);
		enemyMover.MoveOneTurn();
	}
}
