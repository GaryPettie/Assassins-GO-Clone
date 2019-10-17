using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using SOG.Utilities;

public class GameManager : Singleton<GameManager> {
	public UnityEvent setupEvent;
	public UnityEvent startLevelEvent;
	public UnityEvent playLevelEvent;
	public UnityEvent endLevelEvent;
	
	[SerializeField] float delay = 1f;

	Board board;
	PlayerManager player;

	bool hasLevelStarted = false;
	public bool HasLevelStarted { get { return hasLevelStarted; } }

	bool isGamePlaying = false;
	public bool IsGamePlaying { get { return isGamePlaying; } }

	bool isGameOver = false;
	public bool IsGameOver { get { return isGameOver; } }

	bool hasLevelFinished = false;
	public bool HasLevelFinished { get { return hasLevelFinished; } set { hasLevelFinished = value; } }


	void Awake () {
		board = FindObjectOfType<Board>();
		player = FindObjectOfType<PlayerManager>();
	}

	void Start () {
		if (board != null && player != null) {
			StartCoroutine(RunGameLoop());
		}
		else {
			Debug.LogError("[GameManager](Start) Error: Board and Player must not be null.");
		}
	}

	public void PlayLevel () {
		hasLevelStarted = true;
	}

	void RestartLevel () {
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}

	bool IsWinner () {
		if (board.PlayerNode == board.GoalNode) {
			return true;
		}
		return false;
	}

	IEnumerator RunGameLoop () {
		yield return StartCoroutine(StartLevelRoutine());
		yield return StartCoroutine(PlayLevelRoutine());
		yield return StartCoroutine(EndLevelRoutine());
	}

	IEnumerator StartLevelRoutine () {
		Debug.Log("Start Level");
		player.PlayerInput.InputEnabled = false;

		if (setupEvent != null) {
			setupEvent.Invoke();
		}

		while (!hasLevelStarted) {
			//Wait for button press to start level...
			yield return null;
		}

		if (startLevelEvent != null) {
			startLevelEvent.Invoke();
		}
	}

	IEnumerator PlayLevelRoutine () {
		Debug.Log("Play Level");
		isGamePlaying = true;
		yield return new WaitForSeconds(delay);
		player.PlayerInput.InputEnabled = true;

		if (playLevelEvent != null) {
			playLevelEvent.Invoke();
		}

		while (!isGameOver) {
			yield return null;

			//Win Conditions
			isGameOver = IsWinner();

			//Lose Conditions

		}
	}

	IEnumerator EndLevelRoutine () {
		Debug.Log("End Level");
		player.PlayerInput.InputEnabled = false;

		if (endLevelEvent != null) {
			endLevelEvent.Invoke();
		}

		//Show end screen...

		while (!hasLevelFinished) {
			//Wait for player continue action...
			//hasLevelFinished = true;
			yield return null;
		}

		RestartLevel();
	}
}
