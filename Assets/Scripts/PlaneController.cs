using UnityEngine;

public enum PlaneColor
{
    RedPlane,
    BluePlane,
    YellowPlane,
    GreenPlane,
}

public class PlaneController : MonoBehaviour
{

    public PlaneColor planeColor = PlaneColor.RedPlane;
    [Space(8f)]

    [SerializeField]
    GameObject player;

    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    float forceAmount;
    [SerializeField]
    float rotateAmount = 2f;

    [SerializeField] LayerMask collisionLayer;
    [SerializeField] LayerMask coinLayer;

    [Space]
    [Header("Read Only")]
    [SerializeField]
    float yVelocity;

    //Private 
    Animator animator;
    Transform playerTranform;
    Vector3 currentEularAngle;
    Vector3 newEularAngle;
    bool tapInput;



    private void OnEnable()
    {

        player = transform.GetChild(0).gameObject;
        playerTranform = player.transform;
        animator = player.GetComponent<Animator>();


        rb = GetComponent<Rigidbody2D>();


        animator.Play(planeColor.ToString());



    }

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            GetInput();
        }

    }
    private void FixedUpdate()
    {
        AddForce();
        SetPlayerRotation();
    }

    void GetInput()
    {
#if UNITY_EDITOR || UNITY_WEBGL 

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            tapInput = true;
        }



#endif
#if PLATFORM_ANDROID
        if (Input.touchCount > 0)
        {
            Touch firstTouch = Input.GetTouch(0);
            if (firstTouch.phase == TouchPhase.Began)
            {
                tapInput = true;
            }


        }

#endif

    }

    void AddForce()
    {
        if (tapInput)
        {


            rb.AddForce(Vector2.up * forceAmount, ForceMode2D.Impulse);
            if (!GameManager.Instance.SfxSource.isPlaying)
                GameManager.Instance.SfxSource.PlayOneShot(GameManager.Instance.tapClip);

            tapInput = false;

        }
    }

    void SetPlayerRotation()
    {
        yVelocity = rb.velocity.y;

        currentEularAngle = playerTranform.eulerAngles;
        newEularAngle.x = currentEularAngle.x;
        newEularAngle.y = currentEularAngle.y;
        newEularAngle.z = yVelocity * rotateAmount * Time.fixedDeltaTime;

        playerTranform.localEulerAngles = newEularAngle;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (1 << collision.gameObject.layer == collisionLayer)
        {
            GameManager.Instance.GameOver();
            playerTranform.eulerAngles = Vector3.zero;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            GameManager.Instance.AddCoin(collision.gameObject.transform);
            collision.gameObject.SetActive(false);
        }
    }
}
