using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonHulk : MonoBehaviour
{
	public GameObject player;
	public PlayerController playerController;


	void Start()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		playerController = player.GetComponent<PlayerController>();
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		playerController.ChangeCharacter ("Hulk");
	}
}