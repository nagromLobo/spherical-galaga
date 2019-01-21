using UnityEngine;
using System.Collections;

public delegate void FormationStatusChanged(SquadMember member);

// Marker interface for squadies
public class SquadMember : MonoBehaviour
{
    private const float Epsilon = 0.1f;

    public event FormationStatusChanged OnFormationStatusChanged;

    public Vector3 spherePosInSwarm;
    public EnemySquad squad;

    private SpherePhysics physics;
    private SpherePhysics swarmPhysics;

    public bool isInFormation { get; private set; }

    public Vector3 formationPosition
    {
        get
        {
            if (squad == null)
            {
                if (physics != null)
                {
                    return physics.sphericalCoords;
                }
            }
            else if (squad.swarm != null)
            {
                if(swarmPhysics == null)
                {
                    swarmPhysics = squad.swarm.GetComponent<SpherePhysics>();
                }

                return swarmPhysics.sphericalCoords + spherePosInSwarm;
            }

            return new Vector3(0, 0, Planet.instance.Radius);
        }
    }

    public Vector3 swarmVelocity
    {
        get
        {
            if(squad == null && squad.swarm != null) {
                if(swarmPhysics == null)
                {
                    swarmPhysics = squad.swarm.GetComponent<SpherePhysics>();
                }

                return swarmPhysics.velocity;
            }

            return Vector3.zero;
        }
    }

    //TODO does this make sense?
    void TriggerWhenInFormation(FormationStatusChanged handler)
    {
        if(isInFormation)
        {
            handler(this);
        }
        else
        {
            FormationStatusChanged internalHandler = null;

            internalHandler = (member) =>
            {
                if (member.isInFormation)
                {
                    handler(member);
                    OnFormationStatusChanged -= internalHandler;
                }
            };

            OnFormationStatusChanged += internalHandler;
        }
    }

    void Start()
    {
        physics = GetComponent<SpherePhysics>();
    }

    void Update()
    {
        var wasInFormation = isInFormation;
        var swarmWorldPos = SpherePhysics.GetWorldPosition(formationPosition);

        isInFormation = Vector3.Distance(swarmWorldPos, transform.position) <= Epsilon;

        if(wasInFormation != isInFormation)
        {
            if(OnFormationStatusChanged != null)
            {
                OnFormationStatusChanged(this);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (isInFormation) return;

        Gizmos.color = Color.white;

        var swarmWorldPos = SpherePhysics.GetWorldPosition(formationPosition);
        Gizmos.DrawWireSphere(swarmWorldPos, Epsilon);

        var radius = Planet.instance.Radius;
        var center = Planet.instance.transform.position;

        //translate to origin
        var toStart = Vector3.Normalize(transform.position - center);
        var toEnd = Vector3.Normalize(swarmWorldPos - center);

        var times = 0;
        var point = toStart;
        while ( Vector3.Distance(point, toEnd) > 0.1f && times < 16)
        {
            var nextPoint = Vector3.RotateTowards(point, toEnd, Mathf.PI / 16f, 0f);
            Gizmos.DrawLine(
                point * radius + center,
                nextPoint * radius + center
            );

            point = nextPoint;
            times++;
        }
    }
}
