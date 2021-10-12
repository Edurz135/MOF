using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ScoreList : MonoBehaviourPunCallbacks
{
    public GameObject listGO;
    public GameObject scoreListPlayerPrefab;
    public GameObject winPanel;
    public TMP_Text winnerTxt;

    public void UpdateScoreList(List<int> names, List<int> scores) {
        // Clean list
        foreach (Transform child in listGO.transform) {
            GameObject.Destroy(child.gameObject);
        }

        if(names.Count == 0) return;

        // Update text
        for(int i = 0; i < names.Count; i++) {
            string text = names[i] + " " + scores[i];
            GameObject listItem = Instantiate(scoreListPlayerPrefab, listGO.transform, false);
            listItem.GetComponent<ScoreListPlayer>().textMP.text = text;
        }
    }

    public void Win(int name) {
        winPanel.active = true;
        winnerTxt.text = name.ToString() + " WON";
    }

    public void ReturnToMainMenu() {
        PhotonNetwork.LoadLevel(0);
        // SceneManager.LoadScene(0, LoadSceneMode.Additive);
    }
}
