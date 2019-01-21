using UnityEngine;
using System.Collections;

public class MarchInFormation : MonoBehaviour
{
    private SquadMember member;
    private MoveToTarget moveToTarget;

    public float speed = 3f; 

    void Start()
    {
        member = GetComponent<SquadMember>();

        moveToTarget = GetComponent<MoveToTarget>();
        if(moveToTarget == null)
        {
            moveToTarget = gameObject.AddComponent<MoveToTarget>();
        }
    }

    void Update()
    {
        moveToTarget.speed = speed;
        moveToTarget.targetType = MoveToTarget.TargetType.StaticPosition;
        moveToTarget.targetPosition = SpherePhysics.GetWorldPosition(member.formationPosition);
    }
}
