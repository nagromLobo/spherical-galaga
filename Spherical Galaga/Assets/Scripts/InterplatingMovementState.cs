using UnityEngine;
using System.Collections;

public abstract class InterpolatingMovementState : MovementState {

    public float duration;

    protected SpherePhysics physics;
    protected float startTime;
    protected Vector3 sphericalStartPosition;


    public void Start() {
        physics = GetComponent<SpherePhysics>();
    }

    public override void OnEnter() {
        physics.velocity = Vector3.zero;
        startTime = Time.time;
        sphericalStartPosition = physics.sphericalCoords;
    }

    public override bool OnUpdate() {

        var currTime = Time.time;
        var dt = currTime - startTime;
        var t = dt > duration ? 1.0f : dt / duration;

        physics.sphericalCoords = GetPos(t);

        return dt >= duration;
    }

    protected abstract Vector3 GetPos(float t);
}
