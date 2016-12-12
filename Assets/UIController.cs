using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour {

	public Color DefaultColor;
	public Color HintColor;
	public Color ToPressColor;
	public Color PressedColor;
	public Color ErrorColor;

	public GameObject Missed;
	public GameObject Parade;

	public GameObject TooLate;
	public GameObject TooEarly;
	public GameObject GameOver;

	public GameObject KeyboardText;
	public GameObject AzertyButton;
	public GameObject QwertyButton;
	public GameObject PubText;
	public GameObject Menu;

	public Text ScoreText;
	public Text HealthText;

	public Text ArmsUp;
	public Text ArmsDown;
	public Text ArmsLeft;
	public Text ArmsRight;
	public Text LegsUp;
	public Text LegsDown;
	public Text LegsLeft;
	public Text LegsRight;

	public GameController gc;

	Dictionary<string, Text> buttonNameToText;

	void Awake () {
		buttonNameToText = new Dictionary<string, Text> ();
		buttonNameToText.Add ("Arms.Up", ArmsUp);
		buttonNameToText.Add ("Arms.Down", ArmsDown);
		buttonNameToText.Add ("Arms.Left", ArmsLeft);
		buttonNameToText.Add ("Arms.Right", ArmsRight);
		buttonNameToText.Add ("Legs.Up", LegsUp);
		buttonNameToText.Add ("Legs.Down", LegsDown);
		buttonNameToText.Add ("Legs.Left", LegsLeft);
		buttonNameToText.Add ("Legs.Right", LegsRight);


	}

	public void HighlightHint(string buttonName) {
		buttonNameToText[buttonName].color = HintColor;
	}

	public void HighlightToPressColor(string buttonName) {
		buttonNameToText[buttonName].color = ToPressColor;
	}

	public void HighlightPressedColor(string buttonName) {
		buttonNameToText[buttonName].color = PressedColor;
	}

	public void HighlightErrorColor(string buttonName) {
		buttonNameToText[buttonName].color = ErrorColor;
	}

	public void ResetColors () {
		foreach (var pair in buttonNameToText) {
			pair.Value.color = DefaultColor;
		}
	}

	public void ShowMissed () {
		Missed.SetActive(true);
	}

	public void HideMissed () {
		Missed.SetActive(false);
	}

	public void ShowParade () {
		Parade.SetActive(true);
	}

	public void HideParade () {
		Parade.SetActive(false);
	}

	public void ShowTooLate () {
		TooLate.SetActive (true);
	}

	public void HideTooLate () {
		TooLate.SetActive (false);
	}

	public void ShowTooEarly () {
		TooEarly.SetActive (true);
	}

	public void HideTooEarly () {
		TooEarly.SetActive (false);
	}

	public void ResetToNomalSituation () {
		HideMissed ();
		HideParade ();
		HideTooLate ();
		HideTooEarly ();
		ResetColors ();
		HideGameOver ();
	}

	public void UpdateScore (int score) {
		ScoreText.text = "Score : " + score;
	}

	public void UpdateHealth (int health) {
		HealthText.text = "Health : " + health;
	}

	public void ShowGameOver () {
		GameOver.SetActive(true);
		PubText.SetActive (true);
	}

	public void HideGameOver () {
		GameOver.SetActive(false);
		PubText.SetActive (false);
	}

	public void AzertyButtonPressed () {
		HideInputDemand ();
		DisplayMenu ();
	}

	public void QwertyButtonPressed () {
		HideInputDemand ();
		LegsUp.text = "W";
		LegsDown.text = "S";
		LegsLeft.text = "A";
		LegsRight.text = "D";
		DisplayMenu ();
	}

	public void DisplayMenu () {
		Menu.SetActive (true);
	}

	public void StartGame () {
		HideMenu ();
		gc.StartGame ();
	}

	public void HideInputDemand () {
		QwertyButton.SetActive (false);
		AzertyButton.SetActive (false);
		KeyboardText.SetActive (false);
	}

	public void HideMenu () {
		PubText.SetActive(false);
		Menu.SetActive(false);
	}
}
