﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float maxSpeed = 3;
	public float speed = 50f;
	public float jumpPwer = 50f;

	public bool grounded;

	private Rigidbody2D rb;
	private Animator anim;
	public Transform groundCheck;

	public int curHealth;
	public int maxHealth = 100;

	void Start(){

		rb = gameObject.GetComponent<Rigidbody2D>();
		anim = gameObject.GetComponent<Animator> ();

		curHealth = maxHealth;


	}

	void Update(){

		grounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));

		anim.SetBool ("grounded" , grounded);
		anim.SetFloat ("airtime", rb.velocity.y);
		anim.SetFloat ("speed", Mathf.Abs (Input.GetAxis ("Horizontal")));

		if (Input.GetAxis ("Horizontal") < -0.1f) {
			transform.localScale = new Vector3 (-1, 1, 1);
		}

		if (Input.GetAxis ("Horizontal") > 0.1f) {
			transform.localScale = new Vector3 (1, 1, 1);
		}

		if (Input.GetButtonDown ("Jump") && grounded) {
			rb.AddForce (Vector2.up * jumpPwer, ForceMode2D.Impulse);
			anim.SetTrigger ("jumping");
		}


		if (curHealth > maxHealth) {

			curHealth = maxHealth;
		}

		if (curHealth <= 0) {
			Die ();
		}
	}

	void FixedUpdate(){

		float h = Input.GetAxis ("Horizontal");
		rb.AddForce ((Vector2.right * speed) * h);

		if (rb.velocity.x > maxSpeed) {
			rb.velocity = new Vector2 (maxSpeed, rb.velocity.y);

		}

		if (rb.velocity.x < -maxSpeed) {
			rb.velocity = new Vector2 (-maxSpeed, rb.velocity.y);
		}

		if (h == 0) {
			rb.velocity = new Vector2 (0, rb.velocity.y);
		}
			
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "mushroom") {
			rb.AddForce (Vector2.up * jumpPwer * 1.5f, ForceMode2D.Impulse);
		}

	}


	/*void OnTriggerEnter2D(Collider2D other){

		//to add if collision from the Top Part of the Wasp. If player touches wasp from sides, it will remove 1 heart.
		Destroy (other.gameObject);
	}
	*/
	void Die(){

		Application.LoadLevel (Application.loadedLevel);
	}


	public void Damage(int dmg){

		curHealth -= dmg;
		gameObject.GetComponent<Animation> ().Play ("Player_Red");

	}

	public IEnumerator Knockback(float knockDur, float knockbackPower, Vector3 knockbackDirection){

		float timer = 0;

		while (knockDur > timer) {

			timer += Time.deltaTime;

			rb.AddForce (new Vector3 (knockbackDirection.x * -100, knockbackDirection.y * knockbackPower, transform.position.z));
		}

		yield return 0;

	}
}
