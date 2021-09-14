using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeController : MonoBehaviour
{

    public static ScreenShakeController instance;
    private float shakeTimeRemaining, shakePower, shakeFadeTime, shakeRotation;

    public float rotationMultiplier = 7;
    
    void Start()
    {
        instance = this;   
    }

    private void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.O)){
            StartShake(0.2f, 0.2f);
        }
        if(shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float xAmount = Random.Range(-1f, 1f) * shakePower;
            float yAmount = Random.Range(-1f, 1f) * shakePower;

            transform.position += new Vector3(xAmount, yAmount, 0f);

            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);

            shakeRotation = Mathf.MoveTowards(shakeRotation, 0f, shakeFadeTime * rotationMultiplier * Time.deltaTime);
            
            transform.rotation = Quaternion.Euler(0f, 0f, shakeRotation * Random.Range(-1f, 1f));
        }else{
            if(Vector2.Distance(transform.localPosition, new Vector2(0, 0)) < 0.05f){
                transform.localPosition = new Vector3(0f, 0f, -10f);
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }else{
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 0, -10f), Time.deltaTime * 10f);
                float z_factor = Mathf.Lerp(transform.rotation.z, 0f, Time.deltaTime * 10f);
                transform.rotation = Quaternion.Euler(0f, 0f, z_factor);
            }
        }

    }

    public void StartShake(float length, float power)
    {
        shakeTimeRemaining = length;
        shakePower = power;

        shakeFadeTime = power / length;

        shakeRotation = power * rotationMultiplier;
    }
}
