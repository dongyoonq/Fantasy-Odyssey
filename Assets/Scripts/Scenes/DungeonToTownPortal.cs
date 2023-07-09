using System.Collections;
using UnityEngine;

public class DungeonToTownPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Sound.PlaySFX("Portal");

            StartCoroutine(ReColliderOn());

            GameManager.Scene.LoadScene("Town");

            Player.Instance.playerController.StopAllCoroutines();
            Player.Instance.controller.enabled = false;
            Player.Instance.transform.position = new Vector3(169f, -15.8f, -281.3f);
        }

        IEnumerator ReColliderOn()
        {
            GetComponent<Collider>().enabled = false;
            yield return new WaitForSeconds(2f);
            GetComponent<Collider>().enabled = true;
        }
    }
}