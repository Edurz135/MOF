using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreList : MonoBehaviour
{
    public GameObject listGO;
    public GameObject scoreListPlayerPrefab;

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
}
