using UnityEngine;
using System.Collections;
/// <summary>
/// Interface for the movement
/// </summary>
public interface IMovementStrategy
{
    void Initilize(int t_playerLayer, int t_ignoredLayer);
    void UpdateMovement(in int t_numSensorRays, in float t_angle, in float t_rayRange, in Transform t_transform, in Vector3 t_trgetPosition, ref Vector3 t_deltaPosition);
} 

public class GeneralMover : MonoBehaviour, IMovementStrategy
{
    bool m_bHit = false;
    int m_layerMask;
    public void Initilize(int t_playerLayer, int t_ignoredLayer)
    {
        m_layerMask = 1 << t_ignoredLayer;
        m_layerMask = ~m_layerMask;
        Physics.IgnoreLayerCollision(t_playerLayer, t_ignoredLayer);
    }
    public void UpdateMovement(in int t_numSensorRays, in float t_angle, in float t_rayRange, in Transform t_transform, in Vector3 t_trgetPosition, ref Vector3 t_deltaPosition)
    {
        var current_rot = t_transform.rotation;
        var target_dir = (t_trgetPosition - t_transform.position).normalized;
        m_bHit = false;
        for (int i = 0; i < t_numSensorRays; i++)
        {
            
            var desired_rot_angles = Quaternion.AngleAxis((i / (float)t_numSensorRays) * t_angle * 2 - t_angle, t_transform.up);
            var ray_dir = desired_rot_angles *  current_rot* transform.forward;
            var ray = new Ray(t_transform.position, ray_dir);

            RaycastHit hitInfo;//hitInformation
            
            if (Physics.Raycast(ray, out hitInfo, t_rayRange, m_layerMask))
            {
                t_deltaPosition -= (1.0f / t_numSensorRays) *ray_dir;
                m_bHit = true;
            }
            else
            {
                t_deltaPosition += (1.0f / t_numSensorRays) *ray_dir;//doing path tracing;
            }
        }
        if (!m_bHit)
            t_deltaPosition = target_dir;
    }
}


public class Teleport: MonoBehaviour, IMovementStrategy
{
    //using coroutine, wait amount time then update movement.
    private bool m_bHit = false;
    private int m_layerMask;
    private bool m_bWaiting = true;
    private bool m_bAllowedUpate = false;
    public void Initilize(int t_playerLayer, int t_ignoredLayer)
    {
        m_layerMask = 1 << t_ignoredLayer;
        m_layerMask = ~m_layerMask;
        Physics.IgnoreLayerCollision(t_playerLayer, t_ignoredLayer);
    }

    public void UpdateMovement(in int t_numSensorRays, in float t_angle, in float t_rayRange, in Transform t_transform, in Vector3 t_trgetPosition, ref Vector3 t_deltaPosition)
    {
        if(m_bWaiting)
        {
            Debug.Log("wait to Do something");
            StartCoroutine(WaitASecond());
        }

        if(m_bAllowedUpate)
        {
            //parse value
            ComputeUpdate();
        }
    }
    private IEnumerator WaitASecond()
    {
        m_bWaiting = false;
        yield return new WaitForSeconds(3.0f);
        m_bAllowedUpate = true;
    }
    private void ComputeUpdate()
    {
        Debug.Log("This frame you don't have to wait Do something");
        m_bWaiting = true;
        m_bAllowedUpate = false;
    }
}