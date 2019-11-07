using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOG.Utilities;

public class Board : Singleton<Board> {
	public static int spacing = 2;

	public static readonly Vector2Int[] directions = {
		Vector2Int.up * spacing,
		Vector2Int.right * spacing,
		Vector2Int.down * spacing,
		Vector2Int.left * spacing
	};

	List<Node> nodes = new List<Node>();
	public List<Node> Nodes { get { return nodes; } }

	Node playerNode;
	public Node PlayerNode { get { return playerNode; }	}

	Node startNode;
	public Node StartNode { get { return startNode; } }

	Node goalNode;
	public Node GoalNode { get { return goalNode; } }

	bool isDrawn;
	public bool IsDrawn { get { return isDrawn; } }

	PlayerMover player;

	[SerializeField] Transform capturePointParent;

	List<Transform> capturePositions = new List<Transform>();
	public List<Transform> CapturePositions { get { return capturePositions; } }

	int currentCaptureIndex	= 0;
	public int CurrentCaptureIndex { get { return currentCaptureIndex; } set { currentCaptureIndex = value; } }

	protected override void Awake () {
		base.Awake();
		player = FindObjectOfType<PlayerMover>();
		GetNodeList();

		foreach (Transform child in capturePointParent) {
			capturePositions.Add(child);
		}
	}

	void Update () {
		if (!isDrawn) {
			int count = 0;
			for (int i = 0; i < nodes.Count; i++) {
				if (nodes[i].IsInitialized) {
					count++;
				}
			}
			if (count == nodes.Count) {
				isDrawn = true;
			}
		}
	}

	void OnEnable () {
		Mover.notifyCharacterMoveObservers += UpdatePlayerNode;
	}

	void OnDisable () {
		Mover.notifyCharacterMoveObservers -= UpdatePlayerNode;
	}

	public void InitBoard () {
		if (player != null) {
			FindStartNode();
			FindGoalNode();
			player.transform.position = startNode.transform.position;
			UpdatePlayerNode();
			if (playerNode != null) {
				playerNode.InitNode();
			}
		}
	}

	public void GetNodeList () {
		Node[] nodeArray = FindObjectsOfType<Node>();
		nodes = new List<Node>(nodeArray);
	}

	public Node FindNodeAt(Vector3 position) {
		//NOTE: had to use RoundToInt here, as a simple cast to int was causing some issues.
		Vector2Int coordinate = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
		return nodes.Find(n => n.Coordinate == coordinate);
	}

	public Node FindPlayerNode () {
		if (player != null && !player.IsMoving) {
			return FindNodeAt(player.transform.position);
		}
		return null;
	}

	public List<EnemyManager> FindEnemiesAt (Node node) {
		List<EnemyManager> foundEnemies = new List<EnemyManager>();
		GameManager gameManager = FindObjectOfType<GameManager>();
		List<EnemyManager> enemies = gameManager.Enemies;

		foreach (EnemyManager enemy in enemies) {
			if (enemy.CurrentNode == node) {
				foundEnemies.Add(enemy);
			}
		}

		return foundEnemies;
	}

	public void UpdatePlayerNode () {
		playerNode = FindPlayerNode();
	}

	public Node FindStartNode () {
		if (startNode == null) {
			startNode = Nodes.Find(n => n.IsStartNode);
		}
		return startNode;
	}

	public Node FindGoalNode () {
		if (goalNode == null) {
			goalNode = Nodes.Find(n => n.IsGoalNode);
		}
		return GoalNode;
	}

	void OnDrawGizmos () {
		if (playerNode != null) {
			Gizmos.color = new Color(0f, 1f, 1f, 0.5f);
			Gizmos.DrawSphere(playerNode.transform.position, 0.25f);
		}
	}
}
