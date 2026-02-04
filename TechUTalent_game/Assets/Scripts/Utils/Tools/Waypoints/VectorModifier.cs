using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class VectorModifier
{
    public static List<Vector3> Simplify(List<Vector3> points, float tolerance)
    {
        var end = points.Count - 1;
        var keep = new bool[end + 1];
        for (int i = 0; i <= end; i++)
        {
            keep[i] = true;
        }

        SimplifyRecursive(points, 0, end, tolerance, ref keep);

        var simplifiedPoints = new List<Vector3>();
        for (int i = 0; i <= end; i++)
        {
            if (keep[i])
            {
                simplifiedPoints.Add(points[i]);
            }
        }

        return simplifiedPoints;
    }

    private static void SimplifyRecursive(List<Vector3> points, int start, int end, float tolerance, ref bool[] keep)
    {
        if (end <= start + 1) return;
        
        var maxDistance = 0f;
        var farthestIndex = 0;

        var startPt = points[start];
        var endPt = points[end];

        for (int i = start + 1; i < end; i++)
        {
            var distance = Vector3.Distance(Vector3.Project(points[i] - startPt, endPt - startPt) + startPt, points[i]);
            if (!(distance > maxDistance)) continue;
            
            maxDistance = distance;
            farthestIndex = i;
        }

        if (maxDistance > tolerance)
        {
            keep[farthestIndex] = true;
            SimplifyRecursive(points, start, farthestIndex, tolerance, ref keep);
            SimplifyRecursive(points, farthestIndex, end, tolerance, ref keep);
        }
        else
        {
            for (int i = start + 1; i < end; i++)
            {
                keep[i] = false;
            }
        }
    }
    
    public static List<Vector3> SmoothPoints(List<Vector3> points, float smoothness)
    {
        var smoothedPoints = new List<Vector3>();
        var totalDistance = 0f;
        var distances = new float[points.Count];

        var length = points.Count;
        totalDistance = CalculateTotalDistance(points, length, totalDistance, distances);

        var segmentLength = totalDistance / (smoothness * (points.Count - 1));

        smoothedPoints.Add(points[0]);
        var currentPoint = 0;
        var currentDistance = 0f;

        var l = smoothness * (points.Count - 1);
        SmoothCalculation(points, l, segmentLength, distances, currentPoint, currentDistance, smoothedPoints);

        smoothedPoints.Add(points[^1]);
        return smoothedPoints;
    }

    private static void SmoothCalculation(List<Vector3> points, float l, float segmentLength, float[] distances, int currentPoint, float currentDistance, List<Vector3> smoothedPoints)
    {
        for (int i = 1; i < l; i++)
        {
            var targetDistance = i * segmentLength;

            while (distances[currentPoint + 1] < targetDistance)
            {
                currentDistance = distances[currentPoint + 1];
                currentPoint++;
            }

            var point0 = points[Mathf.Max(currentPoint - 1, 0)];
            var point1 = points[currentPoint];
            var point2 = points[Mathf.Min(currentPoint + 1, points.Count - 1)];
            var point3 = points[Mathf.Min(currentPoint + 1, points.Count - 1)];
            var distance = (targetDistance - currentDistance) / (distances[currentPoint + 1] - currentDistance);
            var newPos = 0.5f * ((2 * point1) +
                                 (-point0 + point2) * distance +
                                 (2 * point0 - 5 * point1 + 4 * point2 - point3) * (distance * distance) +
                                 (-point0 + 3 * point1 - 3 * point2 + point3) * (distance * distance * distance));

            smoothedPoints.Add(newPos);
        }
    }

    private static float CalculateTotalDistance(List<Vector3> points, float length, float totalDistance, float[] distances)
    {
        for (int i = 1; i < length; i++)
        {
            var distance = Vector3.Distance(points[i - 1], points[i]);
            totalDistance += distance;
            distances[i] = totalDistance;
        }

        return totalDistance;
    }
}