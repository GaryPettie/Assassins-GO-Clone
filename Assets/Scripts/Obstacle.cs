using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Obstacle : MonoBehaviour
{
	BoxCollider collider;

	void Awake () {
		collider = GetComponent<BoxCollider>();
	}

	void OnDrawGizmos () {
		Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
		Gizmos.DrawCube(transform.position, Vector3.one);
	}
}
