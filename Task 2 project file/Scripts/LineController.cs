using UnityEngine;
using System.Collections.Generic; // Add this line to include the System.Collections.Generic namespace

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private bool isDrawing = false;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrawing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopDrawing();
            CheckForCollisions();
        }

        if (isDrawing)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, mousePos);
        }
    }

    private void StartDrawing()
    {
        isDrawing = true;
        lineRenderer.positionCount = 0;
    }

    private void StopDrawing()
    {
        isDrawing = false;
    }

   private void CheckForCollisions()
{
    int numPositions = lineRenderer.positionCount;

    HashSet<Collider2D> collidedCircles = new HashSet<Collider2D>();

    for (int i = 0; i < numPositions - 1; i++)
    {
        Vector3 startPos = lineRenderer.GetPosition(i);
        Vector3 endPos = lineRenderer.GetPosition(i + 1);

        Vector3 lineDirection = (endPos - startPos).normalized;
        float lineLength = Vector3.Distance(startPos, endPos);
        float expandedLineLength = lineLength + lineRenderer.startWidth;

        RaycastHit2D[] raycastHits = Physics2D.CircleCastAll(startPos, lineRenderer.startWidth, lineDirection, expandedLineLength);

        foreach (var raycastHit in raycastHits)
        {
            if (raycastHit.collider.CompareTag("Circle") && !collidedCircles.Contains(raycastHit.collider))
            {
                collidedCircles.Add(raycastHit.collider);
                Destroy(raycastHit.collider.gameObject);
            }
        }
    }
}


}
