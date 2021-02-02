using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Test script, using MonoBehavour updating to count the time and compare with Coroutine.
/// </summary>
public class TickerTest : MonoBehaviour
{
    float m_timer;
    float m_passedTime;
    bool m_bUpdate = true; 

    void Start()
    {
        m_timer = 0.0f;
        StartCoroutine(CountDownTimer(10));
    }

    private void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= 10 && m_bUpdate)
        {
            m_bUpdate = false;
            Debug.LogFormat("UpdateTimeStop {0}", m_timer);
            Debug.LogFormat("UpdateTimeStop finish! current time is: {0}", Time.time);
        }
    }
    private IEnumerator CountDownTimer(float t_amountTime)
    {

        for(float i = 10; i > 0; i-=1)
        {
            m_passedTime += 1;
            yield return new WaitForSeconds(1.0f);
        }
        Debug.LogFormat("CountDownTimer finish! current time is: {0}", Time.time);
        Debug.LogFormat("Passed time in cortoutine {0}", m_passedTime);
    }
}
