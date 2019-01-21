using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Directions;

public class WaveMovementState : InterpolatingMovementState {

    public RelativeDirection waveDirection = RelativeDirection.DOWN; // which direction to move int
    public float amplitude = 1.0f;
    public float period = 0.25f;
    public float distance = 1.0f; // value between 0.0f and 1.0f around circle;

    protected override Vector3 GetPos(float t) {
        var primaryMovement = t * distance * 2 *  Mathf.PI;
        var secondaryMovement = Mathf.Sin(primaryMovement / period) * amplitude;
        Vector3 modifyingVector = Vector3.zero;
        switch (waveDirection) {
            case RelativeDirection.LEFT:
                modifyingVector.x = -secondaryMovement;
                modifyingVector.y = -primaryMovement;
                break;
            case RelativeDirection.RIGHT:
                modifyingVector.x = secondaryMovement;
                modifyingVector.y = primaryMovement;
                break;
            case RelativeDirection.UP:
                modifyingVector.x = -primaryMovement;
                modifyingVector.y = -secondaryMovement;
                break;
            case RelativeDirection.DOWN:
                modifyingVector.x = primaryMovement;
                modifyingVector.y = secondaryMovement;
                break;
        }

        return sphericalStartPosition + modifyingVector;
    }
}
