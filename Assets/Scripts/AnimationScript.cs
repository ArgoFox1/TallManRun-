using UnityEngine;
public class AnimationScript : MonoBehaviour {

    public GameManager gm;
    public bool isAnimated = false;

    public bool isRotating = false;
    public bool isFloating = false;
    public bool isScaling = false;

    public Vector3 rotationAngle;
    public float rotationSpeed;

    public float floatSpeed;
    private bool goingUp = true;
    public float floatRate;
    private float floatTimer;
   
    public Vector3 startScale;
    public Vector3 endScale;

    private bool scalingUp = true;
    public float scaleSpeed;
    public float scaleRate;
    private float scaleTimer;

	void Update () {         
        if(isAnimated)
        {
            if(isRotating && this.gameObject.name == "DeadCam")
            {
                float x = gameObject.transform.rotation.x;
                x = Mathf.Sin(Time.time) * 10f;
                Quaternion rot = Quaternion.Euler(x, gameObject.transform.rotation.y, gameObject.transform.rotation.z);
                gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, rot, Time.deltaTime * rotationSpeed);
            }
            if(isRotating && this.gameObject.name != "DeadCam")
            {
                transform.Rotate(rotationAngle * rotationSpeed * Time.deltaTime);            
            }

            if(isFloating)
            {
                floatTimer += Time.deltaTime;
                if (this.gameObject.tag != "VerTrap" && this.gameObject.tag != "HorTrap")
                {
                    Vector3 moveDir = new Vector3(0.0f, 0.0f, floatSpeed);
                    transform.Translate(moveDir);
                }
                if(this.gameObject.CompareTag("VerTrap"))
                {
                    Vector3 moveDir = new Vector3(0.0f, floatSpeed, 0.0f);
                    transform.Translate(moveDir);
                }
                if (this.gameObject.CompareTag("HorTrap"))
                {
                    Vector3 moveDir = new Vector3(0.0f, floatSpeed, 0.0f);
                    transform.Translate(moveDir);
                }
                if (goingUp && floatTimer >= floatRate)
                {
                    goingUp = false;
                    floatTimer = 0;
                    floatSpeed = -floatSpeed;
                }

                else if(!goingUp && floatTimer >= floatRate)
                {
                    goingUp = true;
                    floatTimer = 0;
                    floatSpeed = +floatSpeed;
                }
            }

            if(isScaling)
            {
                scaleTimer += Time.deltaTime;

                if (scalingUp)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, endScale, scaleSpeed * Time.deltaTime);
                }
                else if (!scalingUp)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, startScale, scaleSpeed * Time.deltaTime);
                }

                if(scaleTimer >= scaleRate)
                {
                    if (scalingUp) { scalingUp = false; }
                    else if (!scalingUp) { scalingUp = true; }
                    scaleTimer = 0;
                }
            }
        }
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (this.gameObject.CompareTag("Gold"))
            {
                gameObject.SetActive(false);
                gm.golds2.Add(this.gameObject);
                gm.golds.Remove(this.gameObject);
                gm.goldCount++;
            }
            if (this.gameObject.CompareTag("Diamond"))
            {
                gameObject.SetActive(false);
                gm.diamonds2.Add(this.gameObject);
                gm.diamonds.Remove(this.gameObject);
                gm.diamondCount++;
            }
        }        
    }
}
