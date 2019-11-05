using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using SOG.Utilities;
using System.Linq;

[System.Serializable]
public enum Turn { Player, Enemy }

/// <summary>
/// Handles game logic and flow.
/// </summary>
public class GameManager : Singleton<GameManager> {
	public UnityEvent setupEvent;
	public UnityEvent startLevelEvent;
	public UnityEvent playLevelEvent;
	public UnityEvent endLevelEvent;
	public UnityEvent loseLevelEvent;
	
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

	List<EnemyManager> enemies;

	Turn currentTurn = Turn.Player;
	public Turn CurrentTurn { get { return currentTurn; } }

	protected override void Awake () {
		base.Awake();
		board = FindObjectOfType<Board>();
		player = FindObjectOfType<PlayerManager>();
		enemies = FindObjectsOfType<EnemyManager>().ToList();
	}

	void Start () {
		if (board != null && player != null) {
			StartCoroutine(RunGameLoop());
		}
		else {
			Debug.LogError("[GameManager](Start) Error: Board and Player must not be null.");
		}
	}

	/// <summary>
	/// Sets hasLevelStarted to true, which is used to break out of the StartLevelRoutine() coroutine.
	/// </summary>
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

	public void UpdateTurn () {
		if (player != null) {
			switch (currentTurn) {
				case Turn.Player:
					if (player.IsTurnComplete) {
						PlayEnemyTurn();
					}
					break;
				case Turn.Enemy:
					if (isEnemyTurnComplete()) {
						PlayPlayerTurn();
					}
					break;
				default:
					Debug.LogError("[GameManager](UpdateTurn) Error: Turn type not recognized.");
					break;
			}
		}
	}

	void PlayPlayerTurn () {
		currentTurn = Turn.Player;
		player.IsTurnComplete = false;
	}

	void PlayEnemyTurn () {
		currentTurn = Turn.Enemy;
		foreach (EnemyManager enemy in enemies) {
			if (enemy != null) {
				enemy.IsTurnComplete = false;
				enemy.PlayTurn();
			}
		}
	}

	bool isEnemyTurnComplete () {
		foreach (EnemyManager enemy in enemies) {
			if (!enemy.IsTurnComplete) {

				return false;
			}
		}
		return true;
	}

	IEnumerator RunGameLoop () {
		Debug.Log("Run Game Loop");
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

	public void LoseLevel () {
		StartCoroutine(LoseLevelRoutine());
	}

	IEnumerator LoseLevelRoutine () {
		Debug.Log("Level Lost");
		isGameOver = true;

		yield return new WaitForSeconds(1.5f);

		if (loseLevelEvent != null) {
			loseLevelEvent.Invoke();
		}

		//TODO Don't hard code this number here.  It's there to wait for animations to complete so maybe access that time directly?
		yield return new WaitForSeconds(1f);

		RestartLevel();
	}
}
