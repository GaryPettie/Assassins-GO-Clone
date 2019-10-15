using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
	Color solidColor = Color.white;
	Color clearColor = new Color(1f, 1f, 1f, 0);

	[SerializeField] float delay = 0.5f;
	[SerializeField] float timeToFade = 1f;
	[SerializeField] iTween.EaseType easeType = iTween.EaseType.easeOutExpo;
	[SerializeField] MaskableGraphic[] graphicsToFade;
	List<Button> buttons = new List<Button>();
	
	void Awake () {
		for (int i = 0; i < graphicsToFade.Length; i++) {
			if (graphicsToFade[i].GetComponent<Button>() != null) {
				buttons.Add(graphicsToFade[i].GetComponent<Button>());
			}
		}
	}

	void UpdateColor (Color newColor) {
		if (graphicsToFade != null) {
			foreach (MaskableGraphic graphic in graphicsToFade) {
				graphic.color = newColor;
			}
		}
	}

	void EnableButtons () {
		foreach (Button button in buttons) {
			button.interactable = true;
		}
	}

	void DisableButtons () {
		foreach (Button button in buttons) {
			button.interactable = false;
		}
	}

	public void FadeOn () {
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", clearColor,
			"to", solidColor,
			"time", timeToFade,
			"delay", delay,
			"easetype", easeType,
			"onupdatetarget", gameObject,
			"onupdate", "UpdateColor"
		));
		EnableButtons();
	}

	public void FadeOff () {
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", solidColor,
			"to", clearColor,
			"time", timeToFade,
			"delay", delay,
			"easetype", easeType,
			"onupdatetarget", gameObject,
			"onupdate", "UpdateColor"
		));
		DisableButtons();
	}
}
