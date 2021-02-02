using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class BezierRenderer : MonoBehaviour
{
    public List<Transform> m_controlPointsTran;
    public LineRenderer m_lineRenderer;
    int m_vertexCount = 12;

    // Update is called once per frame
    void Update()
    {
        var point_list = GetBezierPoins();
        m_lineRenderer.positionCount = point_list.Count;
        m_lineRenderer.SetPositions(point_list.ToArray());
    }
    private void OnDrawGizmos()
    {
        //Draw assistant line
        Gizmos.color = Color.green;
        for(int i = 0; i < m_controlPointsTran.Count -1; i++)
        {
            Gizmos.DrawLine(m_controlPointsTran[i].position, m_controlPointsTran[i+1].position);
        }
        
    }

    private List<Vector3> GetBezierPoins()//points, numpoints, vertex
    {
        var render_points = new List<Vector3>();
        int num_points = m_controlPointsTran.Count;
        int render_idx = 0, jcount;

        for (float ratio = 0; ratio <= 1; ratio += 1.0f / m_vertexCount)
        {
            jcount = 0;
            render_points.Add(Vector3.zero);
            for(int j =0; j != num_points; j++)
            {
                float bernstein_basis = Bernstein(num_points - 1, j, ratio);//comput B(t)
                render_points[render_idx] += bernstein_basis * m_controlPointsTran[jcount].position;
                jcount++;
            }
            render_idx++;

        }
        return render_points;

    }
    /// <summary>
    ///Bernstein formula: accumulate from cuurent pt to last pt
    /// </summary>
    /// <param name="t_lastPointIdx"></param>
    /// <param name="t_curPointIdx"></param>
    /// <param name="t_ratio"></param>
    /// <returns></returns>
    private float Bernstein(int t_lastPointIdx, int t_curPointIdx, float t_ratio)
    {
        float r = (float)Factional(t_lastPointIdx) / (float)(Factional(t_curPointIdx) * Factional(t_lastPointIdx - t_curPointIdx));
        float ti;//t^t_curPointIdx
        float tni;//(1-t)^t_curPointIdx
        ti = ((t_curPointIdx + t_ratio) == 0)? 1.0f : Mathf.Pow(t_ratio, t_curPointIdx);

        if (t_curPointIdx == t_lastPointIdx && t_ratio == 1.0)
            tni = 0.0f;
        else
            tni = Mathf.Pow((1 - t_ratio), (t_lastPointIdx - t_curPointIdx));

        
        return r*ti*tni;
    }
    int Factional(int n)
    {
        if (n >= 1)
            return n * Factional(n - 1);
        else
            return 1;
    }
}
