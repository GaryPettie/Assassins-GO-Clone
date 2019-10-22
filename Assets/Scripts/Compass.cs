using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Displays the available movement directions on the board
/// </summary>
public class Compass : MonoBehaviour {

	[SerializeField] GameObject arrowPrefab;
	[SerializeField] float scale = 1f;
	[SerializeField] float startDistance = 0.25f;
	[SerializeField] float endDistance = 0.5f;

	[SerializeField] float moveTime = 1f;
	[SerializeField] float delay = 0f;
	[SerializeField] iTween.EaseType easeType = iTween.EaseType.easeInOutSine;
	[SerializeField] iTween.LoopType loopType = iTween.LoopType.pingPong;

	PlayerManager playerManager;
	List<GameObject> arrows = new List<GameObject>();

	void Awake () {
		playerManager = FindObjectOfType<PlayerManager>();
		SetupArrows();
	}

	void SetupArrows () {
		if (arrowPrefab == null) {
			Debug.LogWarning("[Compass](SetupArrows) Warning: arrowPrefab should not be null.");
			return;
		}

		foreach (Vector2 dir in Board.directions) {
			Vector3 direction = new Vector3(dir.normalized.x, 0, dir.normalized.y);
			Quaternion rotation = Quaternion.LookRotation(direction);
			GameObject instance = Instantiate(arrowPrefab, transform.position + direction * startDistance, rotation, transform);
			arrows.Add(instance);
		}
	}

	void ResetArrows () {
		for (int i = 0; i < arrows.Count; i++) {
			iTween.Stop(arrows[i]);
			Vector3 direction = new Vector3(Board.directions[i].x, 0, Board.directions[i].y).normalized;
			arrows[i].transform.position = transform.position + direction * startDistance;
		}
	}

	void MoveArrow (GameObject arrow) {
		iTween.MoveBy(arrow, iTween.Hash(
			"z", endDistance - startDistance,
			"time", moveTime,
			"delay", delay,
			"easetype", easeType, 
			"looptype", loopType
		));
	}

	void MoveArrows () {
		foreach (GameObject arrow in arrows) {
			MoveArrow(arrow);
		}
	}

	public void ShowArrows (bool state) {
		if (arrows.Count != Board.directions.Length) {
			Debug.LogWarning("[Compass](ShowArrows) Warning: Arrow count should equal + " + Board.directions.Length + ".  Arrow count is currently " + arrows.Count + ".");
			return;
		}

		if (playerManager == null) {
			Debug.LogWarning("[Compass](ShowArrows) Warning: PlayerManager not found.");
			return;
		}

		if (playerManager.CurrentNode == null) {
			Debug.LogWarning("[Compass](ShowArrows) Warning: PlayerManager.PlayerNode has not been set.");
			return;
		}

		for (int i = 0; i < arrows.Count; i++) {
			Node neighbour = playerManager.CurrentNode.FindNeighbourAt(Board.directions[i]);

			if (neighbour == null || !state) {
				arrows[i].SetActive(false);
			}
			else {
				bool activeState = playerManager.CurrentNode.LinkedNodes.Contains(neighbour);
				arrows[i].SetActive(activeState);
			}
		}

		ResetArrows();
		MoveArrows();
	}
}
