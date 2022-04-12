using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] Vector2 LimitX;
    [SerializeField] Vector2 LimitY;

    [SerializeField] float moveSpeed;
    [SerializeField] Transform target;
    Camera thisCamera;

    void Start()
    {
        thisCamera = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        FollowArena();
    }

    void FollowArena()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + new Vector3(0, 0, -10), Time.deltaTime * moveSpeed);

        float xClamp = Mathf.Clamp(transform.position.x, LimitX.x, LimitX.y);
        float yClamp = Mathf.Clamp(transform.position.y, LimitY.x, LimitY.y);

        transform.position = new Vector3(xClamp, yClamp, -10f);
    }
}
