using System.Collections.Generic;
using UnityEngine;

public delegate void SquadDestroyed(EnemySquad squad);

public class EnemySquad : MonoBehaviour {

    public event SquadDestroyed OnSquadDestroyed;

    public EnemySwarm swarm;
    public List<SquadMember> members;

    void Start()
    {
        if (members != null) members = new List<SquadMember>();

        Configure();
        //TODO schedule the enemy to "spawn" from somewhere
    }

    public void AddMember(SquadMember newMember)
    {
        if( members.Contains(newMember))
        {
            Debug.LogWarning("Squad already contains this member!");
            return;
        }

        members.Add(newMember);

        //TODO remove from previous squad as safety?
        newMember.squad = this;

        // track the death of this enemy
        var memberHealth = newMember.GetComponent<Health>();
        if (memberHealth != null)
        {
            memberHealth.OnDeath += OnMemberDeath;
        }
    }

    protected void Configure()
    {
        var config = GetComponent<EnemySquadConfig>();
        if (config != null)
        {
            members.Clear();
            config.Configure(this);
        }
    }

    protected void OnMemberDeath(Health health)
    {
        var deadMemberGO = health.gameObject;
        foreach(var member in members)
        {
            if(member.gameObject == deadMemberGO) {
                members.Remove(member);
                break;
            }
        }

        if(members.Count == 0)
        {
            SquadDestroyed();
        }
    }

    protected void SquadDestroyed()
    {
        if(OnSquadDestroyed != null)
        {
            OnSquadDestroyed(this);
            Destroy(this);
        }
    }
}
