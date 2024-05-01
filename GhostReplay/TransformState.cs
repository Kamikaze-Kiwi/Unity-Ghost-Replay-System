using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public record TransformState
{
    /// <summary>
    /// This number affects the maximum amount of decimals the float value for position, rotation and scale are recorded as. 
    /// Higher values are more accurate while lower values result in smaller recording files.
    /// </summary>
    private static readonly int roundTo = 3;

    [HideInInspector] public Position position;
    [HideInInspector] public Rotation rotation;
    [HideInInspector] public Scale scale;


    [Serializable]
    public record Position 
    {
        public float x;
        public float y;
        public float z;

        public Position(Vector3 pos)
        {
            x = MathF.Round(pos.x, roundTo);
            y = MathF.Round(pos.y, roundTo);
            z = MathF.Round(pos.z, roundTo);
        }
        
    }

    [Serializable]
    public record Rotation
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public Rotation(Quaternion rot)
        {
            x = MathF.Round(rot.x, roundTo);
            y = MathF.Round(rot.y, roundTo);
            z = MathF.Round(rot.z, roundTo);
            w = MathF.Round(rot.w, roundTo);
        }
    }

    [Serializable]
    public record Scale
    {
        public float x;
        public float y;
        public float z;

        public Scale(Vector3 sca)
        {
            x = MathF.Round(sca.x, roundTo);
            y = MathF.Round(sca.y, roundTo);
            z = MathF.Round(sca.z, roundTo);
        }
    }
}
