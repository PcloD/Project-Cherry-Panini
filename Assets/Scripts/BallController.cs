using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]

public class BallController : MonoBehaviour {

	//Public
	public float moveForce;
	public float jumpForce;

	//Components
	Rigidbody rb;
	SphereCollider sc;

	//Privates
	Vector3 rightMove;
	Vector3 upMove;

	void Awake(){
		//Get references to componenets
		rb = GetComponent<Rigidbody>();
		sc = GetComponent<SphereCollider>();

		//Calculate vectors based off of camera's rotation
		float oldXRotation = Camera.main.transform.eulerAngles.x; //Store old x angle
		Camera.main.transform.eulerAngles = new Vector3(90, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z); //Set it to look straight down 
		rightMove = Camera.main.transform.right.normalized; //Get the right
		upMove = Camera.main.transform.up.normalized; //Get the up
		Camera.main.transform.eulerAngles = new Vector3(oldXRotation, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z); //Set it back to the old angle
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		HandleMovement();
	}

	void HandleMovement(){
		rb.AddForce((Input.GetAxis("Horizontal") * moveForce * rightMove) + (Input.GetAxis("Vertical") * moveForce * upMove));
		if(Input.GetButtonDown("Jump") && IsGrounded()){
			rb.AddForce(Vector3.up * jumpForce);
		}
	}

	bool IsGrounded(){ //Returns true if we are on the ground, false otherwise
		float raycastOrigin = sc.radius * transform.localScale.y - 0.01f; //Get the point directly under the sphere
		float raycastRange = 0.05f; //How far should we do the raycast

		//Perform a raycast hit check for colliders
		RaycastHit[] hits;
		Ray ray = new Ray(transform.position + Vector3.down * raycastOrigin, Vector3.down); //Starts from just below us, points down
		hits = Physics.RaycastAll(ray, raycastRange); //Preform the raycast

		//Debug.DrawLine(transform.position - new Vector3(0, raycastOrigin, 0), transform.position - new Vector3(0, raycastRange, 0)); //Show the linecast we're preforming

		for (int i = 0; i < hits.Length; i++) {
			if(hits[i].collider.tag == "Terrain"){ //If any of the colliders belong to terrain
				return true; //We're on the ground
			}
		}
		return false; //Otherwise, we're not on the ground
	}
}
