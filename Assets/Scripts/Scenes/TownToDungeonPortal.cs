using System.Collections;
using UnityEngine;

public class TownToDungeonPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Sound.PlaySFX("Portal");

            StartCoroutine(ReColliderOn());
            
            GameManager.Scene.LoadScene("Dungeon");

            Player.Instance.playerController.StopAllCoroutines();
            Player.Instance.controller.enabled = false;
            Player.Instance.transform.position = new Vector3(3.06f, 9.07f, -13.49f);
        }

        IEnumerator ReColliderOn()
        {
            GetComponent<Collider>().enabled = false;
            yield return new WaitForSeconds(2f);
            GetComponent<Collider>().enabled = true;
        }
    }
}