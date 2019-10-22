using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOG.Utilities;


[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemySensor))]
public class EnemyManager : CharacterManager {

	enum EnemyType { Stationary, Patrol }

	protected EnemySensor enemySensor;
	public EnemySensor EnemySensor { get { return enemySensor; } }

	protected override void Awake () {
		base.Awake();
		enemySensor = GetComponent<EnemySensor>();
	}


}
