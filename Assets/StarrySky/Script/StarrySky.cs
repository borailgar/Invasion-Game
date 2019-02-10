using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class StarrySky : MonoBehaviour
{
    public Material m_Mat = null;
	private Vector2 m_Offset = new Vector2 ();
    public Vector2 m_Velocity = new Vector2 ();
	public bool m_Enable = false;

	void Start ()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			enabled = false;
			return;
		}
	}
	void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (m_Enable)
		{	
			Graphics.Blit (sourceTexture, destTexture, m_Mat);
		}
		else
		{
			Graphics.Blit (sourceTexture, destTexture);
		}
	}
	void Update ()
	{
		m_Offset += m_Velocity * 0.00001f;
		m_Mat.SetVector ("_Offset", m_Offset);
		m_Mat.SetVector ("_Resolution", new Vector4(Screen.width, Screen.height, 0, 0));
	}
}