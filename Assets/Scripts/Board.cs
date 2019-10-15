using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
	public static int spacing = 2;

	public static readonly Vector2Int[] directions = {
		Vector2Int.up * spacing,
		Vector2Int.right * spacing,
		Vector2Int.down * spacing,
		Vector2Int.left * spacing
	};

	[SerializeField] List<Node> nodes = new List<Node>();
	public List<Node> Nodes { get { return nodes; } }

	Node playerNode;
	public Node PlayerNode { get { return playerNode; } }

	CharacterMover player;

	void Awake () {
		player = FindObjectOfType<CharacterMover>();
		GetNodeList();
	}

	void Start () {
		if (player != null) {
			UpdatePlayerNode();
			if (playerNode != null) {
				playerNode.InitNode();
			}
		}
	}

	void OnEnable () {
		CharacterMover.notifyCharacterMoveObservers += UpdatePlayerNode;
	}

	void OnDisable () {
		CharacterMover.notifyCharacterMoveObservers -= UpdatePlayerNode;
	}

	public void GetNodeList () {
		Node[] nodeArray = FindObjectsOfType<Node>();
		nodes = new List<Node>(nodeArray);
	}

	public Node FindNodeAt(Vector3 position) {
		Vector2Int coordinate = new Vector2Int((int)position.x, (int)position.z);
		return nodes.Find(n => n.Coordinate == coordinate);
	}

	public Node FindPlayerNode () {
		if (player != null && !player.IsMoving) {
			return FindNodeAt(player.transform.position);
		}
		return null;
	}

	public void UpdatePlayerNode () {
		playerNode = FindPlayerNode();
	}

	void OnDrawGizmos () {
		if (playerNode != null) {
			Gizmos.color = new Color(0f, 1f, 1f, 0.5f);
			Gizmos.DrawSphere(playerNode.transform.position, 0.25f);
		}
	}
}
