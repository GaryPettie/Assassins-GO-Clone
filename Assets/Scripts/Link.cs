using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
	[SerializeField] float borderWidth = 0.02f;
	[SerializeField] float lineThickness = 0.5f;
	[SerializeField] float scaleTime = 0.25f;
	[SerializeField] iTween.EaseType easeType = iTween.EaseType.easeInOutSine;
	[SerializeField] float delay = 1f;

	public void DrawLink (Vector3 startPos, Vector3 endPos) {
		transform.localScale = new Vector3(lineThickness, 1f, 0f);
		Vector3 direction = endPos - startPos;
		float zScale = direction.magnitude - borderWidth * 2;
		Vector3 scale = new Vector3(lineThickness, 1f, zScale);
		transform.rotation = Quaternion.LookRotation(direction);
		transform.position = startPos + transform.forward * borderWidth;

		iTween.ScaleTo(gameObject, iTween.Hash(
			"scale", scale,
			"time", scaleTime,
			"delay", delay,
			"easetype", easeType
			));
	}
}
