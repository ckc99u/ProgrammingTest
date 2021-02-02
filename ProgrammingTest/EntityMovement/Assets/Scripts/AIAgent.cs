using UnityEngine;
/*public enum MovementType
{
    Walk,
    Fly,
    Teleport
}*/
public sealed class AIAgent : MonoBehaviour
{

    public float m_movingSpeed = 5.0f;
    public Transform m_targetPos;
    [SerializeField]
    private int m_playerLayerNum = 3;
    [SerializeField]
    private int m_ignoredLayerNum =6;

    private int m_numSensorRays = 5;
    private float m_angle = 90f;
    private float m_rayRange = 3.0f;

    private IMovementStrategy m_movingMethod;

    void Start()
    {
        m_movingMethod = gameObject.AddComponent<GeneralMover>();
        m_movingMethod.Initilize(m_playerLayerNum, m_ignoredLayerNum);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(m_targetPos.position, transform.position) > 1.0f)
        {
            var deltaPosition = Vector3.zero;
            m_movingMethod.UpdateMovement(m_numSensorRays, m_angle, m_rayRange, this.transform, m_targetPos.position, ref deltaPosition);
            gameObject.transform.position += deltaPosition * Time.deltaTime * m_movingSpeed;
        }
    }


    /// <summary>
    /// Draw assistant Rays
    /// </summary>
    private void OnDrawGizmos()
    {
        for (int i = 0; i < m_numSensorRays; i++)
        {
            var current_rot = this.transform.rotation;
            var desired_rot = Quaternion.AngleAxis((i / (float)m_numSensorRays) * m_angle * 2 - m_angle, transform.up);
            var dir = desired_rot * current_rot * Vector3.forward;
            Gizmos.DrawRay(transform.position, dir);

        }
    }
}
