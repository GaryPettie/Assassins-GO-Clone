using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class Fader : MonoBehaviour
{
	//TODO Break this class up to add to individual maskablegraphics. Otherwise color has to be the same for all.

	Color solidColor = Color.white;
	Color clearColor = new Color(1f, 1f, 1f, 0);

	[SerializeField] float delay = 0.5f;
	[SerializeField] float timeToFade = 1f;
	[SerializeField] iTween.EaseType easeType = iTween.EaseType.easeOutExpo;
	MaskableGraphic graphicsToFade;
	Button button;

	void Awake () {
		button = GetComponent<Button>();
		graphicsToFade = GetComponent<MaskableGraphic>();
		solidColor = graphicsToFade.color;
		clearColor = new Color(graphicsToFade.color.r, graphicsToFade.color.g, graphicsToFade.color.b, 0);
	}

	void UpdateColor (Color newColor) {
		graphicsToFade.color = newColor;
	}

	void EnableButtons () {
		if (button != null) { 
			button.interactable = true;
		}
	}

	void DisableButtons () {
		if (button != null) {
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
