using System;
using UnityEngine;

/// <summary>
/// Class which manages, calculates and stores the game screen coordinates in world space values as a bounding
/// </summary>
public class Limits
{
    private Limits(float l, float r, float t, float b)
    {
        Left = l;
        Right = r;
        Top = t;
        Bottom = b;
    }

    private Limits()
    {
        Left = 0;
        Right = 0;
        Top = 0;
        Bottom = 0;
    }

    public float Left { get; }
    public float Right { get; }
    public float Top { get; }
    public float Bottom { get; }

    public static Limits GetLimits(float edgeOffsetX, float edgeOffsetY)
    {
        if (Camera.main == null)
        {
            Debug.Log("Camera main undefined at this scope.");
            return new Limits();
        }

        Vector3 lowerLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 upperRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        Limits val = new(
            lowerLeft.x + edgeOffsetX,
            upperRight.x - edgeOffsetX,
            upperRight.y - edgeOffsetY,
            lowerLeft.y + edgeOffsetY
        );
        return val;
    }

    public override string ToString()
    {
        return "Screen world limits: Left: " + Left + ", Right: " + Right + ", Top: " + Top + ", Bottom: " + Bottom;
    }

    protected bool Equals(Limits other)
    {
        return Left.Equals(other.Left) && Right.Equals(other.Right) && Top.Equals(other.Top) &&
               Bottom.Equals(other.Bottom);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Left, Right, Top, Bottom);
    }
}