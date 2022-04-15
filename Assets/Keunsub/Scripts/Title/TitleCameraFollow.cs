using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCameraFollow : MonoBehaviour
{
    public RoomType roomType;
    public Transform target;
    Vector2 clamp;
    Transform stand;

    void Start()
    {

    }

    public void Init()
    {
        transform.position = target.position + new Vector3(0, 0, -10f);
    }

    void FixedUpdate()
    {
        if (stand != null)
            FollowTarget();
    }

    public void SetFollowTarget(Transform _target, Transform _stand, RoomType roomType, Vector2 clamp)
    {
        target = _target;
        stand = _stand;
        this.clamp = clamp;
        this.roomType = roomType;
    }

    void FollowTarget()
    {
        Vector3 followPos = Vector3.Lerp(transform.position, target.position + new Vector3(0, 0, -10f), 0.5f);

        switch (roomType)
        {
            case RoomType.FollowVertical:
                followPos = ClampHorizontal(followPos);
                break;
            case RoomType.FollowHorizontal:
                followPos = ClampVertical(followPos);
                break;
        }

        transform.position = Vector3.Lerp(transform.position, followPos, 0.5f);
    }

    Vector3 ClampHorizontal(Vector3 pos)
    {
        float clampY = Mathf.Clamp(pos.y, clamp.x, clamp.y);
        return new Vector3(stand.position.x, clampY, pos.z);
    }

    Vector3 ClampVertical(Vector3 pos)
    {
        float clampX = Mathf.Clamp(pos.x, clamp.x, clamp.y);
        return new Vector3(clampX, stand.position.y, pos.z);
    }
}
