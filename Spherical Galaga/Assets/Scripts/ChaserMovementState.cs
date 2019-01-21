using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserMovementState : MovementState {

    public MoveToTarget toTarget;
    public float timeSpentChasing = 60f;

    private float elapsedTime = 0f;

    public override void Awake() {
        base.Awake();
        toTarget = GetComponent<MoveToTarget>();
        toTarget.targetType = MoveToTarget.TargetType.MovingObject;
    }

    // Use this for initialization
    public override void OnEnter() {
        toTarget.enabled = true;
        toTarget.targetObject = GameManager.instance.Player;
	}

    public override bool OnUpdate() {
        toTarget.targetObject = GameManager.instance.Player;

        elapsedTime += Time.deltaTime;
        return elapsedTime >= timeSpentChasing;
    }

    // Update is called once per frame
    public override void OnExit() {
        toTarget.enabled = false;
	}
}
