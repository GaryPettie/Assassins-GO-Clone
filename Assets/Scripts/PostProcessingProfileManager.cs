using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(PostProcessVolume))]
public class PostProcessingProfileManager : MonoBehaviour
{
	[SerializeField] PostProcessProfile defaultProfile;
	[SerializeField] PostProcessProfile blurProfile;

	PostProcessVolume volume;

	void Awake () {
		volume = GetComponent<PostProcessVolume>();
	}

	public void EnableBlurProfile (bool state) {
		if (defaultProfile != null && blurProfile != null) {
			volume.profile = state ? blurProfile : default;
		}
	}
}
