using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
	[Header("Rotation")]
	[SerializeField] float rotationSpeed = 50f;
	[SerializeField] iTween.EaseType rotationEaseType = iTween.EaseType.linear;
	[SerializeField] iTween.LoopType rotationLoopType = iTween.LoopType.loop;

	void Start () {
		iTween.RotateBy(gameObject, iTween.Hash(
			"y", 360f,
			"speed", rotationSpeed,
			"easetype", rotationEaseType,
			"looptype", rotationLoopType
		));
	}
}
