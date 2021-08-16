using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrangedViewVisualizer : MonoBehaviour
{
    [SerializeField] private int m_width;
    [SerializeField] private int m_height;
    [SerializeField] private Camera m_camera;
    private ArrangedView m_arrangedView;

    private void Start()
    {
        m_arrangedView = new ArrangedView(m_width, m_height, m_camera);
    }

    private void OnDrawGizmos()
    {
        if (m_arrangedView == null) return;

        for (int i = 0; i < m_width * m_height; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(m_arrangedView.GetPositionInView(i), new Vector3(1, 1, 1));

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(new Vector3(m_arrangedView.highCorner.x, m_arrangedView.highCorner.y, 0), 0.25f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(new Vector3(m_arrangedView.lowCorner.x, m_arrangedView.lowCorner.y, 0), 0.25f);
        }
    }
}
