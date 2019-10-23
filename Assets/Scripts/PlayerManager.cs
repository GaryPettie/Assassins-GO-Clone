﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOG.Utilities;


[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerManager : CharacterManager {

	protected PlayerMover playerMover;
	public PlayerMover PlayerMover { get { return playerMover; } }

	protected PlayerInput playerInput;
	public PlayerInput PlayerInput { get { return playerInput; } }

	protected override void Awake () {
		base.Awake();
		playerMover = GetComponent<PlayerMover>();
		playerInput = GetComponent<PlayerInput>();
		if (playerInput != null) {
			playerInput.InputEnabled = true;
		}
	}

	void Update () {		
		if (characterMover.IsMoving || gameManager.CurrentTurn != Turn.Player) {
			return;
		}
		
		if (playerInput != null) {
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
	}
}