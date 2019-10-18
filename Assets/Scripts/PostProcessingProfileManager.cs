using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// Post-Processing profile manager use to swap between different profiles.
/// </summary>
[RequireComponent(typeof(PostProcessVolume))]
public class PostProcessingProfileManager : MonoBehaviour
{
	[SerializeField] PostProcessProfile defaultProfile;
	[SerializeField] PostProcessProfile blurProfile;

	PostProcessVolume volume;

	void Awake () {
		volume = GetComponent<PostProcessVolume>();
	}

	/// <summary>
	/// Switches between the default and blur post-processing profiles.
	/// </summary>
	public void EnableBlurProfile (bool state) {
		if (defaultProfile != null && blurProfile != null) {
			volume.profile = state ? blurProfile : default;
		}
	}
}
