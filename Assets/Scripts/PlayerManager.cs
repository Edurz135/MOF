using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
	PhotonView PV;
	GameObject controller;
	private int currentPoints = 0;
	
	void Awake()
	{
		PV = GetComponent<PhotonView>();
	}

	void Start()
	{
		if (PV.IsMine) {
			CreateController();
			ScoreManager.instance.currentPlayerScoreList = controller.GetComponent<PlayerController>().scoreList; // Update ScoreList on ScoreManager
        	ScoreManager.instance.AddPlayer(PV.ViewID);
		}
	}

	void CreateController()
	{
		Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
		controller = PhotonNetwork.Instantiate("PlayerController", spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
		
		PlayerController playerController = controller.GetComponent<PlayerController>();
		ScoreManager.instance.currentPlayerScoreList = playerController.scoreList; // Update ScoreList on ScoreManager
		ScoreManager.instance.UpdateScoreList();
		playerController.playerID = PV.ViewID;
	}


	public void Die()
	{
		PhotonNetwork.Destroy(controller);
		CreateController();
	}

	// public void AddPoint(int killerID) {
	// 	this.currentPoints += 1;
    //     // ScoreManager.instance.AddPoint(killerID);
	// 	// controller.GetComponent<PlayerController>().scoreList.UpdateScoreList(ScoreManager.instance.scoreboard);
	// }
}
