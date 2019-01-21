using UnityEngine;
using System.Collections;

public abstract class MovementState : MonoBehaviour {

    protected int animationIndex = -1;
    protected MovementStateMachine stateMachine;

    public virtual void Awake() {
        stateMachine = GetComponent<MovementStateMachine>();
        stateMachine.AddState(this, animationIndex);
    }

    public abstract void OnEnter();

    public virtual bool OnUpdate() {
        return true;
    }

    public virtual void OnExit() {

    }

}
