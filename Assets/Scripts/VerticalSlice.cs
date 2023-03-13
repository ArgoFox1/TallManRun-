using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalSlice : MonoBehaviour
{
    private float currentScalex;
    public GameObject verPart;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            VerticalSlicee(other.gameObject);
        }
    }
    private void VerticalSlicee(GameObject g)
    {
        float z = g.gameObject.transform.position.z;
        float x = g.gameObject.transform.position.x;
        float tx = this.gameObject.transform.position.x;
        currentScalex = x - tx;
        if (currentScalex < 0)
            currentScalex = -currentScalex;
        Vector3 otherScale = new Vector3(x / 4 - currentScalex, g.gameObject.transform.localScale.y, g.gameObject.transform.localScale.z);
        g.gameObject.transform.localScale = Vector3.Lerp(g.gameObject.transform.localScale, otherScale, Time.deltaTime * 8f);
        g.gameObject.transform.position = new Vector3(g.gameObject.transform.position.x, g.gameObject.transform.position.y, z - currentScalex);
        // Create sliced part
        SpawnPart(currentScalex);
    }
    public void SpawnPart(float partScalex)
    {
        GameObject part = Instantiate(verPart, gameObject.transform.position, Quaternion.Euler(90,90,0));
        Vector3 pScale = new Vector3(partScalex, partScalex, partScalex);
        part.transform.localScale = pScale;
        part.transform.parent = null;
        if (part.GetComponent<Rigidbody>() == null)
            part.AddComponent<Rigidbody>();
    }
}
