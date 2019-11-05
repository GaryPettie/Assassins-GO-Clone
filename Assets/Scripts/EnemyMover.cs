using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType { Stationary, Patrol, Spinner }
public enum RotationDirection { Clockwise, Anticlockwise }

public class EnemyMover : Mover
{
	[SerializeField] MovementType movementType = MovementType.Stationary;
	[SerializeField] RotationDirection rotationDirection = RotationDirection.Clockwise;
	public MovementType MovementType { get { return movementType; } }

	[SerializeField] Vector3 moveDirection;

	Board board; 

	void Awake () {
		board = FindObjectOfType<Board>();
		moveDirection = Vector3.forward * Board.spacing;
	}

	public void MoveOneTurn () {
		switch (movementType) {
			case MovementType.Stationary:
				Stand();
				break;
			case MovementType.Patrol:
				Patrol();
				break;
			case MovementType.Spinner:
				Spin();
				break;
			default:
				break;
		}
	}

	void Stand () {
		StartCoroutine(StandRoutine());
	}

	IEnumerator StandRoutine () {
		yield return null;
		FinishMovementEvent.Invoke();
	}

	void Patrol () {
		StartCoroutine(PatrolRoutine());
	}

	IEnumerator PatrolRoutine () {
		if (board != null) {
			Node currentNode = board.FindNodeAt(transform.position);

			Vector3 startPosition = new Vector3(currentNode.Coordinate.x, 0, currentNode.Coordinate.y);
			Vector3 destination = startPosition + transform.TransformVector(moveDirection);
			Vector3 nextDestination = startPosition + transform.TransformVector(moveDirection * 2);

			Node destinationNode = board.FindNodeAt(destination);
			Node nextDestinationNode = board.FindNodeAt(nextDestination);

			Move(destination);

			while (IsMoving) {
				yield return null;
			}

			if (nextDestinationNode == null || !destinationNode.LinkedNodes.Contains(nextDestinationNode)) {
				destination = startPosition;
				FaceDestination(startPosition);
				yield return new WaitForSeconds(turnSpeed);
			}
		}

		FinishMovementEvent.Invoke();
	}

	void Spin () {
		StartCoroutine(SpinRoutine(rotationDirection));
	}

	//TODO Pass in rotation direction
	IEnumerator SpinRoutine (RotationDirection direction) {
		int counter = 0;

		switch (rotationDirection) {
			case RotationDirection.Clockwise:
				counter = 1;
				break;
			case RotationDirection.Anticlockwise:
				counter = 3;
				break;
			default:
				break;
		}
		
		Vector3 nextDirection;
		Vector3 destination = Vector3.forward * Board.spacing;
		float xPos;
		float zPos;
		Node currentNode = board.FindNodeAt(transform.position);
		Node destinationNode = null;

		while (destinationNode == null) {
			if (counter > 4 || counter < 0) {
				break;
			}

			xPos = Board.spacing * Mathf.Sin((Mathf.PI / 2) * counter);
			zPos = Board.spacing * Mathf.Cos((Mathf.PI / 2) * counter);
			nextDirection = new Vector3(xPos, 0, zPos);
			destination = transform.position + transform.TransformVector(nextDirection);
			destinationNode = board.FindNodeAt(destination);

			if (!currentNode.LinkedNodes.Contains(destinationNode)) {
				destinationNode = null;
			}

			switch (rotationDirection) {
				case RotationDirection.Clockwise:
					counter++;
					break;
				case RotationDirection.Anticlockwise:
					counter--;
					break;
				default:
					break;
			}
		}

		if (destinationNode != null) {
			FaceDestination(destination);
			yield return new WaitForSeconds(turnSpeed);
		}

		FinishMovementEvent.Invoke();
	}
}
