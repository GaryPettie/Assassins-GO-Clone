using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
	protected GameManager gameManager;

	protected bool isTurnComplete = false;
	public bool IsTurnComplete { get { return isTurnComplete; } set { isTurnComplete = value; } }

	protected virtual void Awake () {
		gameManager = FindObjectOfType<GameManager>();
	}

	public void FinishTurn () {
		isTurnComplete = true;
		if (gameManager != null) {
			gameManager.UpdateTurn();
		}
	}
}
