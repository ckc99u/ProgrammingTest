using UnityEngine;
using System.Collections;

// In this example showing how to invoke a coroutine and execute

public class ExampleClass : MonoBehaviour
{
    Tickable m_tickable1;
    Tickable m_tickable2;

    private void Start()
    {
        if (!Ticker.GetInstance)
            gameObject.AddComponent<Ticker>();

        m_tickable1 = new Tickable();
        m_tickable2 = new Tickable();
        Invoke("RegisterTickable1", 3.0f);
        Invoke("RegisterTickable2", 5.0f);
    }

    void RegisterTickable1()
    {
        Debug.LogError("register first tickable");
        m_tickable1.StartTick(10);
    }
    void RegisterTickable2()
    {
        Debug.LogError("register second tickable");
        m_tickable2.StartTick(10);
    }
}