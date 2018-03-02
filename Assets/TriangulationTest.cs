using System.Collections.Generic;
using UnityEngine;

public class TriangulationTest : MonoBehaviour 
{
    List<Vector3>  _outline = new List<Vector3>{new Vector3(0,0,0),new Vector3(1,0,0),new Vector3(1,0,1),new Vector3(0.5f,0,1.5f),new Vector3(0,0,1)};
    List<List<Vector3>>  _holes = new List<List<Vector3>>();

    public Material wireMaterial;

    public float MaximumArea;
    public float MinimumAngle;

    private float _prevMaxArea;
    private float _prevMinAngle;

    private GameObject _instantiatedMesh;

    void Start()
    {
        TriangulateTestOutline();
    }

	void TriangulateTestOutline ()
	{
	    Debug.Log("Triangulate outline and create mesh");
        List<int> outIndices;
	    List<Vector3> outVertices;

	    Triangulation.MaximumArea = MaximumArea;
	    Triangulation.MinimumAngle = MinimumAngle;

	    Triangulation.Triangulate(_outline, _holes, out outIndices, out outVertices);

	    Mesh triangulatedMesh = new Mesh();

	    triangulatedMesh.vertices = outVertices.ToArray();
	    triangulatedMesh.triangles = outIndices.ToArray();

	    if (_instantiatedMesh != null)
	    {
            Destroy(_instantiatedMesh);
	    }
        _instantiatedMesh = new GameObject();
	    MeshRenderer meshRenderer = _instantiatedMesh.AddComponent<MeshRenderer>();
	    meshRenderer.material = wireMaterial;
        MeshFilter meshFilter = _instantiatedMesh.AddComponent<MeshFilter>();
	    meshFilter.mesh = triangulatedMesh;
	}
	
	// Update is called once per frame
	void Update () {
	    if (MaximumArea != _prevMaxArea || MinimumAngle != _prevMinAngle)
	    {
	        TriangulateTestOutline();
	        _prevMaxArea = MaximumArea;
	        _prevMinAngle = MinimumAngle;
	    }
	}
}
