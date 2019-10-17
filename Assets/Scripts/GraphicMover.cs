using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GraphicMoveMode { MoveTo, MoveFrom, ScaleTo, ScaleFrom }

public class GraphicMover : MonoBehaviour
{
	[SerializeField] GraphicMoveMode mode;
	[SerializeField] Transform startTransform;
	[SerializeField] Transform endTransform;
	[SerializeField] float duration = 1f;
	[SerializeField] float delay = 0f;
	[SerializeField] iTween.LoopType loopType = iTween.LoopType.none;
	[SerializeField] iTween.EaseType easeType = iTween.EaseType.linear;

	void Awake () {
		InitNullTransforms();
		Reset();
	}

	void InitNullTransforms() {
		if (startTransform == null) {
			startTransform = new GameObject(gameObject.name + "StartTransform").transform;
			startTransform.SetParent(transform);
			startTransform.position = transform.position;
			startTransform.rotation = transform.rotation;
			startTransform.localScale = transform.localScale;
		}
		if (endTransform == null) {
			endTransform = new GameObject(gameObject.name + "EndTransform").transform;
			endTransform.SetParent(transform);
			endTransform.position = transform.position;
			endTransform.rotation = transform.rotation;
			endTransform.localScale = transform.localScale;
		}
	}

	public void Reset () {
		switch (mode) {
			case GraphicMoveMode.MoveTo:
				if(startTransform != null) {
					transform.position = startTransform.position;
				}
				break;
			case GraphicMoveMode.MoveFrom:
				if (endTransform != null) {
					transform.position = endTransform.position;
				}
				break;
			case GraphicMoveMode.ScaleTo:
				if (startTransform != null) {
					transform.localScale = startTransform.localScale;
				}
				break;
			case GraphicMoveMode.ScaleFrom:
				if (endTransform != null) {
					transform.localScale = endTransform.localScale;
				}
				break;
			default:
				break;
		}
	}

	public void Stop () {
		iTween.StopByName("anim");
	}

	public void Move () {
		switch (mode) {
			case GraphicMoveMode.MoveTo:
				if (endTransform != null) {
					iTween.MoveTo(gameObject, iTween.Hash(
						"name", "anim",
						"position", endTransform.position,
						"time", duration,
						"delay", delay,
						"easetype", easeType,
						"looptype", loopType
					));
				}
				break;
			case GraphicMoveMode.MoveFrom:
				if (startTransform != null) {
					iTween.MoveFrom(gameObject, iTween.Hash(
						"name", "anim", 
						"position", startTransform.position,
						"time", duration,
						"delay", delay,
						"easetype", easeType,
						"looptype", loopType
					));
				}
				break;
			case GraphicMoveMode.ScaleTo:
				if (endTransform != null) {
					iTween.ScaleTo(gameObject, iTween.Hash(
						"name", "anim", 
						"scale", endTransform.localScale,
						"time", duration,
						"delay", delay,
						"easetype", easeType,
						"looptype", loopType
					));
				}
				break;
			case GraphicMoveMode.ScaleFrom:
				if (startTransform != null) {
					iTween.ScaleFrom(gameObject, iTween.Hash(
						"name", "anim", 
						"scale", startTransform.localScale,
						"time", duration,
						"delay", delay,
						"easetype", easeType,
						"looptype", loopType
					));
				}
				break;
			default:
				break;
		}
	}
}
