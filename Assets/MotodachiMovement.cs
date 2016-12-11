using UnityEngine;
using System.Collections;

public class MotodachiMovement : MonoBehaviour {

	public float WalkSpeed;
	public float WalkDistance;
	public float MenSpeed;

	Animator anim;

	void Awake () {
		anim = gameObject.GetComponent<Animator> ();
	}

	public void MoveForward () {
		anim.SetTrigger ("MoveForward");
		Vector3 targetPosition = transform.position + (transform.forward * WalkDistance);
		StartCoroutine (move (targetPosition));
	}

	public void Men () {
		anim.SetTrigger ("Men");
		Vector3 targetPosition = transform.position + (transform.forward * WalkDistance);
		StartCoroutine (move (targetPosition));
	}

	IEnumerator move (Vector3 targetPosition) {
		while (transform.position != targetPosition) {
			transform.position = Vector3.Lerp (transform.position, targetPosition, WalkSpeed * Time.deltaTime);
			yield return new WaitForFixedUpdate ();
		}
	}


}
