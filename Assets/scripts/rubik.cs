using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * -Lucas Howard
 * this script manages the turning of the cube's parts, rotating the sides in a given direction.
 */


public class rubik : MonoBehaviour {


		//User Inputs

	[Header("Rotating sides")]				//The reason U is separate is to make it easier to look at in the inspector. The same thing applies below.
	public KeyCode	U = KeyCode.UpArrow;
	public KeyCode R = KeyCode.RightArrow, F = KeyCode.RightAlt,
					D = KeyCode.DownArrow, L = KeyCode.LeftArrow, B = KeyCode.LeftAlt;

	public float turnSpeed = 10f;	//gets multiplied by Time.deltaTime

	[Header("Rotating entirety")]
	public KeyCode rotX = KeyCode.X;
	public KeyCode rotY = KeyCode.Y, rotZ = KeyCode.Z;
	[Header("Invert Input")]
	public KeyCode CounterKey = KeyCode.LeftShift;

	//set true or false based on CounterKey. Difference of U or U'. (Determines counter-clockwise, or clockwise)
	public bool invertedInput = false;




		//Background things
	[Header("Collectives, etc.")]


	public List<GameObject> centerEmptys;	//the "empty" gameobjects, which are the parents of the top, down, front, back, left, and right sides of the cube.
	public GameObject[] pieces = new GameObject[54];
	public GameObject parentEmpty;	//the "empty" gameobject to which all pieces are parented.

	//smoothly turning
	public bool somethingIsTurningRightNowSoAnimationBoolean = false;	//as named
	public float Aang = 0f;			//because the local-scoped one only has one a.
	public Vector3 globalV = Vector3.zero;
	public GameObject turningCenterEmpty;



	void Start(){
		pieces = GameObject.FindGameObjectsWithTag ("Piece");
	}



	void Update () {

		if (somethingIsTurningRightNowSoAnimationBoolean) {
			//make turn smooth
			parentEmpty.transform.RotateAround (parentEmpty.transform.position, globalV, Aang * Time.deltaTime);


			if (Mathf.Round (parentEmpty.transform.rotation.eulerAngles.x) % 90 == 0 &&
			    Mathf.Round (parentEmpty.transform.rotation.eulerAngles.y) % 90 == 0 &&
			    Mathf.Round (parentEmpty.transform.rotation.eulerAngles.z) % 90 == 0) {

				somethingIsTurningRightNowSoAnimationBoolean = false;
				Vector3 newRot = new Vector3 (	Mathf.Round (parentEmpty.transform.rotation.eulerAngles.x),
												Mathf.Round (parentEmpty.transform.rotation.eulerAngles.y),
												Mathf.Round (parentEmpty.transform.rotation.eulerAngles.z));
				parentEmpty.GetComponent<Transform> ().eulerAngles = newRot;
			}



		} else {	//when the cube is not rotating in whole or part, the player may input, or the cube may accept an input


			//checking if U or U'
			if (Input.GetKey (CounterKey))
				invertedInput = true;
			if (Input.GetKeyUp (CounterKey))
				invertedInput = false;

			//checking the other player inputs
					//rotating a side
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

					//rotating the cube
			if (Input.GetKeyDown (rotX))
				RotCube (invertedInput, Vector3.left);
			if (Input.GetKeyDown (rotY))
				RotCube (invertedInput, Vector3.down);
			if (Input.GetKeyDown (rotZ))
				RotCube (invertedInput, Vector3.forward);
		}
	}



	public void Rotate(bool isTrue, Vector3 v){
		List<GameObject> selectedSide = getSide (v);
		GameObject foster = new GameObject ();
		foster.transform.position = v;

		if (!isTrue) {
			foreach (GameObject g in selectedSide) {
				g.transform.RotateAround (foster.transform.position, v, turnSpeed);
			}
		} else {
			foreach (GameObject g in selectedSide) {
				g.transform.RotateAround (foster.transform.position, v, -turnSpeed);
			}
		}

		Destroy (foster);
	}


	public void Rotate(string s){		//I know we were talking about passing along strings, so I figured, why not overload? Why not call the other?
		bool isTrue = true;
		Vector3 v = Vector3.zero;


		//is the cube turning? or a side?
		if (s == "x" || s == "x'" ||
		    s == "y" || s == "y'" ||
		    s == "z" || s == "z'") {

			if (s == "x") {
				v = Vector3.left;
			} else if (s == "x'") {
				isTrue = false;
				v = Vector3.left;
			}

			if (s == "y") {
				v = Vector3.down;
			} else if (s == "y'") {
				isTrue = false;
				v = Vector3.down;
			}

			if (s == "z") {
				v = Vector3.forward;
			} else if (s == "z'") {
				isTrue = false;
				v = Vector3.forward;
			}

			RotCube (isTrue, v);



		} else {

			if (s == "U") {
				v = Vector3.up;
			} else if (s == "U'") {
				v = Vector3.up;
				isTrue = false;
			}

			if (s == "F") {
				v = Vector3.back;
			} else if (s == "F'") {
				v = Vector3.back;
				isTrue = false;
			}

			if (s == "R") {
				v = Vector3.right;
			} else if (s == "R'") {
				v = Vector3.right;
				isTrue = false;
			}

			if (s == "D") {
				v = Vector3.down;
			} else if (s == "D'") {
				v = Vector3.down;
				isTrue = false;
			}

			if (s == "L") {
				v = Vector3.left;
			} else if (s == "L'") {
				v = Vector3.left;
				isTrue = false;
			}

			if (s == "B") {
				v = Vector3.forward;
			} else if (s == "B'") {
				v = Vector3.forward;
				isTrue = false;
			}



			Rotate (isTrue, v);
		}
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


	public void RotCube(bool b, Vector3 v) {
		float ang = turnSpeed;
		if (!b)
			ang = -turnSpeed;


		Aang = ang;
		globalV = v;

		//parentEmpty.transform.RotateAround (parentEmpty.transform.position, v, ang);


		//Vector3 newRot = parentEmpty.transform.rotation.eulerAngles + v * ang;		//trying to make it smooth. Didn't work.
		//parentEmpty.transform.rotation = Quaternion.Slerp (parentEmpty.transform.localRotation, Quaternion.Euler (newRot), 7.5f * Time.deltaTime);



		somethingIsTurningRightNowSoAnimationBoolean = true;
	}

}