using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public int zombieMax = 10;
	public GameObject zombiePrefab;



	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		var count = GameObject.FindGameObjectsWithTag ("Punchable").Length;
		while (count <= zombieMax) {
			var zombie = Instantiate (zombiePrefab, new Vector3(Random.Range(-13, 13), 0, 0), Quaternion.identity);
			var controller = zombie.GetComponent<ZombieController> ();
			controller.velocity = Random.Range (0.1f, 0.3f);
			count = GameObject.FindGameObjectsWithTag ("Punchable").Length;
		}
	}
}
