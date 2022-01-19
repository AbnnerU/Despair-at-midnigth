using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPathPoints : MonoBehaviour
{
    [SerializeField] private Transform[] points;

    [SerializeField] private bool drawPoints;

    [SerializeField] private bool notRepeatLastTwoPoints=true;

    [SerializeField]private List<int> validsPoints = new List<int>();

    [SerializeField] private Queue<int> excludedPoints = new Queue<int>();

    private void Awake()
    {
        if(points.Length <= 2)
        {
            notRepeatLastTwoPoints = false;
        }

        for(int i = 0; i < points.Length; i++)
        {
            validsPoints.Add(i);
        }
    }

    public Vector3 GetValidPoint()
    {
        Vector3 value;
        if (notRepeatLastTwoPoints)
        {

            int id = Random.Range(0, validsPoints.Count);

            value = points[validsPoints[id]].position;

            if (excludedPoints.Count < 2)
            {
                excludedPoints.Enqueue(validsPoints[id]);

                validsPoints.RemoveAt(id);
            }
            else
            {
                validsPoints.Add(excludedPoints.Dequeue());

                excludedPoints.Enqueue(validsPoints[id]);

                validsPoints.RemoveAt(id);
            }

            return value;
        }
        else
        {
            int id = Random.Range(0, points.Length);

            return points[id].position;
        }
    }


    private void OnDrawGizmos()
    {
        if (drawPoints)
        {
            for(int i = 0; i < points.Length; i++)
            {
                Gizmos.DrawSphere(points[i].position,0.2f);
            }
        }
    }

}
