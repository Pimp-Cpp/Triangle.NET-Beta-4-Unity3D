using System.Collections.Generic;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using UnityEngine;

public static class Triangulation {

    public static float MinimumAngle { get; set; }
    public static float MaximumArea { get; set; }

    public static bool Triangulate(List<Vector3> points, List<List<Vector3>> holes, out List<int> outIndices, out List<Vector3> outVertices)
    {
        outVertices = new List<Vector3>();
        outIndices = new List<int>();

        Polygon poly = new Polygon();
        for (int i = 0; i < points.Count; i++)
        {
            float x = points[i].x;
            float y = points[i].z;
            poly.Add(new Vertex(x, y));

            if (i == points.Count - 1)
            {
                poly.Add(new Segment(new Vertex(x,y),new Vertex(points[0].x,points[0].z)));
            }
            else
            {
                poly.Add(new Segment(new Vertex(x,y),new Vertex(points[i + 1].x,points[i+1].z)));
            }
        }

        for (int i = 0; i < holes.Count; i++)
        {
            List<Vertex> vertices = new List<Vertex>();
            for (int j = 0; j < holes[i].Count; j++)
            {
                vertices.Add(new Vertex(holes[i][j].x,holes[i][j].z));
            }
            poly.Add(new Contour(vertices),true);
        }
        ConstraintOptions constraints = new ConstraintOptions();
        QualityOptions quality = new QualityOptions();
        quality.MinimumAngle = MinimumAngle;
        quality.MaximumArea = MaximumArea;
        var mesh = poly.Triangulate(constraints,quality);

        foreach (ITriangle t in mesh.Triangles)
        {
            for (int j = 2; j >=0; j--)
            {
                bool found = false;
                for (int k = 0; k < outVertices.Count; k++)
                {
                    if ((outVertices[k].x == t.GetVertex(j).X) && (outVertices[k].y == t.GetVertex(j).Y))
                    {
                        outIndices.Add(k);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    outVertices.Add(new Vector3((float) t.GetVertex(j).X, 0, (float)t.GetVertex(j).Y));
                    outIndices.Add(outVertices.Count-1);
                }
            }
        }

        return true;
    }
}