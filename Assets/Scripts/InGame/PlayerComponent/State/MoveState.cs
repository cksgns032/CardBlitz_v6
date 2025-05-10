using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveState : ButtonSearch, IState
{
    NavMeshAgent agent;

    public void Init(Player data)
    {
        player = data;
        stateCom = data.GetState();
        agent = player.gameObject.GetComponent<NavMeshAgent>();
    }
    public void Enter()
    {
        if (agent)
        {
            agent.isStopped = false;
        }
        SetTarget();
    }
    public void Exit()
    {

    }
    public void StateUpdate()
    {
        if (player.IsDie() == true || GameManager.Instance.GetClear())
        {
            return;
        }
    }
    public void SetTarget()
    {
        // 목표 지점 설정 및 이동
        if (agent.enabled == true)
        {
            List<Player> enemyList = player.GetEnemyList();
            if (enemyList.Count > 0)
            {
                agent.SetDestination(enemyList[0].transform.position);
            }
            else
            {
                if (player.gameObject.layer == LayerMask.NameToLayer("HERO"))
                {
                    GameObject obj = GameObject.Find("EnemyGoal");
                    agent.SetDestination(obj.transform.position);
                }
                else if (player.gameObject.layer == LayerMask.NameToLayer("ENEMY"))
                {
                    GameObject obj = GameObject.Find("MyGoal");
                    agent.SetDestination(obj.transform.position);
                }
            }
        }
    }
}
