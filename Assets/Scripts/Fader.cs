using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to fade Maskable graphic components and enable/disable any button component attached to the same gameObject.
/// </summary>
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

	/// <summary>
	/// Wrapper for iTween.ValueTo(gameObject, args), used to fade a MaskableGraphic alpha from 0 to 1.
	/// Also enables any attached Button component on the same gameObject.
	/// /// </summary>
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

	/// <summary>
	/// Wrapper for iTween.ValueTo(gameObject, args), used to fade a MaskableGraphic alpha from 1 to 0.
	/// Also disables any attached Button component on the same gameObject.
	/// </summary>
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
