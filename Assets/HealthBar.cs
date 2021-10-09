using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HealthBar : MonoBehaviourPun
{
    private Slider slider;
    private PhotonView PV;
    private float desiredHealth;
    private float currentHealth;
    private float lerpSpeed = 10f;

    // Start is called before the first frame update
    private void Awake() {
        slider = GetComponent<Slider>();    
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        slider.value = 1;
        desiredHealth = 1;
        currentHealth = 1;
    }

    private void Update() {
        currentHealth = Mathf.Lerp(currentHealth, desiredHealth, lerpSpeed * Time.deltaTime);
        slider.value = currentHealth;
    }

    public void UpdateHealthBar(float scale) {
        PV.RPC("RPC_UpdateHealthBar", RpcTarget.All, scale);
    }

    [PunRPC]
    public void RPC_UpdateHealthBar(float scale) {
        desiredHealth = scale;
    }

}
