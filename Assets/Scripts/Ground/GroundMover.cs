using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMover : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 moveDir;
    [SerializeField] SpriteRenderer _renderer;
    [Space]
    [SerializeField] List<Sprite> groundSprites;
    //
    Vector3 boundsValue;
    IEnumerator myCourutine;
    EnvironmentHandler environmentHandler;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        boundsValue = _renderer.bounds.extents;
        boundsValue.x *= 2f;
    }
    private void Start()
    {
        environmentHandler = EnvironmentHandler.Instance;

        _renderer.sprite = groundSprites[(int)environmentHandler.currentEnvironment];





    }
    public void ResetRender()
    {
        _renderer.sprite = groundSprites[(int)environmentHandler.currentEnvironment];
    }

    private void FixedUpdate()
    {
        if (transform.position.x < -10.04f)
        {

            if (myCourutine == null)
            {
                myCourutine = ResetPosition();
                StartCoroutine(myCourutine);
            }
        }
        moveSpeed = environmentHandler.moveSpeed;
        moveDir = environmentHandler.moveDirection;

        transform.position += moveSpeed * Time.fixedDeltaTime * (Vector3)moveDir;


    }

    IEnumerator ResetPosition()
    {
        _renderer.sprite = groundSprites[(int)environmentHandler.currentEnvironment];
        transform.position = transform.parent.GetChild(2).transform.position + new Vector3(boundsValue.x, 0, 0);
        transform.SetAsLastSibling();
        transform.parent.GetChild(1).GetComponent<SpriteRenderer>().sprite = _renderer.sprite;
        yield return null;
        myCourutine = null;
    }


}
