using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnComplete:UnityEvent { }

public class PushObject : MonoBehaviour
{
    [SerializeField] private bool drawGizmos;

    [SerializeField] private bool useLocalPositions;

    [SerializeField] private string tagReference="Player";

    [SerializeField] private Transform detectionStartPoint;

    [SerializeField] private float radius;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private int maxAllocation;

    [SerializeField] private float onFailDetectionDelay=0.1f;

    [SerializeField] private Vector2 inputDirectionReference=Vector2.up;

    [SerializeField] private Vector3 targetPosition;

    [SerializeField] private float speed;

    [SerializeField] private bool startActived=true;

    public OnComplete OnComplete;

    private Transform _transform;

    private InputController inputController;

    private Vector2 currentDirection;

    private Coroutine pushCoroutine;

    private Collider[] hitResults;

    private void Awake()
    {
        inputController = FindObjectOfType<InputController>();

        inputController.OnMoveEvent += PressingKey;

        _transform = GetComponent<Transform>();

        hitResults = new Collider[maxAllocation];
    }

    private void Start()
    {
        if(startActived)
            StartCoroutine(Push());
    }

    private void PressingKey(Vector2 movementInput )
    {
        currentDirection = movementInput;
    }

    IEnumerator Push()
    {
        do
        {
            int amount = Physics.OverlapSphereNonAlloc(detectionStartPoint.position, radius, hitResults, layerMask);
            if (amount > 0)
            {
               for(int i = 0; i < amount; i++)
               {
                    if (hitResults[i].gameObject.CompareTag(tagReference))
                    {
                        TryPush();
                        break;
                    }
               }
            }
            else
            {
                yield return new WaitForSeconds(onFailDetectionDelay);
            }

            yield return null;

        } while (_transform.position != targetPosition);

        OnComplete?.Invoke();

        yield break;
    }

    private void TryPush()
    {
        if (currentDirection == inputDirectionReference)
        {
            if (useLocalPositions)
            {
                _transform.localPosition = Vector3.MoveTowards(_transform.localPosition, targetPosition, speed * Time.deltaTime);
            }
            else
            {
                _transform.position = Vector3.MoveTowards(_transform.position, targetPosition, speed * Time.deltaTime);
            }
        }
    }


    public void Active(bool value)
    {
        if (value)
        {
            StartCoroutine(Push());
        }
        else
        {
            StopCoroutine(Push());
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.DrawSphere(detectionStartPoint.position, radius);

            Gizmos.color = Color.red;
           
            Gizmos.DrawSphere(targetPosition, 0.1f);
            
        }
    }
}
