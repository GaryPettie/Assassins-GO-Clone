using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOG.Utilities;


public class CharacterManager : MonoBehaviour {
	protected Mover characterMover;
	public Mover CharacterMover { get { return characterMover; } }
	
	Node currentNode;
	public Node CurrentNode {
		get {
			if (board != null) {
				currentNode = board.FindNodeAt(transform.position);
			}
			return currentNode;
		}
	}

	protected Board board;

	protected virtual void Awake () {
		board = FindObjectOfType<Board>();
		characterMover = GetComponent<Mover>();
	}

	protected virtual void Start () {
	}

	protected virtual void Update () {
		
	}

	public void Move (Vector3 destinationPos, float delayTime = 0f) {
		if (board != null) {
			Node target = board.FindNodeAt(destinationPos);
			if (board.IsDrawn && target != null && CurrentNode.LinkedNodes.Contains(target)) {
				characterMover.Move(destinationPos);
			}
		}
	}
}
