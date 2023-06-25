using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    public ItemData itemData;

    private void OnEnable()
    {
        transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        ParticleSystem particle = GameManager.Resource.Instantiate<ParticleSystem>("Prefabs/Item/FieldItemParticle", transform);
        particle.transform.localPosition = new Vector3(0.013f, 0.006f, 0.419f);
        particle.transform.localRotation = Quaternion.Euler(20.705f, 49.107f, 22.208f);
        GetComponent<Collider>().enabled = false;
        StartCoroutine(ActiveColliderTimer());
        StartCoroutine(DisappearTimer());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player.Instance.AddItemToInventory(itemData);
            Destroy(gameObject);
        }
    }

    IEnumerator ActiveColliderTimer()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Collider>().enabled = true;
    }

    IEnumerator DisappearTimer()
    {
        yield return new WaitForSeconds(10f);
        if (gameObject.IsValid())
            Destroy(gameObject);
    }
}
