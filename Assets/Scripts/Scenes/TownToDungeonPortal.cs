using System.Collections;
using UnityEngine;

public class TownToDungeonPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(ReColliderOn());
            GameManager.Scene.LoadScene("Dungeon");

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