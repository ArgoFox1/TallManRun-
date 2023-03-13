using UnityEngine;
public class Trap : MonoBehaviour
{
    [SerializeField] private float speed;
    private void Update()
    {
        if (this.gameObject.CompareTag("Trap"))
        {
            Rotate();
        }
    }
    private void Rotate()
    {
        float x = gameObject.transform.position.x;
        x += Time.time * speed;
        Quaternion rotCy = Quaternion.Euler(x, 0, 90);
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, rotCy, Time.deltaTime * speed);
    }
}
