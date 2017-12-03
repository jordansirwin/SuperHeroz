using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour {

	public float velocity = 0.1f;
	public AudioClip dead;
	public AudioClip groan;
	public string animationTriggerWalk = "walk";
	public string animationTriggerIdle = "idle";
	public string animationTriggerJump = "jump";
	public string animationTriggerAttack = "attack";
	public string animationTriggerDead = "dead";

	public GameObject[] animationControllers;

	private float gravity = -0.1f;
//	private float jumpVelocity = 2.5f;

	private Vector3 moveDirection = Vector3.zero;
//	private bool isJumping = false;
	private bool isGrounded = false;
	private bool isDead = false;
	private Vector3 facing = Vector3.right;
	private Animator animator;
	private GameObject player;
	private new AudioSource audio;

	//	private Renderer renderer;

	// Use this for initialization
	void Start () {

		player = GameObject.FindWithTag("Player");	
		animator = GetComponent<Animator>();
		audio = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {

		if (isDead) {
			return;
		}
		// are we facing the correct way?
		var localScale = transform.localScale;
		if (moveDirection.x > 0 && facing == Vector3.left) {
			facing = Vector3.right;
			localScale.x *= -1;
		}
		else if(moveDirection.x < 0 && facing == Vector3.right) {
			facing = Vector3.left;
			localScale.x *= -1;
		}
		transform.localScale = localScale;

		if (Mathf.Abs (moveDirection.x) >= 0.5f) {
			Debug.Log (moveDirection.x);
//			animator.SetTrigger (animationTriggerWalk);
		} else {
//			animator.SetTrigger (animationTriggerIdle);
		}

		// are we on the ground?
		var len = GetComponent<Renderer>().bounds.extents.y;
		Debug.DrawRay (transform.position, Vector2.down * len, Color.blue, 1f, false);
//		if (!isGrounded) {// && !startedJump) {
			var hit = Physics2D.Raycast (transform.position, Vector2.down, len, LayerMask.GetMask ("Ground"));
			if (hit.collider == null) {
				isGrounded = false;
			}
			else {
//				Debug.Log ("Enemy on ground");
				moveDirection.y = 0f;
				isGrounded = true;
//				isJumping = false;
			}
//		}

		//TODO: JUMPING SOMETIMES IS DOUBLE HEIGHT :(

		// did we start jumping?
		//		var startedJump = false;
//		if (isGrounded && !isJumping
//			&& Input.GetKeyDown (KeyCode.Space)) {
//			//			startedJump = true;
//			isJumping = true;
//			isGrounded = false;
//			moveDirection.y = jumpVelocity * Time.deltaTime;
//			Debug.Log ("Enemy jumping " + moveDirection.y);
//		}

		// are we punching?
//		if (Input.GetKeyDown (KeyCode.Mouse0)) {
////			animator.SetTrigger (animationTriggerAttack);
//
//			var punchLen = GetComponent<Renderer>().bounds.extents.x * 2;
//			Debug.DrawRay (transform.position, facing * punchLen, Color.red, 3f);
//			var hits = Physics2D.RaycastAll (transform.position, facing, punchLen, LayerMask.GetMask ("Default"));
//			foreach (var hit in hits) {
//				if (hit.collider.CompareTag("Punchable")) {
//					Debug.Log ("Punched something!");
//					var ZombieController = hit.collider.gameObject.GetComponent<ZombieController> ();
//					//					var animator = hit.collider.gameObject.GetComponent<Animator> ();
//					//					animator.SetTrigger ("zombieDead");
//					StartCoroutine(ZombieController.Die());
//					//					yield return new WaitForSeconds(1.0f);
//					//					Destroy (hit.collider.gameObject);
//				}
//			}
//		}

		// are we falling?
		if (!isGrounded) {
			//			Debug.Log ("Player falling");
			moveDirection.y += gravity * Time.deltaTime;
		} else {
			// get x direction
			moveDirection.x = (player.transform.position - transform.position).normalized.x * velocity;
//			Debug.Log (moveDirection);
		}

		transform.Translate(moveDirection.x * Time.deltaTime, moveDirection.y, moveDirection.z);
	}

	void FixedUpdate() {
		if (Random.value < 0.001f) {
			audio.pitch = Random.Range(0.5f, 2f);
			audio.PlayOneShot (groan);
		}
	}

	public IEnumerator Die() {
		if (isDead) {
			yield return null;
		}

		isDead = true;
		animator.SetTrigger (animationTriggerDead);
		audio.pitch = Random.Range(0.5f, 2f);
		audio.PlayOneShot (dead);
		yield return new WaitForSeconds(1.0f);

		if (gameObject != null) {
			Destroy (gameObject);
		}
	}
}
