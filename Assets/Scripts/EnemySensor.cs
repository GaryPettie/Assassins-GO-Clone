using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensor : MonoBehaviour {
	Vector3 searchDirection;
	public Vector3 SearchDirection { get { return searchDirection; } }

	bool foundPlayer = false;
	public bool FoundPlayer { get { return foundPlayer; } }

	Board board;
	Node searchNode;

	void Awake () {
		board = FindObjectOfType<Board>();
		searchDirection = Vector3.forward * Board.spacing;
	}

	public void UpdateSensor (Node enemyNode) {
		Vector3 positionToSearch = transform.position + transform.TransformVector(searchDirection);
		if (board != null) {
			searchNode = board.FindNodeAt(positionToSearch);
			if (enemyNode.LinkedNodes.Contains(searchNode)) {
				if (searchNode != null && searchNode == board.PlayerNode) {
					foundPlayer = true;
				}
			}
			else {
				foundPlayer = false;
			}
		}
	}
}
