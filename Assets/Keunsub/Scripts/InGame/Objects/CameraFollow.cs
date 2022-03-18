using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraFollowType
{
    FollowAnyway,
    FollowHorizontal,
    FollowVertical,
    FollowArena
}

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] Vector2 LimitX;
    [SerializeField] Vector2 LimitY;

    [SerializeField] float moveSpeed;
    [SerializeField] Transform target;

    public CameraFollowType followType;
    Camera thisCamera;

    void Start()
    {
        thisCamera = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        switch (followType)
        {
            case CameraFollowType.FollowAnyway:
                FollowAnyway();
                break;
            case CameraFollowType.FollowHorizontal:
                FollowHorizontal(-6f);
                break;
            case CameraFollowType.FollowVertical:
                FollowVertical(-22f);
                break;
            case CameraFollowType.FollowArena:
                FollowArena();
                break;
        }
    }

    void FollowHorizontal(float y)
    {
        transform.position = Vector3.Lerp(transform.position, target.position + new Vector3(0, 0, -10f), Time.deltaTime * moveSpeed);

        float yClamp = y;
        transform.position = new Vector3(transform.position.x, yClamp, -10f);
    }

    void FollowVertical(float x)
    {
        transform.position = Vector3.Lerp(transform.position, target.position + new Vector3(0, 0, -10f), Time.deltaTime * moveSpeed);

        float xClamp = x;
        transform.position = new Vector3(transform.position.x, xClamp, -10f);
    }

    void FollowAnyway()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + new Vector3(0, 0, -3), Time.deltaTime * moveSpeed);
    }

    void FollowArena()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + new Vector3(0, 0, -10), Time.deltaTime * moveSpeed);

        float xClamp = Mathf.Clamp(transform.position.x, LimitX.x, LimitX.y);
        float yClamp = Mathf.Clamp(transform.position.y, LimitY.x, LimitY.y);

        transform.position = new Vector3(xClamp, yClamp, -10f);
    }
}
