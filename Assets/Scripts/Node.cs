using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour 
{
	[SerializeField] GameObject geometry;
	[SerializeField] GameObject linkPrefab;
	[SerializeField] LayerMask obstacleLayer;
	[SerializeField] float scaleTime = 0.3f;
	[SerializeField] iTween.EaseType easeType = iTween.EaseType.easeInSine;
	[SerializeField] float delay = 1f;
	[SerializeField] bool[] blockedLink = { false, false, false, false }; 

	Vector2Int coordinate;
	public Vector2Int Coordinate { get { return coordinate; } }

	[SerializeField] List<Node> neighbours = new List<Node>();
	public List<Node> Neighbours { get { return neighbours; } }

	[SerializeField] List<Node> linkedNodes = new List<Node>();
	public List<Node> LinkedNodes { get { return linkedNodes; } }

	bool isInitialized = false;
	Board board;

	
	void Awake () {
		board = FindObjectOfType<Board>();
		coordinate = new Vector2Int((int)transform.position.x, (int)transform.position.z);
	}

	void Start () {
		if (geometry != null) {
			geometry.transform.localScale = Vector3.zero;

			if (board != null) {
				neighbours = FindNeighbours(board.Nodes);
			}
		}
	}

	public void InitNode () {
		if (!isInitialized) {
			ShowGeometry();
			InitNeigbours();
			isInitialized = true;
		}
	}

	void InitNeigbours () {
		StartCoroutine(InitNeighboursRoutine());
	}

	IEnumerator InitNeighboursRoutine () {
		yield return new WaitForSeconds(delay);
		foreach (Node node in Neighbours) {
			if (!linkedNodes.Contains(node)) {
				if (!HitObstacle(node)) {
					LinkNode(node);
					node.InitNode();
				}
			}
		}
	}

	public void ShowGeometry () {
		if (geometry != null) {
			iTween.ScaleTo(geometry, iTween.Hash(
				"scale", Vector3.one,
				"time", scaleTime,
				"delay", delay,
				"easetype", easeType
				));
		}
	}

	public List<Node> FindNeighbours (List<Node> nodes) {
		List<Node> neighbours = new List<Node>();

		foreach (Vector2Int direction in Board.directions) {
			Node foundNeighbour = nodes.Find(n => n.Coordinate == Coordinate + direction);
			if (foundNeighbour != null && !neighbours.Contains(foundNeighbour)) {
				neighbours.Add(foundNeighbour);
			}
		}

		return neighbours;
	}

	void LinkNode (Node target) {
		if (linkPrefab != null) {
			GameObject linkInstance = Instantiate(linkPrefab, transform.position, Quaternion.identity);
			linkInstance.transform.parent = transform;

			Link link = linkInstance.GetComponent<Link>();
			if (link != null) {
				link.DrawLink(transform.position, target.transform.position);
			}

			if (!linkedNodes.Contains(target)) {
				linkedNodes.Add(target);
			}
			if (!target.LinkedNodes.Contains(this)) {
				target.LinkedNodes.Add(this);
			}
		}
	}

	bool HitObstacle (Node target) {
		Vector3 direction = target.transform.position - transform.position;
		
		if (direction.z > 0 && (blockedLink[0] || target.blockedLink[2])) {
			return true;
		}
		if (direction.x > 0 && (blockedLink[1] || target.blockedLink[3])) {
			return true;
		}
		if (direction.z < 0 && (blockedLink[2] || target.blockedLink[0])) {
			return true;
		}
		if (direction.x < 0 && (blockedLink[3] || target.blockedLink[1])) {
			return true;
		}

		RaycastHit hit;
		if (Physics.Raycast(transform.position, direction, out hit, Board.spacing, obstacleLayer)) {
			if (hit.collider.GetComponent<Obstacle>()) {
				if (direction.z > 0) { 
					blockedLink[0] = true;
					target.blockedLink[2] = true;
				}
				if (direction.x > 0) {
					blockedLink[1] = true;
					target.blockedLink[3] = true;
				}
				if (direction.z < 0) {
					blockedLink[2] = true;
					target.blockedLink[0] = true;
				}
				if (direction.x < 0) {
					blockedLink[3] = true;
					target.blockedLink[1] = true;
				}
				return true;
			}
		}

		return false;
	}

	void OnDrawGizmos () {
		float distance = 0.2f;
		float radius = 0.05f;
		Color red = new Color(1f, 0f, 0f, 0.5f);
		Color green = new Color(0f, 1f, 0f, 0.5f);

		Gizmos.color = green;
		if (blockedLink[0]) {
			Gizmos.color = red;
		}
		Gizmos.DrawSphere(transform.position + Vector3.forward * distance, radius);

		Gizmos.color = green;
		if (blockedLink[1]) {
			Gizmos.color = red;
		}
		Gizmos.DrawSphere(transform.position + Vector3.right * distance, radius);

		Gizmos.color = green;
		if (blockedLink[2]) {
			Gizmos.color = red;
		}
		Gizmos.DrawSphere(transform.position + Vector3.back * distance, radius);

		Gizmos.color = green;
		if (blockedLink[3]) {
			Gizmos.color = red;
		}
		Gizmos.DrawSphere(transform.position + Vector3.left * distance, radius);
	}
}