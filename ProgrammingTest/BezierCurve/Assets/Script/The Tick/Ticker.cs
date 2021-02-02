using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Assertions;
/// <summary>
/// Implement the Ticker System
/// </summary>
public sealed class Ticker: MonoBehaviour
{
    private static Ticker m_instance;
    private Dictionary<Tickable, Coroutine> m_registeredTickables = new Dictionary<Tickable, Coroutine>();

    public event EventHandler<TickableArgs> TickTime;

    private void Awake()
    {
        m_instance = (Ticker)FindObjectOfType(typeof(Ticker));
        if (m_instance == null)
            m_instance = this as Ticker;
    }
    public static Ticker GetInstance
    {
        get => m_instance;
    }
    public void RegisterTickable(Tickable t_tickable , float t_time)
    {
        Assert.IsNotNull(t_tickable, "Intanciate tickable before registration");
        
       
        Coroutine coroutine_ = StartCoroutine(Tick(t_time));
        m_registeredTickables.Add(t_tickable, coroutine_);
    }
    /// <summary>
    /// Stop tickable and remove it from the list
    /// </summary>
    /// <param name="t_tickable"></param>
    public void StopTickable(Tickable t_tickable)
    {
        Assert.IsNotNull(t_tickable, "Intanciate tickable before deregistration");

        if (m_registeredTickables.ContainsKey(t_tickable))
        {
            StopCoroutine(m_registeredTickables[t_tickable]);
            m_registeredTickables.Remove(t_tickable);
        }
    }
    private IEnumerator Tick(float t_amountTime)
    {
        while (t_amountTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            OnTimeTicked(--t_amountTime);
        }
        Debug.LogWarning("No more registered tickables, stop updating");
    }
    /// <summary>
    /// suscriber: tickable
    /// </summary>
    /// <param name="t_remainingTime"></param>
    private void OnTimeTicked(float t_remainTime)
    {
        if (this.TickTime != null)
        {
            this.TickTime(this, new TickableArgs(t_remainTime));
        }
    }

}

public sealed class Tickable 
{
    private float m_passedTime;

    public void StartTick(float t_amountTime)
    {
        m_passedTime = t_amountTime;
        Ticker.GetInstance.RegisterTickable(this, t_amountTime);
        Ticker.GetInstance.TickTime += this.OnTimeTicked;
    }
    public void StopTick()
    {
        Ticker.GetInstance.StopTickable(this);
        Ticker.GetInstance.TickTime -= this.OnTimeTicked;
    }

    void OnTimeTicked(object t_sender, TickableArgs t_args)
    {
        Debug.Log((t_sender as Ticker).GetInstanceID());
        Debug.Log(t_args.RemainingTimer);
        m_passedTime -= 1;

        if (m_passedTime == 0)
        {
            StopTick();
            Debug.Log(GetHashCode() + "Finish update");
        }
    }

}
public class TickableArgs: EventArgs
{
    public TickableArgs(float t)
    {
        RemainingTimer = t;
    }
    public float RemainingTimer { get; private set; }
}