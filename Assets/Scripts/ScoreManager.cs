using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ScoreManager : MonoBehaviourPun {
    public static ScoreManager instance;
    public int player;
    public int totalPoints = 16;
    private PhotonView PV;
    
    public List<int> names;
    public List<int> currentPoints;

    public ScoreList currentPlayerScoreList;

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
            return;
        }
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        //player = PhotonNetwork.player.ID - 1;
		PV.RPC("InitScoreboard", RpcTarget.All);
    }

    public void AddPlayer(int name){
        PV.RPC("RPC_AddPlayer", RpcTarget.All, name);
    }

    [PunRPC]
    void RPC_AddPlayer(int name) {
        names.Add(name);
        currentPoints.Add(0);
        UpdateScoreList();
    }

    public void AddPoint(int name) {
        PV.RPC("RPC_AddPoint", RpcTarget.All, name);
    }

    [PunRPC]
    void RPC_AddPoint(int name) {
        int count = 0;
        for(int i = 0; i < names.Count; i++) {
            if(names[i] == name){
                count = i;
                break;
            }
        }

        currentPoints[count] = currentPoints[count] + 1;
        UpdateScoreList();
        Debug.Log(name + "  kill.");
        if(currentPoints[count] + 1 >= totalPoints) {
            if(currentPlayerScoreList == null){
                Debug.Log("currentPlayerScoreList null");
                return;
            }
            PV.RPC("RPC_SetWinner", RpcTarget.All, name);
        }
    }

    [PunRPC]
    void InitScoreboard() {
        UpdateScoreList();
        totalPoints = 16;
    }

    [PunRPC]
    void RPC_SetWinner(int name) {
        currentPlayerScoreList.Win(name);
    }

    public void UpdateScoreList() {
        if(currentPlayerScoreList == null){
            Debug.Log("currentPlayerScoreList null");
            return;
        }
        currentPlayerScoreList.UpdateScoreList(names, currentPoints);
    }

    void CheckScore() {
        if (currentPoints[player] >= totalPoints) {
            currentPoints[player] = totalPoints;
            Debug.Log("Game Over " + "player " + player + " has Won");
        }
    }
}