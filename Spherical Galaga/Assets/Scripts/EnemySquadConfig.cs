using UnityEngine;
using System.Collections.Generic;

// Add a subclass of this to an EnemySquad prefab to configure its members
public abstract class EnemySquadConfig : MonoBehaviour
{
    protected SquadMember InstantiateMember(GameObject enemyPrefab, Vector3 marchingPosition)
    {
        var memberGO = Instantiate(enemyPrefab);

        var member = memberGO.GetComponent<SquadMember>();
        if (member == null)
        {
            member = memberGO.AddComponent<SquadMember>();
        }

        member.spherePosInSwarm = marchingPosition;

        return member;
    }

    public abstract void Configure(EnemySquad squad);
}
