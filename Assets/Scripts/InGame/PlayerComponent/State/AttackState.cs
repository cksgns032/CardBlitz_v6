using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : MonoBehaviour, IState
{
    Player player;
    Coroutine attackCoroutine;
    PlayerState stateCom;
    List<Player> enemyList;
    Animator ani;
    HeroData stat;
    public AttackState(Player data)
    {
        player = data;
        stateCom = GetComponent<PlayerState>();
        ani = GetComponentInChildren<Animator>();
    }
    public void Enter()
    {
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(IEAttack());
        }
        stat = player.GetStat();
    }

    public void Exit()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    public void Update()
    {
        if (player.IsDie() == true || GameManager.Instance.GetClear())
        {
            return;
        }
        if (player.GetEnemyList().Count <= 0)
        {
            stateCom.TransState(StateType.Move);
        }
    }
    IEnumerator IEAttack()
    {
        if (GameManager.Instance.GetClear())
        {
            Exit();
            yield return null;
        }
        enemyList = player.GetEnemyList();
        while (enemyList.Count > 0)
        {
            Attack();
            yield return stat.attackSpeed;
        }
    }
    public void Attack()
    {
        if (GameManager.Instance.GetClear() && player.IsDie())
        {
            return;
        }
        // 공격 가능 유닛체크 공격
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (i >= stat.attackCnt)
                return;
            else
            {
                ani.SetTrigger("Attack");
                // 타워 공격
                if (enemyList[i].gameObject.tag == "EnemyTower")
                {
                    Team hitTeam = Team.Red == UserData.team ? Team.Blue : Team.Red;
                    GameUI gameUI = GameManager.Instance.GetGameUI();
                    if (gameUI)
                    {
                        gameUI.UpdateTower(hitTeam, stat.attack);
                    }
                }
                // 몬스터 공격
                else if (enemyList[i].gameObject.tag == "Player")
                {
                    Player enemy = enemyList[i].GetComponent<Player>();
                    if (!enemy.IsDie())
                    {
                        enemy.Hit(stat.attack);
                    }
                }
            }
        }
    }
}
