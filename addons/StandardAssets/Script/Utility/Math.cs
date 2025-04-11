using Godot;
using System;
using System.Collections.Generic;

namespace Toolbox
{
    // This is great to have for use with [Export] fields
    public enum AxisDirection
    {
        Up,
        Down,
        Left,
        Right,
        Forward,
        Backward
    }

    public static class MathLib
    {
        // Randomly reorders items in place using Fisher-Yates algorithm
        public static void Shuffle<T>(this IList<T> list)
        {
            var rng = new Random();
            for (int i = 0; i < list.Count; i++)
            {
                int j = rng.Next(i, list.Count);
                (list[j], list[i]) = (list[i], list[j]);
            }
        }

        // Returns true only when exactly one input is true
        public static bool XOR3(bool a, bool b, bool c) => (a && !b && !c) || (!a && b && !c) || (!a && !b && c);

        // Returns 1 if target is to the right of forward, -1 if to the left, 0 if aligned
        public static int AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
        {
            Vector3 perp = fwd.Cross(targetDir);
            float dir = perp.Dot(up);

            if (dir > 0)
            {
                return 1;
            }
            else if (dir < 0)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        // Maps value x from range [A,B] to range [C,D]
        public static float Remap(float x, float A, float B, float C, float D)
        {
            return C + (x - A) / (B - A) * (D - C);
        }

        // Returns random choice from list based on integer weights. Returns null if no valid choice found
        public static string Choice(IEnumerable<string> choices, IEnumerable<int> weights)
        {
            var cumulativeWeight = new List<int>();
            int last = 0;
            foreach (var cur in weights)
            {
                last += cur;
                cumulativeWeight.Add(last);
            }
            int choice = new Random().Next(0, last);
            int i = 0;
            foreach (var cur in choices)
            {
                if (choice < cumulativeWeight[i])
                {
                    return cur;
                }
                i++;
            }
            return null;
        }

        // A map of directional vectors used to convert our AxisDirection enum choices
        public static readonly Vector3[] VectorDirection = new Vector3[]
        {
            Vector3.Up,
            Vector3.Down,
            Vector3.Left,
            Vector3.Right,
            Vector3.Forward,
            Vector3.Back,
        };

        // Converts enum choice to corresponding Vector3
        public static Vector3 GetAxisDirection(AxisDirection axis)
        {
            return VectorDirection[(int)axis];
        }

        // Projects direction onto plane defined by normal. Returns normalized result
        public static Vector3 ProjectOnPlane(Vector3 _direction, Vector3 _normal)
        {
            _direction = _direction.Normalized();
            _normal = _normal.Normalized();

            float _mag = _direction.LengthSquared();

            if (Mathf.IsZeroApprox(_mag))
            {
                return _direction;
            }

            float _dot = _direction.Dot(_normal);

            return new Vector3(_direction.X - _normal.X * _dot,
                               _direction.Y - _normal.Y * _dot,
                               _direction.Z - _normal.Z * _dot).Normalized();
        }

        // Rotates vector around Y axis by given angle in radians
        public static Vector3 RotateAroundUp(Vector3 characterForward, float radians)
        {
            float cos = Mathf.Cos(radians);
            float sin = Mathf.Sin(radians);

            Vector3 newForward = new(
                cos * characterForward.X + sin * characterForward.Z,
                characterForward.Y,
                cos * characterForward.Z - sin * characterForward.X
            );

            return newForward;
        }

    }

}

