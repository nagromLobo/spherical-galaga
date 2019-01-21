using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Directions;

public class SpherialCirclesMovementState : InterpolatingMovementState {

    public enum RotationalDirection { CLOCKWISE, COUNTER_CLOCKWISE }
    public RotationalDirection rotDir = RotationalDirection.CLOCKWISE;

    public float radius = 10.0f;
    public float distance = 1.0f;
    public RelativeDirection circleDirection = RelativeDirection.RIGHT;

    protected Vector3 startPos;
    protected Vector3 centerNormal;
    protected Vector3 startFoward;
    protected float radiusDegrees;
    protected float radiusRadians;

    protected bool wasPhysicsEnabled;

    public override void OnEnter() {

        base.OnEnter();
        startPos = transform.position;

        switch (circleDirection) {
            case RelativeDirection.UP:
                startFoward = transform.forward;
                break;
            case RelativeDirection.DOWN:
                startFoward = -transform.forward;
                break;
            case RelativeDirection.LEFT:
                startFoward = -transform.right;
                break;
            case RelativeDirection.RIGHT:
                startFoward = transform.right;
                break;
        }

        wasPhysicsEnabled = physics.enabled;
        physics.enabled = false;

    }

    protected override Vector3 GetPos(float t) {
        var centerRotateAxis = Vector3.Cross(startFoward, transform.position).normalized;
        radiusRadians = radius / physics.radius;
        radiusDegrees = Mathf.Rad2Deg * radiusRadians;
        centerNormal = Quaternion.AxisAngle(centerRotateAxis, radiusDegrees) * transform.position;

        float radiansAroundCircle = 2 * Mathf.PI * distance * t;
        float radiansWithDir = rotDir == RotationalDirection.CLOCKWISE ? -radiansAroundCircle : radiansAroundCircle;

        var newPosWithNormalPoles = SpherePhysics.GetWorldPosition(radiusRadians, radiusDegrees, physics.radius);
        Vector3 rotateToCenterAxis = Vector3.Cross(Planet.instance.transform.up, centerNormal);
        var rotateToCircle = Vector3.Angle(Planet.instance.transform.up, centerNormal);
        var newWorldPos = Quaternion.AxisAngle(rotateToCenterAxis, rotateToCircle) * transform.position;
        //return SpherePhysics.GetSpherePos(newWorldPos);
        return Vector3.zero;

    }

    public override void OnExit() {
        physics.enabled = wasPhysicsEnabled;
    }
}
