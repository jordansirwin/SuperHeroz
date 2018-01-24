using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour {

	public GameObject[] characters;

	public AudioClip punch;
	public AudioClip jump;

	private float velocity = 1.0f;
	private float gravity = -0.1f;
	public float jumpVelocity = 2.5f;
	public float jumpHeight = 0.5f;
	private float currentJumpMaxHeight;

	private GameObject activeCharacter;
	private Vector3 moveDirection = Vector3.zero;
	private bool isJumping = false;
	private bool isGrounded = false;
	private Vector3 facing = Vector3.right;
	private Animator animator;
	private new AudioSource audio;

	private new Renderer renderer;

	// Use this for initialization
	void Start () {
		ChangeCharacter ("Batman");
		audio = GetComponent<AudioSource> ();

		SwipeManager.OnSwipeDetected += OnSwipeDetected;
	}

	public void ChangeCharacter(string characterName) {

		foreach (var c in characters) {
			c.SetActive (c.name == characterName);
			if (c.activeSelf) {
				activeCharacter = c;
			}
		}

		renderer = activeCharacter.GetComponent<Renderer> ();
		animator = activeCharacter.GetComponent<Animator> ();
	}

	private bool shouldJump = false;

	private Swipe swipeDir = Swipe.None;
	void OnSwipeDetected (Swipe direction, Vector2 swipeVelocity)
	{
		// do something with direction or the velocity of the swipe
		swipeDir = direction;

		switch (swipeDir) {
		case Swipe.Up:
			shouldJump = true;
			break;
		case Swipe.Left:
			moveDirection.x = -1;
			break;
		case Swipe.Right:
			moveDirection.x = 1;
			break;
		case Swipe.Down:
			moveDirection.x = 0;
			shouldJump = false;
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {

		// get x direction
		moveDirection.x = moveDirection.x * velocity;

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
//			Debug.Log (moveDirection.x);
			animator.SetTrigger ("playerWalk");
		} else {
			animator.SetTrigger ("playerIdle");
		}

		// are we on the ground?
		var len = renderer.bounds.extents.y;
		Debug.DrawRay (transform.position, Vector2.down * len, Color.blue, 1f, false);
		if (!isJumping) {
//			Debug.Log("Player check for on ground");
			var hit = Physics2D.Raycast (transform.position, Vector2.down, len, LayerMask.GetMask ("Ground"));
			if (hit.collider == null) {
				isGrounded = false;
			}
			else {
				// stop y movement and set state as needed
				moveDirection.y = 0f;
				isGrounded = true;
				isJumping = false;
			}
		}

		// are we hitting a wall?
		var hLen = renderer.bounds.extents.x;
		// left
		Debug.DrawRay (transform.position, Vector2.left * len, Color.blue, 1f, false);
		var leftHit = Physics2D.Raycast (transform.position, Vector3.left, hLen, LayerMask.GetMask ("Ground"));
		if (leftHit.collider != null) {
			Debug.Log ("Player hit left wall");
			moveDirection.x = 1f;
		}
		// right
		Debug.DrawRay (transform.position, Vector2.right * len, Color.green, 1f, false);
		var rightHit = Physics2D.Raycast (transform.position, Vector3.right, hLen, LayerMask.GetMask ("Ground"));
		if (rightHit.collider != null) {
			Debug.Log ("Player hit right wall");
			moveDirection.x = -1f;
		}

		//TODO: JUMPING SOMETIMES IS DOUBLE HEIGHT :(

		// did we start jumping?
		//		var startedJump = false;
		if (isGrounded && !isJumping) {
			if(shouldJump) {
//				Debug.Log ("Player should jump");
				isJumping = true;
				isGrounded = false;
				shouldJump = false;

				// calculate the jump height from current position
				currentJumpMaxHeight = transform.position.y + jumpHeight;
				StartCoroutine (Jumping());
				audio.pitch = Random.Range(0.5f, 2f);
				audio.PlayOneShot (jump);
			}
		}

		// are we punching?
//		if (Input.GetKeyDown (KeyCode.Mouse0)) {
		if(Input.GetMouseButtonDown(0)) {
			animator.SetTrigger ("playerPunch");
			audio.pitch = Random.Range(0.5f, 2f);
			audio.PlayOneShot (punch);

			var punchLen = renderer.bounds.extents.x * 1.0f;
			Debug.DrawRay (transform.position, facing * punchLen, Color.red, 1f);
			var hits = Physics2D.RaycastAll (transform.position, facing, punchLen, LayerMask.GetMask ("Default"));
			foreach (var hit in hits) {
				if (hit.collider.CompareTag("Punchable")) {
					if (hit.collider.gameObject != null) {
						Debug.Log ("Punched something!");
						var ZombieController = hit.collider.gameObject.GetComponent<ZombieController> ();
						StartCoroutine (ZombieController.Die ());
					}
				}
			}
		}

		// are we falling and at max jump height?
		if (!isGrounded && !isJumping) {
//			Debug.Log ("Player is falling");
			moveDirection.y += gravity * Time.deltaTime;
		}
			
		transform.Translate(moveDirection.x * Time.deltaTime, moveDirection.y, moveDirection.z);
	}

	IEnumerator Jumping() {
//		Debug.Log ("Jumping...");
		while (isJumping) {
//			Debug.Log ("Jumping... isJumping");
			if (transform.position.y <= currentJumpMaxHeight) {
//				Debug.Log (string.Format ("Jumping... {0} <= {1}", transform.position.y, currentJumpMaxHeight));
				transform.Translate (Vector3.up * jumpVelocity * Time.smoothDeltaTime);
			} else {
//				Debug.Log ("Jumping... start falling");
				isJumping = false;
			}
				
			yield return new WaitForEndOfFrame ();
		} 

		StopCoroutine (Jumping ());
	}
		
}
