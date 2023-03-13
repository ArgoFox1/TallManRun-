using UnityEngine; 
using Random = UnityEngine.Random;
public class Gate : MonoBehaviour
{
    [Header("Properties")]
    public TMPro.TextMeshProUGUI gateTMP;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float distance;
    private int random;
    public GameObject player;
    public Scriptable scr;
    public int number;
    public Color color;
    public Material mat;
    public GameManager gameManager;
    private void Start()
    {
        random = Random.Range(0, gameManager.gates.Count - 5);
        gameManager = GameManager.instance;
        gameObject.name = scr.name;
        if (this.gameObject.name == "Thinner")
        {
            color = Color.red;
            mat.color = color;
            number = Random.Range(25, 75);
            gateTMP.text = $"{number} ↔";
        }
        if(this.gameObject.name == "Thicker")
        {
            color = Color.green;
            mat.color = color;
            number = Random.Range(25, 100);
            gateTMP.text = $"{number} ↔";
        }
        if (this.gameObject.name == "Taller")
        {
            color = Color.green;
            mat.color = color;
            number = Random.Range(25, 100);
            gateTMP.text = $"{number} ↕";
        }
        if (this.gameObject.name == "Shorter")
        {
            color = Color.red;
            mat.color = color;
            number = Random.Range(25, 75);
            gateTMP.text = $"{number} ↕";
        }
    }
    private void Update()
    {

        #region RandomPatroll
        for (int i = 0; i < random; i++)
        {
            float x = gameManager.gates[random].transform.position.x;
            x += Mathf.Sin(Time.time) * distance;
            Vector3 randomPos = new Vector3(x, gameManager.gates[random].transform.position.y, gameManager.gates[random].transform.position.z);
            gameManager.gates[random].transform.position = Vector3.Lerp(gameManager.gates[random].transform.position, randomPos, Time.deltaTime * patrolSpeed);
        }
        #endregion

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Procsess();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameManager.tallCount = 0;
            gameManager.scaleCount = 0;
        }
    }
    // Gate Number Procsess
    public void Procsess()
    {
        if (this.gameObject.name == "Taller")
        {
            gameManager.tallCount += number;
            IncreaseTall();
        }
        if (this.gameObject.name == "Shorter")
        {
            gameManager.tallCount -= number;
            DecreaseTall(gameManager.tallCount / 20);
        }
        if (this.gameObject.name == "Thicker")
        {
            gameManager.scaleCount += number;
            IncreaseScale();
        }
        if (this.gameObject.name == "Thinner")
        {
            gameManager.scaleCount -= number;
            DecreaseScale(gameManager.scaleCount / 20);
        }
    }
    // Object Gets Thinner
    public void DecreaseScale(int value)
    {
        int getScale = gameManager.scaleCount / 15;
        getScale = value;
        if (getScale < 0)
        {
            getScale = -getScale;
        }
        float x = player.transform.localScale.x;
        x -= 0.9f * getScale;
        Vector3 objScale = new Vector3(x, player.transform.localScale.y, player.transform.localScale.z);
        player.transform.localScale = Vector3.Lerp(player.transform.localScale, objScale, Time.deltaTime * 7);
    }
    // Object Gets Thicker
    public void IncreaseScale()
    {
        int getScale = gameManager.scaleCount / 15;
        float x = player.transform.localScale.x;
        x += 0.9f * getScale;
        Vector3 objScale = new Vector3(x, player.transform.localScale.y, player.transform.localScale.z);
        player.transform.localScale = Vector3.Lerp(player.transform.localScale, objScale, Time.deltaTime * 7);
    }
    // Object Goes Short
    public void DecreaseTall(int value)
    {
        int getTall = gameManager.tallCount / 15;
        getTall = value;
        if (getTall < 0)
        {
            getTall = -getTall;
        }
        float y = player.transform.localScale.y;
        y -= 0.9f * getTall;
        Vector3 objScale = new Vector3(player.transform.localScale.x, y, player.transform.localScale.z);
        player.transform.localScale = Vector3.Lerp(player.transform.localScale, objScale, Time.deltaTime * 7);
    }
    // Object Goes Tall
    public void IncreaseTall()
    {
        int getTall = gameManager.tallCount / 15;
        float y = player.transform.localScale.y;
        y += 0.9f * getTall;
        Vector3 objScale = new Vector3(player.transform.localScale.x, y, player.transform.localScale.z);
        player.transform.localScale = Vector3.Lerp(player.transform.localScale, objScale, Time.deltaTime * 7);
    }
}
