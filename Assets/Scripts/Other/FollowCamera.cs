using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform ObjectToFollow;
    Vector3 targetPosition;

    void LateUpdate()
    {
        if (ObjectToFollow == null)
        {
            ObjectToFollow = GameObject.FindGameObjectWithTag("Player").transform;
        }
        targetPosition.x = ObjectToFollow.position.x;
        targetPosition.y = ObjectToFollow.position.y;
        targetPosition.z = transform.position.z;

        transform.position = targetPosition;
    }
}
