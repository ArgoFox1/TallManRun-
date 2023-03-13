using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [Header("UIs")]

    #region UIS
    public Image PauseMenuImage;
    public Image lostImage;
    public Image wonImage;
    public Text goldText;
    public Text diamondText;
    #endregion   

    [Header("Lists")]

    #region Lists 
    public List<GameObject> diamonds = new List<GameObject>();
    public List<GameObject> golds = new List<GameObject>();
    public List<GameObject> npcs = new List<GameObject>();
    public List<Transform> possies = new List<Transform>();
    public List<GameObject> gates = new List<GameObject>();
    #endregion   

    [Header("Pools")]

    #region Pools
    public List<GameObject> golds2 = new List<GameObject>();
    public List<GameObject> diamonds2 = new List<GameObject>();
    #endregion   

    [Header("Properties")]

    #region Static Variables
    private string _name;
    public AudioSource folder;
    public Camera cam;
    public GameObject player;
    public int diamondCount;
    public int goldCount;
    private int spawnCount;
    public int tallCount;
    public int scaleCount;
    #endregion  

    #region Singleton
    public static GameManager instance;
    #endregion

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        name = SceneManager.GetActiveScene().name;
        folder.Play();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        tallCount = 0;
        scaleCount = 0;
        InvokeRepeating(nameof(SpawnGates), 0, 0.1f);
        InvokeRepeating(nameof(SpawnNPC), 0, 0.2f);
    }
    private  void Update()
    {

        #region UIs

        #region Gems
        goldText.text = goldCount.ToString();
        diamondText.text = diamondCount.ToString();
        #endregion

        #region Lost
        if(player == null || player.gameObject.activeInHierarchy != true)
        {
            if(lostImage.gameObject != null)
            {
                cam.gameObject.SetActive(true);
                StartCoroutine(nameof(CoolDown4LostImage));
            }          
        }
        #endregion

        #region Won
        if(player.GetComponent<MickyMouse>().canFinal == true)
        {
            if(wonImage.gameObject != null)
            {
                cam.gameObject.SetActive(true);
                StartCoroutine(nameof(CoolDown4WonImage));
            }         
        }
        #endregion

        #endregion

    }
    public  void SpawnGates()
    {    
        if (gates.Count != spawnCount)
        {
            spawnCount++;
            int random = Random.Range(0, possies.Count);
            if (gates.Count != spawnCount)
            {
                gates[0].gameObject.SetActive(true);
                gates[spawnCount].gameObject.SetActive(true);
                gates[spawnCount].transform.position = possies[random].transform.position;
            }
            possies.RemoveAt(random);
        }
    }
    public void SpawnNPC()
    {
        for (int i = 0; i < npcs.Count; i++)
        {
            npcs[i].gameObject.SetActive(true);
        }
    }
    public void PlayAgain()
    {
        if(_name == "Level1")
        {
            SceneManager.LoadScene(1);
        }
        if(_name == "Level2")
        {
            SceneManager.LoadScene(2);
        }
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        PauseMenuImage.gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        folder.Pause();
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        PauseMenuImage.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        folder.UnPause();
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(2);
    }
    IEnumerator CoolDown4LostImage()
    {
        yield return new WaitForSeconds(5f);
        lostImage.gameObject.SetActive(true);
    }
    IEnumerator CoolDown4WonImage()
    {
        yield return new WaitForSeconds(1f);
        wonImage.gameObject.SetActive(true);
    }
}
