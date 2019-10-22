using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DisplayLevelName : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI label;

	void Awake () {
		if (label != null) {
			label.text = SceneManager.GetActiveScene().name;
		}
	}
}
