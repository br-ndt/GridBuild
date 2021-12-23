using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshUtils
{
    public static Mesh CreateEmptyMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[0];
        mesh.uv = new Vector2[0];
        mesh.triangles = new int[0];
        return mesh;
    }

    public static void CreateEmptyMeshArrays(int quadCount, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles)
    {
        vertices = new Vector3[4 * quadCount];
        uvs = new Vector2[4 * quadCount];
        triangles = new int[6 * quadCount];
    }

    public static Mesh CreateMesh(Vector3 pos, float rot, Vector3 baseSize, Vector2 uv00, Vector2 uv11)
    {
        return AddToMesh(null, pos, rot, baseSize, uv00, uv11);
    }

    public static Mesh AddToMesh(Mesh mesh, Vector3 pos, float rot, Vector3 baseSize, Vector2 uv00, Vector2 uv11)
    {
        if (mesh == null)
        {
            mesh = CreateEmptyMesh();
        }
		Vector3[] vertices = new Vector3[4 + mesh.vertices.Length];
		Vector2[] uvs = new Vector2[4 + mesh.uv.Length];
		int[] triangles = new int[6 + mesh.triangles.Length];
            
        mesh.vertices.CopyTo(vertices, 0);
        mesh.uv.CopyTo(uvs, 0);
        mesh.triangles.CopyTo(triangles, 0);

        int index = vertices.Length / 4 - 1;
		//Relocate vertices
		int vIndex = index*4;
		int vIndex0 = vIndex;
		int vIndex1 = vIndex+1;
		int vIndex2 = vIndex+2;
		int vIndex3 = vIndex+3;

        baseSize *= .5f;

        bool skewed = baseSize.x != baseSize.y;
        if (skewed)
        {
			vertices[vIndex0] = pos+MathUtils.GetQuaternionEuler(rot)*new Vector3(-baseSize.x,  baseSize.y);
			vertices[vIndex1] = pos+MathUtils.GetQuaternionEuler(rot)*new Vector3(-baseSize.x, -baseSize.y);
			vertices[vIndex2] = pos+MathUtils.GetQuaternionEuler(rot)*new Vector3( baseSize.x, -baseSize.y);
			vertices[vIndex3] = pos+MathUtils.GetQuaternionEuler(rot)*baseSize;
		}
        else
        {
			vertices[vIndex0] = pos+MathUtils.GetQuaternionEuler(rot-270)*baseSize;
			vertices[vIndex1] = pos+MathUtils.GetQuaternionEuler(rot-180)*baseSize;
			vertices[vIndex2] = pos+MathUtils.GetQuaternionEuler(rot- 90)*baseSize;
			vertices[vIndex3] = pos+MathUtils.GetQuaternionEuler(rot-  0)*baseSize;
		}
		
		//Relocate UVs
		uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
		uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
		uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
		uvs[vIndex3] = new Vector2(uv11.x, uv11.y);
		
		//Create triangles
		int tIndex = index*6;
		
		triangles[tIndex+0] = vIndex0;
		triangles[tIndex+1] = vIndex3;
		triangles[tIndex+2] = vIndex1;
		
		triangles[tIndex+3] = vIndex1;
		triangles[tIndex+4] = vIndex3;
		triangles[tIndex+5] = vIndex2;
            
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;

        //mesh.bounds = bounds;

        return mesh;
    }

    public static void AddToMeshArrays(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index, Vector3 pos, float rot, Vector3 baseSize, Vector2 uv00, Vector2 uv11) 
    {
		//Relocate vertices
		int vIndex = index*4;
		int vIndex0 = vIndex;
		int vIndex1 = vIndex+1;
		int vIndex2 = vIndex+2;
		int vIndex3 = vIndex+3;

        baseSize *= .5f;

        bool skewed = baseSize.x != baseSize.y;
        if (skewed)
        {
			vertices[vIndex0] = pos+MathUtils.GetQuaternionEuler(rot)*new Vector3(-baseSize.x,  baseSize.y);
			vertices[vIndex1] = pos+MathUtils.GetQuaternionEuler(rot)*new Vector3(-baseSize.x, -baseSize.y);
			vertices[vIndex2] = pos+MathUtils.GetQuaternionEuler(rot)*new Vector3( baseSize.x, -baseSize.y);
			vertices[vIndex3] = pos+MathUtils.GetQuaternionEuler(rot)*baseSize;
		}
        else
        {
			vertices[vIndex0] = pos+MathUtils.GetQuaternionEuler(rot-270)*baseSize;
			vertices[vIndex1] = pos+MathUtils.GetQuaternionEuler(rot-180)*baseSize;
			vertices[vIndex2] = pos+MathUtils.GetQuaternionEuler(rot- 90)*baseSize;
			vertices[vIndex3] = pos+MathUtils.GetQuaternionEuler(rot-  0)*baseSize;
		}
		
		//Relocate UVs
		uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
		uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
		uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
		uvs[vIndex3] = new Vector2(uv11.x, uv11.y);
		
		//Create triangles
		int tIndex = index*6;
		
		triangles[tIndex+0] = vIndex0;
		triangles[tIndex+1] = vIndex3;
		triangles[tIndex+2] = vIndex1;
		
		triangles[tIndex+3] = vIndex1;
		triangles[tIndex+4] = vIndex3;
		triangles[tIndex+5] = vIndex2;
    }
}
