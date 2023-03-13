using UnityEngine;
namespace Assets.Scripts
{
    public class CamFollow : MonoBehaviour
    {
        public Transform player;
        public Vector3 followDistance;
        private void Update()
        {
            if (player != null)
            {
                gameObject.transform.parent = player.transform;
                gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, followDistance, Time.deltaTime);
            }          
        }           
    }
}
