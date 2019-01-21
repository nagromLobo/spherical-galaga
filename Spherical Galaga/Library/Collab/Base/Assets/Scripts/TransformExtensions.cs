using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions {

    public static Vector3 GetSphericalPosition(this Transform t, Vector3 worldPosition)
    {
        var v = worldPosition - t.position;
        var theta = Mathf.Atan2( Mathf.Sqrt(v.x*v.x + v.y*v.y), v.z);
        var rho = Mathf.Atan2(v.y, v.x);

        return new Vector3(theta, rho, v.magnitude);
    }

    public static Vector3 GetWorldPosition(this Transform t, Vector3 spherical)
    { 
        var theta = spherical.x;
        var rho = spherical.y;
        var r = spherical.z;

        return new Vector3(
            r * Mathf.Sin(rho) * Mathf.Cos(theta),
            r * Mathf.Sin(rho) * Mathf.Sin(theta),
            r * Mathf.Cos(rho)
        );
    }
}
