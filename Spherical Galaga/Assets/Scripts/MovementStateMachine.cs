using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class MovementStateMachine : MonoBehaviour {

    public bool repeats;

    public List<MovementState> states;

    public int currentState = -1;

    public UnityAction onMovementComplete;

    public void Start() {
        currentState = -1;
    }

    public void AddState(MovementState state, int forceIndex = -1) {
        if (forceIndex < 0) {
            states.Add(state); 
        } else {
            states.Insert(forceIndex, state);
        }
    }

    public void OnStart() {
        TriggerNextStateOrExit();
    }


    public void FixedUpdate() {
        if (currentState >= 0 && states[currentState].OnUpdate()) {
            TriggerNextStateOrExit();
        } 
    }

    protected MovementState ToNextState() {
        var nextState = currentState + 1;
        if(nextState >= states.Count && !repeats) {
            return null;
        }
        currentState = nextState % states.Count;
        return states[currentState];
    }

    protected void TriggerNextStateOrExit() {
        var nextState = ToNextState();
        if (nextState) {
            nextState.OnEnter();
        } else {
            if (currentState >= 0 && currentState < states.Count) {
                states[currentState].OnExit();
            }
            StatesEnded();
        }
    }

    protected void StatesEnded() {
        currentState = -1;
        if(onMovementComplete != null) {
            onMovementComplete();
        }
    }

}
