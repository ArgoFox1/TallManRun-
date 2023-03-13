using UnityEngine;
public class HorizontalSlice : MonoBehaviour
{
    public GameObject playerPart;
    private float currentVelocity;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HorizontalSlicee(other.gameObject);
        }
    }  
    private void HorizontalSlicee(GameObject g)
    {
        float y = g.gameObject.transform.position.y;
        float ty = this.gameObject.transform.position.y;
        currentVelocity = y - ty;
        if (currentVelocity < 0)
            currentVelocity = -currentVelocity;
        Vector3 otherScale = new Vector3(g.gameObject.transform.localScale.x, y / 4 - currentVelocity, g.gameObject.transform.localScale.z);
        g.gameObject.transform.localScale = Vector3.Lerp(g.gameObject.transform.localScale, otherScale, Time.deltaTime * 8f);
        g.gameObject.transform.position = new Vector3(g.gameObject.transform.position.x, y - currentVelocity / 80, g.gameObject.transform.position.z);
        // Creates sliced part
        SpawnPart(currentVelocity);
    }
    private void SpawnPart(float partScalex)
    {
        GameObject part = Instantiate(playerPart, gameObject.transform.position, Quaternion.Euler(90, 90, 0));
        part.transform.parent = null;
        Vector3 pScale = new Vector3(partScalex, part.transform.localScale.y, part.transform.localScale.z);
        part.transform.localScale = Vector3.Lerp(part.transform.localScale, pScale, Time.deltaTime * 8f);
        if (part.GetComponent<BoxCollider>() == null)
            part.AddComponent<BoxCollider>();
    }
}
