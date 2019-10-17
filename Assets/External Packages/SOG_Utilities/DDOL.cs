using UnityEngine;

namespace SOG.Utilities {

	public class DDOL : MonoBehaviour {

		void Start () {
			DontDestroyOnLoad(gameObject);
		}
	}
}