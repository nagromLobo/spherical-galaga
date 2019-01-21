using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnSwarmDestroyed(EnemySwarm swarm);

public class EnemySwarm : MonoBehaviour {

    public event OnSwarmDestroyed OnSwarmDestroyed;

    public List<EnemySquad> squads;

    private void Start()
    {
        if (squads == null) squads = new List<EnemySquad>();

        var childSquads = GetComponentsInChildren<EnemySquad>();
        foreach(var squad in childSquads)
        {
            AddSquad(squad);
        }
    }

    public void AddSquad(EnemySquad squad)
    {
        if (squads.Contains(squad)) return;
      
        squads.Add(squad);

        squad.swarm = this;

        squad.OnSquadDestroyed += OnSquadDestroyed;
    }

    protected void OnSquadDestroyed(EnemySquad squad)
    {
        squads.Remove(squad);
        if(squads.Count == 0)
        {
            AllSquadsDestroyed();
        }
    }

    protected void AllSquadsDestroyed()
    {
        if(OnSwarmDestroyed != null)
        {
            OnSwarmDestroyed(this);
        }

        Destroy(gameObject);
    }
}
