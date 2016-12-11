using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Hit {
	public string Name;
	public string AnimationName;
	public string [] ButtonsName;
}

public class GameController : MonoBehaviour {

	public Hit [] Hits;

	Animator anim;

	void Awake () {
		anim = gameObject.GetComponent<Animator> ();
	}

	void Start () {
		StartCoroutine (RandomHits ());
	}

	bool timeToBlock = false;
	Hit currentHit;

	void Update () {
		if (timeToBlock) {
			if (Input.anyKeyDown) {
				bool allButtonsPressed = true;
				foreach (string buttonName in currentHit.ButtonsName) {
					if (!Input.GetButtonDown(buttonName)) {
						allButtonsPressed = false;
						break;
					}
				}
				if (allButtonsPressed) {
					hitBlocked ();		
				}
			}
		}
	}

	bool hitBlockedInTime = false;

	void makeHit (Hit hit) {
		hitBlockedInTime = false;
		timeToBlock = false;
		anim.SetTrigger (hit.AnimationName);
		currentHit = hit;
	}

	void hitBlocked () {
		timeToBlock = false;
		hitBlockedInTime = true;
		Debug.Log ("Hit blocked !");
	}

	/// <summary>
	/// The animation call this function when it's time to push the buttons to block.
	/// So it's the moment I put colors on UI.
	/// </summary>
	public void BlockStarting () {
		timeToBlock = true;
	}

	/// <summary>
	/// The animation call this function when it's to late to push the buttons to block.
	/// </summary>
	public void BlockEnding () {
		timeToBlock = false;
		if (!hitBlockedInTime) {
			Debug.Log ("Hit taken !");
		}
	}

	IEnumerator RandomHits () {
		while (true) {
			makeHit (Hits [Random.Range(0, Hits.Length)]);
			yield return new WaitForSeconds (4f);
		}
	}
}
