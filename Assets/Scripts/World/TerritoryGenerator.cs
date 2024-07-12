using UnityEngine;

public class TerritoryGenerator : MonoBehaviour
{
    [SerializeField]
    private Material color;
    private Mesh shape;

    private Vector3[] baseVertices = new Vector3[4];
    private Vector3[] finalVertices = new Vector3[24];

    private int[] triangles = new int[36];
    private int[][] allFaces = new int[1][];
    private readonly int faceNumber = 1;

    private void Start()
    {
        this.gameObject.AddComponent<MeshFilter>();
        this.gameObject.AddComponent<MeshRenderer>();
        GetComponent<MeshFilter>().mesh = new Mesh();
        GetComponent<MeshRenderer>().material = color;
        shape = GetComponent<MeshFilter>().mesh;
        this.gameObject.AddComponent<MeshCollider>();
        GetComponent<MeshCollider>().sharedMesh = shape;

        // Creation of the coordinates
        baseVertices[0] = new Vector3(-2f, 4f, 1f);
        baseVertices[1] = new Vector3(2f, 4f, 1f);
        baseVertices[2] = new Vector3(2f, -2f, 1f);
        baseVertices[3] = new Vector3(-2f, -2f, 1f);

        // Get the indexes of "baseVertices"
        allFaces[0] = new int[4] { 0, 1, 2, 3 };

        GeneratePolygon();
    }

    private void GeneratePolygon()
    {
        int numberOfVertices = 4;
        int verticesCount = 0;
        int trianglesCount = 0;

        for (int face=0; face<faceNumber; face++)
        {
            triangles[trianglesCount + 0] = verticesCount;
            triangles[trianglesCount + 1] = verticesCount + 1;
            triangles[trianglesCount + 2] = verticesCount + 2;
            triangles[trianglesCount + 3] = verticesCount;
            triangles[trianglesCount + 4] = verticesCount + 2;
            triangles[trianglesCount + 5] = verticesCount + 3;

            trianglesCount += faceNumber;

            for (int vertex=0; vertex<numberOfVertices; vertex++) {
                Vector3 currentPoint = baseVertices[allFaces[face][vertex]]; // * 0.5f
                finalVertices[verticesCount] = currentPoint;
                verticesCount++;
            }
        }

        UpdateMesh();
    }

    private void UpdateMesh()
    {
        shape.Clear();
        shape.vertices = finalVertices;
        shape.triangles = triangles;
        shape.RecalculateNormals();
    }
}
