using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaceInteraction : Interaction
{

    [SerializeField]
    private Vector3 displacement;
    [SerializeField]
    private bool isSmooth;
    [SerializeField]
    private float openSpeed = 8;

    private bool isDisplaced;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    public override void PerformInteraction()
    {
        base.PerformInteraction();

        isDisplaced = !isDisplaced;
    }

    private void Update()
    {
        if ((isDisplaced && transform.position != startPosition + displacement) || (!isDisplaced && transform.position != startPosition))
        {
            if (isDisplaced)
                transform.position = Vector3.MoveTowards(transform.position, startPosition + displacement, (Vector3.Distance(transform.position, startPosition + displacement) * 1.2f) * Time.deltaTime * openSpeed);
            else
                transform.position = Vector3.MoveTowards(transform.position, startPosition, (Vector3.Distance(transform.position, startPosition) * 1.2f) * Time.deltaTime * openSpeed);

        }
    }
}
