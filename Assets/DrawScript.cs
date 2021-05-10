using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.codinblack.com/how-to-draw-lines-circles-or-anything-else-using-linerenderer/
// Ismail Camonu 
// This code was modified based on the above tutorial

namespace Utilities.Drawing 
{

    public class DrawScript : MonoBehaviour
    {
        string settingsFile = @"";
        public Transform p0;
        public Transform p1;
        public Transform p2;

        Dictionary<string, Vector3> _positions = new Dictionary<string, Vector3>();

        private LineRenderer lineRenderer;
        // Start is called before the first frame update
        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        public void SaveTransform(string key)
        {
            Debug.Log(p1.position);

            if (_positions.ContainsKey(key))
                _positions[key] = p1.position;
            else 
                _positions.Add(key, p1.position);

            // var data = "";

            // System.IO.File.AppendAllLines(data, settingsFile);
        }

        // Update is called once per frame
        void Update()
        {
            DrawQuadraticBezierCurve(p0.position, p1.position, p2.position);
        }

        void DrawQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2)
        {
            lineRenderer.positionCount = 200;
            lineRenderer.startWidth = 0.02f;
            lineRenderer.endWidth = 0.02f;
            lineRenderer.loop = false;
            float t = 0f;
            Vector3 B = new Vector3(0, 0, 0);

            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
                lineRenderer.SetPosition(i, B);
                t += (1 / (float)lineRenderer.positionCount);
            }
        }

        void DrawRandomTriangle()
        {
            lineRenderer = GetComponent<LineRenderer>();

            Vector3[] positions = new Vector3[3] { new Vector3(0, 0, 0), new Vector3(-1, 1, 0), new Vector3(1, 1, 0) };

            DrawTriangle(positions, 0.02f, 0.02f);
        }

        void DrawRandoPoly()
        {
            lineRenderer = GetComponent<LineRenderer>();

            // Vector3[] positions = new Vector3[3] { new Vector3(0, 0, 0), new Vector3(-1, 1, 0), new Vector3(1, 1, 0) };

            DrawPolygon(15, 1.0f, new Vector3(1, 1, 0), 0.03f, 0.03f);
        }

        void DrawTriangle(Vector3[] vertexPositions, float startWidth, float endWidth)
        {
            lineRenderer.startWidth = startWidth;
            lineRenderer.endWidth = endWidth;
            lineRenderer.positionCount = 3;
            lineRenderer.loop = true;
            lineRenderer.SetPositions(vertexPositions);
        }

        void DrawPolygon(int vertexNumber, float radius, Vector3 centerPos, float startWidth, float endWidth)
        {
            lineRenderer.startWidth = startWidth;
            lineRenderer.endWidth = endWidth;
            lineRenderer.loop = true;
            float angle = 2 * Mathf.PI / vertexNumber;
            lineRenderer.positionCount = vertexNumber;

            for (int i = 0; i < vertexNumber; i++)
            {
                Matrix4x4 rotationMatrix = new Matrix4x4(new Vector4(Mathf.Cos(angle * i), Mathf.Sin(angle * i), 0, 0),
                                                         new Vector4(-1 * Mathf.Sin(angle * i), Mathf.Cos(angle * i), 0, 0),
                                           new Vector4(0, 0, 1, 0),
                                           new Vector4(0, 0, 0, 1));
                Vector3 initialRelativePosition = new Vector3(0, radius, 0);
                lineRenderer.SetPosition(i, centerPos + rotationMatrix.MultiplyPoint(initialRelativePosition));

            }
        }
    }
}
