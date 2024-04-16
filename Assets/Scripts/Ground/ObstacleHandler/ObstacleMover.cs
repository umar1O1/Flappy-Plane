using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 moveDir;

    [SerializeField] SpriteRenderer[] renderRocksArray;

    [SerializeField] List<Sprite> spriteList;

    EnvironmentHandler environmentHandler;

    [Space]
    [SerializeField] float disableDistance = -10f;

    int environmetIndex;
    private void Awake()
    {
        renderRocksArray = transform.GetComponentsInChildren<SpriteRenderer>();
        environmentHandler = EnvironmentHandler.Instance;
    }
    private void OnEnable()
    {
        moveSpeed = environmentHandler.moveSpeed;
        moveDir = environmentHandler.moveDirection;

        environmetIndex = ((int)environmentHandler.currentEnvironment);
        foreach (var rock in renderRocksArray)
        {
            rock.sprite = spriteList[environmetIndex];

        }

    }
    private void FixedUpdate()
    {
        moveSpeed = environmentHandler.moveSpeed;

        if (disableDistance > transform.position.x)
        {
            gameObject.SetActive(false);
            return;
        }
        transform.position += moveSpeed * Time.fixedDeltaTime * (Vector3)moveDir;


    }


}
