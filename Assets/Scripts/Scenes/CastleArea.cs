using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Sound.PlayMusic("Castle");
            GameManager.Sound.musicSource.loop = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Sound.PlayMusic("Town");
            GameManager.Sound.musicSource.loop = true;
        }
    }
}
