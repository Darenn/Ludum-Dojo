using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public struct Hit {
	public string Name;
	public string AnimationName;
	public string [] ButtonsName;
	public int Value;
}

public class GameController : MonoBehaviour {

	public Hit [] Hits;
	public UIController uic;
	public int Score;
	public int Health = 3;
	public int numHitsToUpgradeDifficulty = 4;
	public float Difficulty = 1.0f;
	public float TimeBetweenAttacks = 4f;
	bool gameOver = false;
	int hitCount = 0;

	public AudioClip[] clips;
	public AudioClip[] woodShocks;
	public AudioClip hurt;
	public AudioSource audioSource;
	public AudioSource audioSourceHit;

	Animator anim;
	List<string> possibleButtons;

	void Awake () {
		possibleButtons = new List<string> (new string[] {"Arms.Up", "Arms.Down", "Arms.Right", "Arms.Left", "Legs.Up", "Legs.Down", "Legs.Left", "Legs.Right"});
		anim = gameObject.GetComponent<Animator> ();
	}

	void Start () {
		uic.UpdateHealth (Health);
	}

	bool timeToBlock = false;
	Hit currentHit;
	List<string> buttonsToPush = new List<string> ();
	List<string> buttonsPushed = new List<string> ();
	bool wrongButtonPressed = false;
	bool hitStarted = false;
	bool pressTooEarly = false;

	void Update () {
		if (Input.GetButtonDown ("Cancel")) {
			Application.Quit ();
		}
		if (timeToBlock) {
			if (Input.anyKeyDown) {
				wrongButtonPressed = true;
				foreach (string buttonName in possibleButtons) {
					if (buttonsToPush.Contains (buttonName)) {
						if (Input.GetButtonDown (buttonName) && !buttonsPushed.Contains (buttonName)) {
							buttonsPushed.Add (buttonName);
							uic.HighlightPressedColor (buttonName);
							wrongButtonPressed = false;
						}
					} else {
						// If the button is pushed but is not in buttons to push
						if (Input.GetButtonDown (buttonName)) {
							wrongButtonPressed = true;
							uic.HighlightErrorColor (buttonName);

							break;
						}
					}
				}
				if (wrongButtonPressed) {
					timeToBlock = false;
					hitTaken ();
					return;
				}
				bool allButtonsPressed = true;
				foreach (string buttonName in buttonsToPush) {
					// The button should be down, and pressed during the time to block
					if (!Input.GetButton (buttonName) || !buttonsPushed.Contains (buttonName)) {
						uic.HighlightHint (buttonName);
						allButtonsPressed = false;
					}
				}
				if (allButtonsPressed) {
					hitBlocked ();
				}
			}
		} else if (hitStarted && !pressTooEarly) {
			foreach (string buttonName in possibleButtons) {
				if (Input.GetButtonDown (buttonName)) {
					uic.HighlightErrorColor (buttonName);
					uic.ShowTooEarly ();
					pressTooEarly = true;
					hitTaken ();
					break;
				}
			}
		}
	}

	bool hitBlockedInTime = false;

	void makeHit (Hit hit) {
		uic.ResetToNomalSituation ();
		pressTooEarly = false;
		hitBlockedInTime = false;
		timeToBlock = false;
		wrongButtonPressed = false;
		buttonsToPush = new List<string> (hit.ButtonsName);
		buttonsPushed.Clear ();
		anim.SetTrigger (hit.AnimationName);
		hitStarted = true;
		currentHit = hit;
		foreach (var buttonName in hit.ButtonsName) {
			uic.HighlightHint (buttonName);
		}
		hitCount++;
		if (hitCount >= numHitsToUpgradeDifficulty) 
		{
			hitCount = 0;
			Difficulty += 0.1f;
			anim.SetFloat ("Multiplicateur", Difficulty);
			if (TimeBetweenAttacks >= 2.2f) {
				TimeBetweenAttacks -= 0.2f;
			}
		}
	}

	void hitBlocked () {
		timeToBlock = false;
		hitBlockedInTime = true;
		uic.ShowParade();
		Score += currentHit.Value;
		uic.UpdateScore (Score);
		hitStarted = false;
		playShock ();
	}

	void hitTaken() {
		uic.ShowMissed ();
		Health--;
		uic.UpdateHealth (Health);
		hitStarted = false;
		playHurt ();
		if (Health <= 0) {
			StartCoroutine (DoGameOver ());
		}
	}

	/// <summary>
	/// The animation call this function when it's time to push the buttons to block.
	/// So it's the moment I put colors on UI.
	/// </summary>
	public void BlockStarting () {
		playCry ();
		if (pressTooEarly) {
			return;
		}
		foreach (var buttonName in buttonsToPush) {
			uic.HighlightToPressColor (buttonName);
		}
		timeToBlock = true;
	}

	/// <summary>
	/// The animation call this function when it's to late to push the buttons to block.
	/// </summary>
	public void BlockEnding () {
		timeToBlock = false;
		// If the wrong button is pressed hitTaken was already called
		if (!hitBlockedInTime && !wrongButtonPressed && !pressTooEarly) {
			uic.ShowTooLate ();
			hitTaken ();
		}
	}

	public IEnumerator RandomHits () {
		yield return new WaitForSeconds (2f);
		while (!gameOver) {
			makeHit (Hits [Random.Range(0, Hits.Length)]);
			yield return new WaitUntil ( () => hitStarted);
			yield return new WaitForSeconds (TimeBetweenAttacks);
		}
	}

	IEnumerator ReloadScene () {
		yield return new WaitForSeconds (6f);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void StartGame () {
		Score = 0;
		Health = 3;
		uic.UpdateScore (Score);
		uic.UpdateHealth (Health);
		gameOver = false;
		TimeBetweenAttacks = 4f;
		hitCount = 0;
		Difficulty = 1f;
		StartCoroutine (RandomHits ());
	}

	IEnumerator DoGameOver () {
		gameOver = true;
		uic.ShowGameOver ();
		yield return new WaitForSeconds (4f);
		uic.ResetToNomalSituation ();
		uic.DisplayMenu ();
	}

	void playCry () {
		AudioClip cry = clips [Random.Range (0, clips.Length)];
		audioSource.clip = cry;
		audioSource.pitch = Random.Range (0.95f, 1.2f);
		audioSource.Play ();
	}

	void playShock () {
		AudioClip shock = woodShocks [Random.Range (0, woodShocks.Length)];
		audioSourceHit.clip = shock;
		audioSourceHit.pitch = Random.Range (0.95f, 1.05f);
		audioSourceHit.Play ();
	}

	void playHurt () {
		audioSourceHit.clip = hurt;
		audioSourceHit.pitch = Random.Range (0.95f, 1.05f);
		audioSourceHit.Play ();
	}
}
