using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	float horizontalThrow;
	public float HorizontalThrow { get { return horizontalThrow; } }

	float verticalThrow;
	public float VerticalThrow { get { return verticalThrow; } }

	bool inputEnabled = false;
	public bool InputEnabled { get { return inputEnabled; } set { inputEnabled = value; } }


	public void GetKeyInput () {
		if (inputEnabled) {
			horizontalThrow = Input.GetAxisRaw("Horizontal");
			verticalThrow = Input.GetAxisRaw("Vertical");
		}
	}
}
