using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScore : MonoBehaviour
{
    int point = 5;
    public static event Action<int> onShot;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("*************CollisionScore");
            point = 1;

    }

    public void ScoretoAdd()
    {
        onShot?.Invoke(point);
    }
}
