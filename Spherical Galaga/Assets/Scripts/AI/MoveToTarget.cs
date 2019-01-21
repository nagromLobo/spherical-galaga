using UnityEngine;
using System.Collections;

public class MoveToTarget : MonoBehaviour
{
    public enum TargetType { StaticPosition, MovingObject }

    public Vector3 targetPosition;
    public GameObject targetObject;
    public TargetType targetType = TargetType.StaticPosition;
    public float speed = 3f;

    private SpherePhysics physics;
    private Rigidbody rbody;

    private bool setAltTarget = false;
    private Vector3 altTarget;

    void Start()
    {
        physics = GetComponent<SpherePhysics>();
        rbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float arcDelta = speed * Time.deltaTime;
        if (Mathf.Approximately(arcDelta, 0f)) return;

        var curr = GetCurrentVector();
        var target = GetTargetVector();

        var step = arcDelta / curr.magnitude;

        var newPos = Vector3.RotateTowards(curr, target, step, 0f);

        if(physics != null)
        {
            physics.sphericalCoords = SpherePhysics.GetSphericalPosition(newPos);
        }
        else if(rbody != null)
        {
            rbody.MovePosition(newPos);
        }
        else
        {
            transform.position = newPos;
        }
    }

    private Vector3 GetCurrentVector()
    {
        var currWorldPos = (physics != null)
                ? SpherePhysics.GetWorldPosition(physics.sphericalCoords)
                : transform.position;

        return currWorldPos - Planet.instance.transform.position;
    }

    private Vector3 GetTargetVector()
    {
        var target = Vector3.zero;

        if (targetType == TargetType.MovingObject)
        {
            if (targetObject == null)
            {
                if(!setAltTarget)
                    return GetCurrentVector();
                else
                {
                    return altTarget;
                }
            }

            target = targetObject.transform.position;

            altTarget = target - Planet.instance.transform.position;
            altTarget.x += Mathf.PI;
            altTarget.y += Random.Range(-Mathf.PI / 16f, Mathf.PI / 16f);

            setAltTarget = true;
        }
        else
        {
            target = targetPosition;
        }

        return target - Planet.instance.transform.position;


    }
}
