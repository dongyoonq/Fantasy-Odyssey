using System;
using UnityEngine;

public class Scarecrow : Monster, IHitable
{
    Animator animator;

    void Start() 
    {
        currHp = data.maxHp;
        animator = GetComponent<Animator>();
    }
    
    public override void DropItemAndUpdateExp()
    {

    }

    public void Hit(int damage)
    {
        animator.SetBool("Damage", true);
        currHp -= damage;
        GameManager.Ui.SetFloating(gameObject, -damage);

        if (currHp <= 0)
        {
            GetComponent<Collider>().enabled = false;
            animator.SetTrigger("Die");
            Player.Instance.OnChangeKillQuestUpdate?.Invoke(data);
            Destroy(gameObject, 3f);

            spawnInfo.currMonster--;
            int index = Array.IndexOf(spawnInfo.monters, this);
            spawnInfo.spawnPoint[index].state = SpawnPoint.State.Empty;
            spawnInfo.monters[index] = null;
        }
    }

    public void FinishScarecrowDamage()
    {
        animator.SetBool("Damage", false);
    }
}