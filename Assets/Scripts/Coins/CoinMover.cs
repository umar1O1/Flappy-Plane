using UnityEngine;

public class CoinMover : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 moveDir;


    [Space]
    [SerializeField] float disableDistance = -10f;

    private void Update()
    {


        if (disableDistance > transform.position.x)
        {
            gameObject.SetActive(false);
            return;
        }
        transform.position += moveSpeed * Time.deltaTime * (Vector3)moveDir;


    }
}
