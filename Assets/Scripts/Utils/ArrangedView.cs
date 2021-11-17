using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that implements the grid view organization.
/// </summary>
public class ArrangedView
{
    /// <summary>
    /// Position of the upper left corner of the grid.
    /// </summary>
    private Vector2 m_highCorner;

    /// <summary>
    /// Position of the lower right corner of the grid.
    /// </summary>
    private Vector2 m_lowCorner;

    /// <summary>
    /// Grid's width in cells.
    /// </summary>
    private int m_width;

    /// <summary>
    /// Grid's height in cells.
    /// </summary>
    private int m_height;

    /// <summary>
    /// Cell's width.
    /// </summary>
    private float m_cellWidth;

    /// <summary>
    /// Cell's height.
    /// </summary>
    private float m_cellHeight;

    /// <summary>
    /// Accessor property to the grid's width.
    /// </summary>
    public int width
    {
        get
        {
            return m_width;
        }
    }

    /// <summary>
    /// Accessor property to the grid's height.
    /// </summary>
    public int height
    {
        get
        {
            return m_height;
        }
    }

    /// <summary>
    /// Accessor property to the grid's upper left corner position.
    /// </summary>
    public Vector2 highCorner
    {
        get
        {
            return m_highCorner;
        }
    }

    /// <summary>
    /// Accessor property to the grid's lower right corner position.
    /// </summary>
    public Vector2 lowCorner
    {
        get
        {
            return m_lowCorner;
        }
    }

    /// <summary>
    /// Base constructor.
    /// </summary>
    public ArrangedView(int width, int height, Vector2 highCorner, Vector2 lowCorner)
    {
        Initialize(width, height, highCorner, lowCorner);
    }

    /// <summary>
    /// Constructor that set's the corners' position based on the scene camera.
    /// </summary>
    public ArrangedView(int width, int height, Camera camera)
    {
        Vector2 highCorner = camera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));

        Vector2 lowCorner = camera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));

        Initialize(width, height, highCorner, lowCorner);
    }

    /// <summary>
    /// Mehtod implementing the basic attributes initialization.
    /// </summary>
    private void Initialize(int width, int height, Vector2 highCorner, Vector2 lowCorner)
    {
        m_width = width;
        m_height = height;
        m_highCorner = highCorner;
        m_lowCorner = lowCorner;
        m_cellWidth = (m_lowCorner.x - m_highCorner.x) / m_width;
        m_cellHeight = (m_lowCorner.y - m_highCorner.y) / m_height;
    }

    /// <summary>
    /// Obtain the coordinates inside the grid based on the index of the element
    /// in the grid. The index is assumed to be between 0 and width * height - 1.
    /// </summary>
    /// <param name="index">Index in the grid. It's assumed to be between 0 and width * height - 1.</param>
    public Vector2 GetPositionInView(int index)
    {
        return new Vector2(
            (index % m_width) * m_cellWidth + m_cellWidth / 2.0f,
            (index / m_width) * m_cellHeight + m_cellHeight / 2.0f) + m_highCorner;
    }
}
