using Assets.Scripts;
using System.Collections;
using UnityEngine;
public class MickyMouse : MonoBehaviour
{
    [Header("Properties of Player")]

    #region Static Variables
    [SerializeField] private float sens;
    private float rotX;
    private float timer;
    public GameObject skinMesh;
    public ParticleSystem bombFX;
    public AudioClip bombClip;
    public Vector3 followDistance;
    public Transform headPos;
    public GameObject head;
    public Camera cam;
    public ParticleSystem bloodFX;
    public ParticleSystem confetti;
    public Transform headTop;
    public GameManager gm;
    private AudioSource soundFolder;
    public bool canFinal;
    private bool isGrounded;
    private bool isDying;
    private bool canJump;
    private Rigidbody rb;
    [SerializeField] private float jumpHeight;
    [SerializeReference] private float speed;
    private Animator animator;
    #endregion   

    [Header("Gravity Properties")]

    #region Gravity Variables
    [SerializeField] private float fDistance;
    public LayerMask floorMask;
    public Transform checker;
    #endregion   

    private void Start()
    {
        isDying = false;
        confetti.gameObject.SetActive(false);
        confetti.Stop();
        bloodFX.gameObject.SetActive(false);
        bloodFX.Stop();
        canFinal = false;
        soundFolder = this.gameObject.GetComponent<AudioSource>();
        rb = this.gameObject.GetComponent<Rigidbody>();
        gm = GameManager.instance;
        animator = this.gameObject.GetComponent<Animator>();
    }
    private void Update()
    {        
        #region Movement      
        gameObject.transform.position += transform.forward * Time.deltaTime * speed;
        float x = gameObject.transform.position.x;
        x += Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        Vector3 playerPos = new Vector3(x, gameObject.transform.position.y, gameObject.transform.position.z);
        gameObject.transform.position = playerPos;
        #endregion

        #region Rotation
        Look();
        #endregion

        #region Gravity         
        CheckGround();
        #endregion

        #region Final   
        Final();
        #endregion

        #region Bleeding
        if(canFinal != true)
        {
            if (bloodFX.isPlaying == true)
            {
                DecreaseScaleAndTall();
            }
        }       
        #endregion

        #region Dead
        float scaleY = this.gameObject.transform.localScale.y;
        float scaleX = this.gameObject.transform.localScale.x;
        if(scaleX < 0.5f || scaleY < 0.5f)
        {
            Dead(scaleX,scaleY);
        }
        #endregion

        #region Fall
        if(canFinal != true)
        {
            RaycastHit fHit;
            if (Physics.Raycast(checker.position, checker.TransformDirection(Vector3.down), out fHit, fDistance, floorMask))
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer += Time.deltaTime;
                if (timer >= 3f)
                {
                    StartCoroutine(nameof(CoolDown4Dead));
                }
            }
            if (timer < 0f)
                timer = 0;
        }       
        #endregion
    }
    private  void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HorTrap"))
        {
            Dead(0.4f, 0.4f);
        }
        if (other.gameObject.CompareTag("VerTrap"))
        {
            Dead(0.4f,0.4f);
        }
        if (other.gameObject.CompareTag("Bomb"))
        {
            DecreaseScaleAndTall();
            SpawnFX(other.gameObject.transform);
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.name == "Boss")
        {
            gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Trap"))
        {
            bloodFX.gameObject.SetActive(true);
            bloodFX.Play();
        }
        if (other.gameObject.CompareTag("Diamond"))
        {
            soundFolder.Play();
        }
        if (other.gameObject.CompareTag("Gold"))
        {
            soundFolder.Play();
        }
        if (other.gameObject.CompareTag("Jump"))
        {
            canJump = true;
            rb.AddForce(Vector3.up * jumpHeight);
            animator.SetBool("Jump", true);
            this.gameObject.GetComponent<BoxCollider>().isTrigger = false;
            if (other.gameObject.name == "FinalJump")
            {
                canFinal = true;
                cam.gameObject.SetActive(false);
            }
        }
        else
            animator.SetBool("Jump", false);
    }
    private void DecreaseScaleAndTall()
    {
        if(isDying != true)
        {
            float sY = gameObject.transform.localScale.y;
            float sX = gameObject.transform.localScale.x;
            sY -= Time.deltaTime * 1f;
            sX -= Time.deltaTime * 1f;
            Vector3 scale = new Vector3(sX, sY, gameObject.transform.localScale.z);
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, scale, Time.deltaTime * speed);
        }        
    }

    private void Dead(float scaleX,float scaleY)
    {
        isDying = true;
        if(scaleX < 0.5f)
        {
            scaleX = 0.4f;
        }
        if(scaleY < 0.5f)
        {
            scaleY = 0.4f;
        }
        Destroy(cam.GetComponent<CamFollow>());
        cam.transform.parent = null;
        cam.transform.LookAt(head.transform.position);
        head.transform.localScale = new Vector3(scaleX, scaleX, scaleX);
        head.gameObject.SetActive(true);
        head.transform.parent = null;
        Destroy(this.gameObject.GetComponent<BoxCollider>());
        skinMesh.SetActive(false);
        StartCoroutine(nameof(CoolDown4Dead));
    }

    private void Final()
    {
        if(canFinal == true)
        {
            confetti.gameObject.SetActive(true);
            confetti.Play();
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, headTop.transform.position, Time.deltaTime / 2);
            gameObject.transform.LookAt(headTop.transform.position);
            animator.SetBool("Jump", false);
            animator.SetTrigger("Kick");
        }
    }
    private void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = .75f;
        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance, floorMask))
        {
            isGrounded = true;
            animator.SetBool("Jump", true);
        }
        else
        {
            isGrounded = false;
            if (canFinal != true)
            {
                animator.SetBool("Jump", false);
            }
        }
    }
    private void SpawnFX(Transform spawnPos)
    {
        ParticleSystem fx = Instantiate(bombFX, spawnPos.position, Quaternion.identity);
        fx.gameObject.SetActive(true);
        fx.Play();
        soundFolder.PlayOneShot(bombClip);
    }
    IEnumerator CoolDown4Dead()
    {
        yield return new WaitForSeconds(2f);
        this.gameObject.SetActive(false);
        cam.gameObject.SetActive(false);
    }
    private void Look()
    {
       float x = Input.GetAxis("Horizontal") * Time.deltaTime * sens;
       rotX = x * sens;
       rotX = Mathf.Clamp(rotX, -30f, 30f);
       gameObject.transform.localRotation = Quaternion.Lerp(gameObject.transform.localRotation, Quaternion.Euler(0, rotX, 0), Time.deltaTime * sens);
       this.gameObject.transform.Rotate(Vector3.up * x * sens);
    }
}
