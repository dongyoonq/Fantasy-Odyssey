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

    public void Hit(int damamge)
    {
        animator.SetBool("Damage", true);
        currHp -= damamge;

        if (currHp <= 0)
        {
            GetComponent<Collider>().enabled = false;
            animator.SetTrigger("Die");
            Player.Instance.OnChangeKillQuestUpdate?.Invoke();
            Destroy(gameObject, 3f);
        }
    }

    public void FinishScarecrowDamage()
    {
        animator.SetBool("Damage", false);
    }
}