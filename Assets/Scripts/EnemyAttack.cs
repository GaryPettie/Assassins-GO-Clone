using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
	PlayerManager player;

	void Awake () {
		player = FindObjectOfType<PlayerManager>();
	}

	public void Attack () {
		if (player != null) {
			player.Die();
		}
	}
}
