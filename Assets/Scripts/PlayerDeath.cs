using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
	[SerializeField] Animator anim;
	string animDeathTrigger = "isDead";

	void OnEnable () {
		PlayerManager.notifyPlayerDeathObservers += Die;
	}

	void OnDisable () {
		PlayerManager.notifyPlayerDeathObservers -= Die;
	}

	public void Die () {
		if (anim != null) {
			anim.SetTrigger(animDeathTrigger);
		}
	}
}
