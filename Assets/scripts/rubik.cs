using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * -Lucas Howard
 * this script manages the turning of the cube's parts, rotating the sides in a given direction.
 */


public class rubik : MonoBehaviour {


	//list of player/user controls
	public KeyCode	U = KeyCode.UpArrow, R = KeyCode.RightArrow, F = KeyCode.RightAlt,
					D = KeyCode.DownArrow, L = KeyCode.LeftArrow, B = KeyCode.LeftAlt,
					CounterKey = KeyCode.LeftShift;

	//set true or false based on CounterKey. Difference of U or U'. (Determines counter-clockwise, or clockwise)
	public bool invertedInput = false;

	//the "empty" gameobjects, which are the parents of the top, down, front, back, left, and right sides of the cube.
	public List<GameObject> centerEmptys;
	public GameObject[] pieces = new GameObject[54];



	void Start(){
		pieces = GameObject.FindGameObjectsWithTag ("Piece");
	}



	void Update () {
		
		//checking if U or U'
		if (Input.GetKey (CounterKey))
			invertedInput = true;
		if (Input.GetKeyUp (CounterKey))
			invertedInput = false;

		//checking the other player inputs
		if (Input.GetKeyDown (U))
			Rotate (invertedInput, Vector3.up);
		if (Input.GetKeyDown (R))
			Rotate (invertedInput, Vector3.right);
		if (Input.GetKeyDown (F))
			Rotate (invertedInput, Vector3.back);
		if (Input.GetKeyDown (D))
			Rotate (invertedInput, Vector3.down);
		if (Input.GetKeyDown (L))
			Rotate (invertedInput, Vector3.left);
		if (Input.GetKeyDown (B))
			Rotate (invertedInput, Vector3.forward);
	}

	void Rotate(bool isTrue, Vector3 v){
		List<GameObject> selectedSide = getSide (v);
		GameObject foster = new GameObject ();
		foster.transform.position = v;

		if (!isTrue) {
			foreach (GameObject g in selectedSide) {
				g.transform.RotateAround (foster.transform.position, v, 45f);
			}
		} else {
			foreach (GameObject g in selectedSide) {
				g.transform.RotateAround (foster.transform.position, v, -45f);
			}
		}

		Destroy (foster);
	}

	List<GameObject> getSide(Vector3 v){
		List<GameObject> sidePieces = new List<GameObject> ();
		if (v.x != 0f) {	//left or right
			if (v.x > 0f) {		//house right
				foreach (GameObject go in pieces) {
					if (go.transform.position.x > 0.5f)
						sidePieces.Add (go);
				}
			} else {	//house left
				foreach (GameObject go in pieces) {
					if (go.transform.position.x < -0.5f)
						sidePieces.Add (go);
				}
			}
		} else if (v.y != 0f) {	//top or bottom
			if (v.y > 0f) {		//top
				foreach (GameObject go in pieces) {
					if (go.transform.position.y > 0.5f)
						sidePieces.Add (go);
				}
			} else {	//bottom
				foreach (GameObject go in pieces) {
					if (go.transform.position.y < -0.5f)
						sidePieces.Add (go);
				}
			}
		} else {	//front or back
			if (v.z > 0f) {		//back
				foreach (GameObject go in pieces) {
					if (go.transform.position.z > 0.5f)
						sidePieces.Add (go);
				}
			} else {		//front
				foreach (GameObject go in pieces) {
					if (go.transform.position.z < -0.5f)
						sidePieces.Add (go);
				}
			}
		}

		return sidePieces;
	}
}
//chang