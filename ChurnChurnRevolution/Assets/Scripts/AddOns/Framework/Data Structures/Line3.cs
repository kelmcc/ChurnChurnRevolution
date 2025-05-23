﻿using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// A structure representing a line segment in 3D space. If you want an infinite line (not a segment), use a Ray instead.
    /// </summary>
    [Serializable]
    public struct Line3
    {

        [SerializeField]
        private Vector3 _start;

        [SerializeField]
        private Vector3 _end;

        /// <summary>
        /// A point on the line. The start of the line.
        /// </summary>
        public Vector3 Start => _start;

        /// <summary>
        /// A second point on the line. The end of the line.
        /// </summary>
        public Vector3 End => _end;

        /// <summary>
        /// The point directly between the point A and B.
        /// </summary>
        public Vector3 MidPoint => (_end + _start) * 0.5f;

        /// <summary>
        /// The normalized direction of the line.
        /// </summary>
        public Vector3 Direction => (_end - _start).normalized;

        public Vector3 PerpendicularHorizontal => new Vector3(-Direction.z, 0, Direction.x);
        /// <summary>
        /// The length of the line segment (distance between point A and B).
        /// </summary>
        public float Length => (_end - _start).magnitude;

        /// <summary>
        /// The length of the line segment (distance between point A and B), but squared.
        /// </summary>
        public float LengthSquared => (_end - _start).sqrMagnitude;

        /// <summary>
        /// Half the length of the line segment (distance between point A and B).
        /// </summary>
        public float HalfLength => (_end - _start).magnitude * 0.5f;

        public Vector3 Evaluate(float proportion) => Vector3.Lerp(_start, _end, proportion);

        public Vector3 EvaluateUnclamped(float proportion) =>  _start * (1- proportion) +  _end * proportion ;

        /// <summary>
        /// A line that start at the origin and is aligned with the positive Y axis. Has a length of one.
        /// </summary>
        public static Line3 Up => new Line3(Vector3.zero, Vector3.up);

        /// <summary>
        /// A line that start at the origin and is aligned with the positive Z axis. Has a length of one.
        /// </summary>
        public static Line3 Forward => new Line3(Vector3.zero, Vector3.forward);

        /// <summary>
        /// A line that start at the origin and is aligned with the positive X axis. Has a length of one.
        /// </summary>
        public static Line3 Right => new Line3(Vector3.zero, Vector3.right);

        /// <summary>
        /// Creates a new line from two points.
        /// </summary>
        /// <param name="start">The first point</param>
        /// <param name="end">The second point</param>
        public Line3(Vector3 start, Vector3 end)
        {
            _start = start;
            _end = end;
        }

        public Line3(Vector3 origin, Vector3 direction, float distance)
        {
            _start = origin;
            _end = origin + (direction.normalized * distance);

        }

        /// <summary>
        /// Returns a ray of infinite length that has the same direction as this line segment.
        /// </summary>
        /// <returns>The ray</returns>
        public Ray ToRay()
        {
            return new Ray(_start, Direction);
        }

        /// <summary>
        /// Returns a vector from the beginning of the line to the end.
        /// </summary>
        /// <returns>A vector from the beginning of the line to the end.</returns>
        public Vector3 ToVector()
        {
            return _end - _start;
        }

        public Vector3 GetClosestPointOnFiniteLine(Vector3 point )
        {
            Vector3 line_direction = _end - _start;
            float line_length = line_direction.magnitude;
            line_direction.Normalize();
            float project_length = Mathf.Clamp(Vector3.Dot(point - _start, line_direction), 0f, line_length);
            return _start + line_direction * project_length;
        }
        public Vector3 GetClosestPointOnFiniteLine(Vector3 point , out float projectionLength)
        {
            Vector3 line_direction = _end - _start;
            float line_length = line_direction.magnitude;
            line_direction.Normalize();
            projectionLength = Mathf.Clamp(Vector3.Dot(point - _start, line_direction), 0f, line_length);            
            return _start + line_direction * projectionLength;
        }
        public Vector3 GetClosestPointOnInfiniteLine(Vector3 point)
        {
            Vector3 line_direction = _end - _start; 
            line_direction.Normalize();
            float project_length =  Vector3.Dot(point - _start, line_direction) ;
            return _start + line_direction * project_length;
        }
    }
}
