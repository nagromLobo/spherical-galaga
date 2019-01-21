using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpherePhysics : MonoBehaviour {

    public bool isOnSphere = false;
    public delegate void LandedOnSphere();
    public event LandedOnSphere landedOnSphere;

    public float theta;
    public float rho;
    public float radius;

    public Vector3 velocity;
    public float gravity = 0f;

    public bool onPositiveSide = false;

    private Rigidbody rbody;

    public Vector3 sphericalCoords {
        get {
            return new Vector3(theta, rho, radius);
        }
        set {
            theta = value[0];
            rho = value[1];
            radius = value[2];
        }
    }

    private void Start() {
        rbody = GetComponent<Rigidbody>();

        radius = Vector3.Distance(transform.position, Planet.instance.transform.position);
        theta = Mathf.Acos(transform.position.z / radius);
        rho = !IsAtPole() ? Mathf.Atan2(transform.position.y, transform.position.x) : 0f;
    }
	
	void FixedUpdate () {
        velocity = new Vector3(
            velocity.x,
            velocity.y, 
            velocity.z + gravity * Time.deltaTime
        );

        var deltaPos = velocity * Time.deltaTime;

        var newTheta = theta + deltaPos.y / radius;
        var newRho = rho;

        // did not cross a pole
        var rhoRadius = Mathf.Sin(newTheta) * radius;
        if(!Mathf.Approximately(rhoRadius, 0)) { 
            newRho = rho + deltaPos.x / rhoRadius;
        }

        var groundLevel = Planet.instance.Radius;
        var newRadius = Mathf.Max(radius + deltaPos.z, groundLevel);
        if (newRadius == groundLevel) {
            if (!isOnSphere) {
                isOnSphere = true;
                if (landedOnSphere != null) {
                    landedOnSphere();
                }
            }
        }
        var newPos = GetWorldPosition(newTheta, newRho, newRadius);
       
        rbody.MovePosition(newPos);

        theta = newTheta;
        rho = newRho;
        radius = newRadius;

        var origPos = transform.position;
        onPositiveSide = (Mathf.Abs(newTheta) % (2.0f * Mathf.PI) > Mathf.PI);
        if(newTheta < 0) {
            onPositiveSide = !onPositiveSide;
        }

        var planet = Planet.instance.transform;

        var radiusVector = new Vector3(0, 0, radius);
        var lookingAt = planet.position + (onPositiveSide ?  radiusVector : -radiusVector);
        var up = newPos - planet.position;

        rbody.MoveRotation(Quaternion.LookRotation(up, lookingAt));
    }

    public bool IsAtPole()
    {
        var x = transform.position.x;
        var y = transform.position.y;

        return Mathf.Approximately(x, 0) && Mathf.Approximately(y, 0);
    }

    public bool IsNearPole()
    {
        float absTheta = Mathf.Abs(theta);
        return absTheta % Mathf.PI < 0.1f || absTheta % Mathf.PI > Mathf.PI - 0.1f;
    }

    protected bool IsBetween(float value, float start, float end)
    {
        var min = Mathf.Min(start, end);
        var max = Mathf.Max(start, end);

        return min <= value && max >= value;
    }

    public static Vector3 GetWorldPosition(float theta, float rho, float radius)
    {
        return 
            Planet.instance.transform.position + 
            new Vector3(
                radius * Mathf.Sin(theta) * Mathf.Cos(rho),
                radius * Mathf.Sin(theta) * Mathf.Sin(rho),
                radius * Mathf.Cos(theta)
            );
    }

    public static Vector3 GetWorldPosition(Vector3 sphereCoords)
    {
        return GetWorldPosition(sphereCoords[0], sphereCoords[1], sphereCoords[2]);
    }

    public static Vector3 GetSpherePos(Vector3 worldPos) {
        var isAtPole = Mathf.Approximately(worldPos.x, 0) && Mathf.Approximately(worldPos.y, 0);
        var radius = Vector3.Distance(worldPos, Planet.instance.transform.position);
        var theta = Mathf.Acos(worldPos.z / radius);
        var rho = !isAtPole ? Mathf.Atan2(worldPos.y, worldPos.x) : 0f;
        return new Vector3(theta, rho, radius);
    }

    public static Vector3 AddArc(Vector3 sphereCoords, Vector2 arc)
    {
        var theta = sphereCoords[0];
        var rho = sphereCoords[1];
        var radius = sphereCoords[2];

        var newTheta = theta + arc.y / radius;
        var newRho = rho;

        // did not cross a pole
        var rhoRadius = Mathf.Sin(newTheta) * radius;
        if (!Mathf.Approximately(rhoRadius, 0))
        {
            newRho = rho + arc.x / rhoRadius;
        }

        return new Vector3(newTheta, newRho, radius);
    }

    public static float ArcDistance(Vector3 a, Vector3 b)
    {
        b.z = a.z;

        var aWorld = GetWorldPosition(a);
        var bWorld = GetWorldPosition(b);

        return a.z * Mathf.Acos(Vector3.Dot(a.normalized, b.normalized));
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        var from = GetWorldPosition(0, 0, radius + 5f);
        var to = GetWorldPosition(Mathf.PI, 0, radius + 5f);
        Gizmos.DrawLine(from, to);

        var rhoRadius = Mathf.Sin(theta) * radius;
        var z = Planet.instance.transform.position.z + Mathf.Cos(theta) * radius;

        var step = Mathf.PI / 16;
        from = new Vector3(rhoRadius, 0, z);
        for (var r = step; r <= 2 * Mathf.PI + step / 2f; r += step)
        {
            to = new Vector3(
                rhoRadius * Mathf.Cos(r),
                rhoRadius * Mathf.Sin(r),
                z
            );

            Gizmos.DrawLine(from, to);
            from = to;
        }
    }
}
