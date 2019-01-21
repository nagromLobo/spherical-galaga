using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour {

    public UnityAction startIdling;

    public enum EnemyState {Landing, Idle, Moving, TakingOff};
    public EnemyState state;
    private SpherePhysics sp;
    private Rigidbody rb;
    private MovementStateMachine movingStates;

	// Use this for initialization
	void Start () {
        state = EnemyState.Landing;
        sp = GetComponent<SpherePhysics>();
        rb = GetComponent<Rigidbody>();
        sp.gravity = -2f;
        sp.landedOnSphere += TransitionIntoMovingState;
        
        movingStates = GetComponent<MovementStateMachine>();
        startIdling = new UnityAction(TransitionIntoIdleState);
    }

    public void TransitionIntoMovingState() {
        state = EnemyState.Moving;
        movingStates.onMovementComplete += TransitionIntoTakeoffState;
        movingStates.OnStart();
    }

    void TransitionIntoIdleState() {
        state = EnemyState.Idle;
    }

    void TransitionIntoTakeoffState() {
        state = EnemyState.TakingOff;
        movingStates.onMovementComplete -= TransitionIntoTakeoffState;

        sp.gravity = 0f;
        sp.velocity = new Vector3(0, 0, 2);

        Invoke("ScoreMissed", 1f);
    }

    void ScoreMissed()
    {
        var scoreKeeper = GameManager.instance.GetComponent<ScoreKeeper>();
        var enemy = GetComponent<EnemyController>();

        scoreKeeper.IncreaseMissedScore(enemy.pointsWorth);
    }
}
