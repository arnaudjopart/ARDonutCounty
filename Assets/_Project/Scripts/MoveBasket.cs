using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveBasket : MonoBehaviour
{
    Vector3[] positions= new Vector3[4];
    int index;
    Vector3 startPosition, actualPosition, targetPosition;
    [SerializeField]
    float duration; 

    private void Start()
    {
        index = 0;
        positions[index] = new Vector3(0, 0, 0.75f);
        index++;
        positions[index] = new Vector3(0,0,-0.75f);
        index++;
        positions[index] = new Vector3(1,0,-0.75f);
        index++;
        positions[index] = new Vector3(1,0,0.75f);
        startPosition = transform.position;

        StartCoroutine(PatrolCoroutine());
    }

    private void Update()
    {

    }

    IEnumerator PatrolCoroutine() 
    {
        int i = 0;
        index = 0;
        
        Vector3 startPosition = transform.position;
        new WaitForSeconds(0.5f);
        while (true)
        {
            float time = 0;
            if (index<positions.Length-1)
            {

                index++;
            }
            else
            {
                index = 0;
            }

            targetPosition = startPosition + positions[index];
            actualPosition = transform.position;
            while (time < duration)
            {
                transform.position = Vector3.Lerp(actualPosition, targetPosition, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPosition;
            i++;
        }
        

        
    }

}
