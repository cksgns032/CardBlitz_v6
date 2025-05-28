using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : MonoBehaviour, IState
{
    Monster player;
    UserGameData userData;
    PlayerState stateCom;
    List<Unit> enemyList;
    Animator ani;
    HeroData stat;
    NavMeshAgent agent;
    public void Init(Monster data)
    {
        userData = GameManager.Instance.GetMyGameData();
        player = data;
        stateCom = data.GetState();
        ani = player.gameObject.GetComponentInChildren<Animator>();
        agent = player.gameObject.GetComponent<NavMeshAgent>();
    }
    public void Enter()
    {
        Attack();
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
    public void Attack()
    {
        if (GameManager.Instance.GetClear() && player.IsDie())
        {
            return;
        }
        agent.speed = 0;
        agent.isStopped = true;
        // 공격 가능 유닛체크 공격
        enemyList = player.GetEnemyList();
        ani.SetTrigger("Attack");
    }
    public void EndAttackEvent()// 애니메이션 이벤트
    {
        if (player.GetEnemyList().Count <= 0)
        {
            stateCom.TransState(StateType.Move);
        }
        else
        {
            stateCom.TransState(StateType.Idle);
        }
    }
    public void AttackEvent()// 애니메이션 이벤트
    {
        List<Unit> newEnemyList = new List<Unit>();
        for (int i = 0; i < enemyList.Count; i++)
        {
            stat = player.GetStat();
            if (i >= stat.attackCnt)
                return;
            else
            {
                // 타워 공격
                if (enemyList[i].gameObject.tag == "EnemyTower")
                {
                    Team hitTeam = Team.Red == userData.team ? Team.Blue : Team.Red;
                    GameUI gameUI = GameManager.Instance.GetGameUI();
                    if (gameUI)
                    {
                        gameUI.UpdateTower(hitTeam, stat.attack);
                    }
                }
                // 몬스터 공격
                else if (enemyList[i].gameObject.tag == "Player")
                {
                    Monster enemy = enemyList[i].GetComponent<Monster>();
                    if (!enemy.IsDie())
                    {
                        Debug.Log($"Damage : {stat.attack}");
                        enemy.Hit(stat.attack);
                    }
                }
                // if (enemyList[i].IsDie() == false)
                // {
                //     newEnemyList.Add(enemyList[i]);
                // }
            }
        }
        // player.SetEnemyList(newEnemyList);
    }
}
