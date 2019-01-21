using UnityEngine;
using System.Collections;

public class ConfigureSquadAsLine : EnemySquadConfig
{
    public GameObject enemyPrefab;
    public Vector3 positionOfFirstMember;
    public Vector3 directionOfLine;
    public float distanceBetweenMembers = 1f;
    public int numberInSquad = 0;

    public override void Configure(EnemySquad squad)
    {
        var step = directionOfLine.normalized * distanceBetweenMembers;

        if(Mathf.Approximately(step.magnitude, 0))
        {
            string msg = string.Format("Cannot configure squad, direction not configured for {0}", gameObject);
            Debug.LogWarning(msg);
            return;
        }

        var surfaceOffset = new Vector3(0, 0, Planet.instance.Radius);
        Vector3 pos = positionOfFirstMember + surfaceOffset;
        for(var i = 0; i < numberInSquad; i++)
        {
            var member = InstantiateMember(enemyPrefab, pos - surfaceOffset);
            squad.AddMember(member);

            pos = SpherePhysics.AddArc(pos, step);
        }
    }
}
