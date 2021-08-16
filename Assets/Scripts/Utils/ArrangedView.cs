using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrangedView
{
    private Vector2 m_highCorner;
    private Vector2 m_lowCorner;
    private int m_width;
    private int m_height;
    private float m_cellWidth;
    private float m_cellHeight;

    public int width
    {
        get
        {
            return m_width;
        }
    }

    public int height
    {
        get
        {
            return m_height;
        }
    }

    public Vector2 highCorner
    {
        get
        {
            return m_highCorner;
        }
    }

    public Vector2 lowCorner
    {
        get
        {
            return m_lowCorner;
        }
    }

    public ArrangedView(int width, int height, Vector2 highCorner, Vector2 lowCorner)
    {
        Initialize(width, height, highCorner, lowCorner);
    }

    public ArrangedView(int width, int height, Camera camera)
    {
        Vector2 highCorner = camera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));

        Vector2 lowCorner = camera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));

        Initialize(width, height, highCorner, lowCorner);
    }

    private void Initialize(int width, int height, Vector2 highCorner, Vector2 lowCorner)
    {
        m_width = width;
        m_height = height;
        m_highCorner = highCorner;
        m_lowCorner = lowCorner;
        m_cellWidth = (m_lowCorner.x - m_highCorner.x) / m_width;
        m_cellHeight = (m_lowCorner.y - m_highCorner.y) / m_height;
    }

    public Vector2 GetPositionInView(int index)
    {
        return new Vector2(
            (index % m_width) * m_cellWidth + m_cellWidth / 2.0f,
            (index / m_width) * m_cellHeight + m_cellHeight / 2.0f) + m_highCorner;
    }
}
