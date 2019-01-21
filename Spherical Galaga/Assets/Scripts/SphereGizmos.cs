using System;
using UnityEngine;

public class SphereGizmos
{
    public static void DrawMeridian(float theta)
    {
        DrawMeridian(theta, Planet.instance != null ? Planet.instance.Radius : 5);
    }

    public static void DrawMeridian(float theta, float radius)
    {
        var rhoRadius = Mathf.Sin(theta) * radius;
        var z = Planet.instance != null ? Planet.instance.transform.position.z : 0 + Mathf.Cos(theta) * radius;

        var step = Mathf.PI / 16;
        var from = new Vector3(rhoRadius, 0, z);
        for (var r = step; r <= 2 * Mathf.PI + step / 2f; r += step)
        {
            var to = new Vector3(
                rhoRadius * Mathf.Cos(r),
                rhoRadius * Mathf.Sin(r),
                z
            );

            Gizmos.DrawLine(from, to);
            from = to;
        }
    }
}

