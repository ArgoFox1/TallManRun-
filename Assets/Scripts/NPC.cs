using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
namespace Assets.Scripts
{
    public class NPC : MonoBehaviour
    {
        private float timer;
        private Animation anim;
        public GameObject player;
        public Gate gate;
        public Scriptable scr;
        public GameManager gameManager;
        private void Start()
        {
            anim = GetComponent<Animation>();
            gameObject.name = scr.name;
        }
        private void Update()
        {
            if(this.gameObject.name == "Level1Bot")
            {
                anim.Play("Hip Hop Dancing");    
            }
            if(this.gameObject.name == "Boss")
            {
                anim.Stop("Hip Hop Dancing");
                if(player == null || player.gameObject.activeInHierarchy != true)
                {
                    anim.Play("Laughing");
                }
            }
            if(this.gameObject.name == "Level2Bot")
            {
                float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
                if(distance <= 30f)
                {
                    anim.Play("Run");
                    gameObject.GetComponent<NavMeshAgent>().destination = player.transform.position;
                }
            }
            if(player.GetComponent<MickyMouse>().canFinal == true)
            {
                timer += Time.deltaTime;
                if (timer >= 1.5f)
                    anim.Play("Sword And Shield Death");
            }
        }
        private async void OnTriggerEnter(Collider other)
        {
            if(this.gameObject.name != "Boss")
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    await NPCCollison();
                }
            }
        }

        private async Task NPCCollison()
        {
            int random = Random.Range(0, 50);
            gameManager.scaleCount -= random;
            gameManager.tallCount -= random;
            gate.GetComponent<Gate>().DecreaseScale(gameManager.scaleCount / 20);
            gate.GetComponent<Gate>().DecreaseTall(gameManager.tallCount / 20);
            await Task.Delay(100);
            gameManager.scaleCount = 0;
            gameManager.tallCount = 0;
            await Task.Delay(100);
            gameObject.SetActive(false);
            gameManager.npcs.Remove(this.gameObject);
        }
    }  
}
