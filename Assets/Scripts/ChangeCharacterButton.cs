using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeCharacterButton : MonoBehaviour
{
	public string characterName;

	private GameObject player;
	private PlayerController playerController;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		playerController = player.GetComponent<PlayerController>();
		var btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		playerController.ChangeCharacter (characterName);
	}
}