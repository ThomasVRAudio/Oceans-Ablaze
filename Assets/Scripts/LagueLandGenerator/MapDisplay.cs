using UnityEngine;
using System.Collections;

public class MapDisplay : MonoBehaviour {

	public Renderer textureRender;
	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;

	public bool useTexture;

	public void DrawTexture(Texture2D texture) {

		if (useTexture)
		{
			textureRender.sharedMaterial.mainTexture = texture;
		}
        else
        {
            textureRender.sharedMaterial.mainTexture = null;
        }

        textureRender.transform.localScale = new Vector3 (texture.width, 1, texture.height);
	}

	public void DrawMesh(MeshData meshData, Texture2D texture) {
		meshFilter.sharedMesh = meshData.CreateMesh ();

        if (useTexture)
        {
            textureRender.sharedMaterial.mainTexture = texture;
        } else
		{
			textureRender.sharedMaterial.mainTexture = null;
		}
	}

}
