using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOG.Utilities;

/// <summary>
/// Responsible for converting player input into character movement.
/// </summary>
[RequireComponent(typeof(CharacterMover))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerManager : Singleton<PlayerManager> {
	private CharacterMover characterMover;
	public CharacterMover CharacterMover { get { return characterMover; } }

	private PlayerInput playerInput;
	public PlayerInput PlayerInput { get { return playerInput; } }

	Node playerNode;
	public Node PlayerNode {
		get {
			if (board != null) {
				playerNode = board.PlayerNode;
			}
			return playerNode;
		}
	}

	Board board;

	void Awake () {
		board = FindObjectOfType<Board>();
		characterMover = GetComponent<CharacterMover>();
		playerInput = GetComponent<PlayerInput>();
		playerInput.InputEnabled = true;
	}

	void Update () {
		if (characterMover.IsMoving) {
			return;
		}

		playerInput.GetKeyInput();

		if (board != null) {
			if (playerInput.VerticalThrow == 0) {
				if (playerInput.HorizontalThrow < 0) {
					Vector3 destination = transform.position + Vector3.left * Board.spacing;
					Move(destination);
				}
				else if (playerInput.HorizontalThrow > 0) {
					Vector3 destination = transform.position + Vector3.right * Board.spacing;
					Move(destination);
				}
			}

			if (playerInput.HorizontalThrow == 0) {
				if (playerInput.VerticalThrow < 0) {
					Vector3 destination = transform.position + Vector3.back * Board.spacing;
					Move(destination);
				}
				else if (playerInput.VerticalThrow > 0) {
					Vector3 destination = transform.position + Vector3.forward * Board.spacing;
					Move(destination);
				}
			}
		}
	}

	void Move (Vector3 destinationPos, float delayTime = 0f) {
		if (board != null) {
			Node target = board.FindNodeAt(destinationPos);
			if (board.IsDrawn && target != null && PlayerNode.LinkedNodes.Contains(target)) {
				characterMover.Move(destinationPos);
				playerNode = board.PlayerNode;
			}
		}
	} 
}
