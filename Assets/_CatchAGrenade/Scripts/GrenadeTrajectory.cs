using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrenadeTrajectory
{
    private readonly List<Vector3> _points;
    private readonly List<float> _pathsSegmentsLengths;

    private Vector3 LastDirection => _points[^1] - _points[^2];

    public float PathLength { get; }

    public GrenadeTrajectory(List<Vector3> points)
    {
        _points = points;
        _pathsSegmentsLengths = new List<float>();
        var pathLength = 0f;

        for (int i = 1; i < _points.Count; i++)
        {
            var distance = Vector3.Distance(_points[i], _points[i - 1]);
            pathLength += distance;
            _pathsSegmentsLengths.Add(distance);
        }

        PathLength = pathLength;
    }

    public Vector3 GetPointAtDistance(float distance)
    {
        if (distance >= PathLength)
            return _points.Last();

        for (int i = 0; i < _pathsSegmentsLengths.Count; i++)
        {
            if (_pathsSegmentsLengths[i] < distance)
            {
                distance -= _pathsSegmentsLengths[i];
            }
            else
            {
                var segmentNormalized = distance / _pathsSegmentsLengths[i];
                return Vector3.Lerp(_points[i], _points[i + 1], segmentNormalized);
            }
        }

        return _points.Last();
    }

    public Vector3 GetDirectionAtDistance(float distance)
    {
        if (distance >= PathLength)
            return LastDirection;

        for (int i = 0; i < _pathsSegmentsLengths.Count; i++)
        {
            if (_pathsSegmentsLengths[i] < distance)
            {
                distance -= _pathsSegmentsLengths[i];
            }
            else
            {
                return _points[i+1] - _points[i];
            }
        }

        return LastDirection;
    }
}