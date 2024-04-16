
using UnityEngine;

public class DummyPlane : MonoBehaviour
{
    public PlaneColor dummyColor = PlaneColor.YellowPlane;
    [Range(-1f, 0f)]
    public float moveSpeedX;
    [Range(0f, 1f)]
    public float moveSpeedY;

    [SerializeField] bool useExtraSine;
    [SerializeField] float sinMultiplyer = 7;
    [SerializeField] bool useExtraCosine;
    [SerializeField] float cosineMultiplyer = 5;

    Vector3 moveDir = Vector3.one;
    float a = 0;
    float b = 0;

    private void Awake()
    {
        GetComponent<Animator>().Play(dummyColor.ToString());
    }

    private void FixedUpdate()
    {
        if (useExtraSine)
            a = Mathf.Sin(Time.time * sinMultiplyer) * 0.5f;

        if (useExtraCosine)
            b = Mathf.Cos(Time.time * cosineMultiplyer) * 0.5f;

        moveDir.x = transform.position.x + moveSpeedX * Time.deltaTime;
        moveDir.y = Mathf.Sin(Time.time * moveSpeedY) + a + b;
        moveDir.z = transform.position.z;

        transform.position = moveDir;
    }

}
