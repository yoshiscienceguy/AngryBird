using UnityEngine;
using System.Collections;

public class TargetDamage : MonoBehaviour {

	public int hitPoint = 2;
	public Sprite damagedSprite;
	public float damageImpactSpeed;

	private int currentHitPoints;
	private float damageImpactSpeedSqr;
	private SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		currentHitPoints = hitPoint;
		damageImpactSpeedSqr = damageImpactSpeed * damageImpactSpeed;
	
	}
	
	void OnCollisionEnter2D(Collision2D collision){
		if (collision.collider.tag != "Damager") {
			return;
		}
		if (collision.relativeVelocity.sqrMagnitude < damageImpactSpeedSqr) {
			return;
		}
		spriteRenderer.sprite = damagedSprite;
		currentHitPoints--;
		if (currentHitPoints <= 0) {
			Kill ();
		}
	}
	void Kill(){
		spriteRenderer.enabled = false;
		GetComponent<BoxCollider2D> ().enabled = false;
		GetComponent<Rigidbody2D> ().isKinematic = true;
		GetComponent<ParticleSystem> ().Play (true);
	}
}
