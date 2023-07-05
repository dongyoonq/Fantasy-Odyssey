using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrageIceHitBox : MonoBehaviour
{
    public Phantom owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<IHitable>().Hit(owner.data.rangeMonsterData[0].attackDamage);

            if (gameObject.IsValid())
                GameManager.Resource.Destroy(gameObject);
        }
    }
}
