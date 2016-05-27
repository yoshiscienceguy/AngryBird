using UnityEngine;
using System.Collections;

public class Dragging : MonoBehaviour {
	public float maxStretch = 3.0f;
	public LineRenderer catapultLineFront;
	public LineRenderer catapultLineBack;

	private Transform catapult;
	private SpringJoint2D spring;
	private bool clickedOn;
	private Ray rayToMouse;
	private Ray leftCatapultToProjectile;
	private float maxStretchSqr;
	private Rigidbody2D rb;
	private float circleRadius;
	private Vector2 prevVelocity;

	// Use this for initialization
	void Awake(){
		spring = GetComponent<SpringJoint2D> ();
		rb = GetComponent<Rigidbody2D> ();
		catapult = spring.connectedBody.transform;
	}
	void Start () {
		LineRendererSetup ();
		rayToMouse = new Ray (catapult.position, Vector3.zero);
		leftCatapultToProjectile = new Ray (catapultLineFront.transform.position, Vector3.zero);
		maxStretchSqr = maxStretch * maxStretch;
		CircleCollider2D circle = GetComponent<CircleCollider2D> ();
		circleRadius = circle.radius;
	}
	
	// Update is called once per frame
	void Update () {
		if (clickedOn) {
			Drag ();
		}
		if (spring != null) {
			if (!rb.isKinematic && prevVelocity.sqrMagnitude > rb.velocity.sqrMagnitude) {
				Destroy (spring);
				rb.velocity = prevVelocity;
			}
			if (!clickedOn) {
				prevVelocity = rb.velocity;
			}
			LineRendererUpdate ();
		} else {
			catapultLineBack.enabled = false;
			catapultLineFront.enabled = false;
		}
	}
	void LineRendererSetup(){
		catapultLineFront.SetPosition (0, catapultLineFront.transform.position);
		catapultLineBack.SetPosition (0, catapultLineBack.transform.position);

		catapultLineFront.sortingLayerName = "Foreground";
		catapultLineBack.sortingLayerName = "Foreground";

		catapultLineFront.sortingOrder = 3;
		catapultLineBack.sortingOrder = 1;


	}
	void OnMouseDown(){
		spring.enabled = false;
		rb.isKinematic = false;
		clickedOn = true;
	}
	void OnMouseUp(){
		spring.enabled = true;
		rb.isKinematic = false;
		clickedOn = false;
	}
	void Drag(){
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 catapultToMouse = mouseWorldPoint - catapult.position;
		if (catapultToMouse.sqrMagnitude > maxStretchSqr) {
			rayToMouse.direction = catapultToMouse;
			mouseWorldPoint = rayToMouse.GetPoint (maxStretch);
		}

		mouseWorldPoint.z = 0;
		transform.position = mouseWorldPoint;
	}
	void LineRendererUpdate(){
		Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;
		leftCatapultToProjectile.direction = catapultToProjectile;
		Vector2 holdPoint = leftCatapultToProjectile.GetPoint (catapultToProjectile.magnitude + circleRadius);
		catapultLineFront.SetPosition (1, holdPoint);
		catapultLineBack.SetPosition (1, holdPoint);
	}
}
