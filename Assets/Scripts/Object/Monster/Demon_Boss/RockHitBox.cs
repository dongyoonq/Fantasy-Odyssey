using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockHitBox : MonoBehaviour
{
    public DemonBoss owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<IHitable>().Hit(owner.data.meleeMonsterData[0].attackDamage);

            if (gameObject.IsValid())
                GameManager.Resource.Destroy(gameObject);
        }
    }
}
