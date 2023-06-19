using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillCollison : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        int TotalDmg = Player.Instance.Status.AttackPower + 30;
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
            other.gameObject.GetComponent<IHitable>().Hit(TotalDmg);
    }
}
