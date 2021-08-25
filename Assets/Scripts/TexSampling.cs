using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class TexSampling : MonoBehaviour
{
  
    public RenderTexture image;
    public ComputeShader csBuffer;
    ComputeBuffer buffer;

    private GameObject[,] pixels;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    public void Init(GameObject[,] pixels)
    {
        this.pixels = pixels;
    }
    public void Tex()
    {
     
        Vector3[,] total = new Vector3[8, 32];
        buffer = new ComputeBuffer(256, 12);
        int kernel = csBuffer.FindKernel("CSMain");
        csBuffer.SetTexture(kernel, "Orgin", image);
        
        csBuffer.SetBuffer(kernel, "Result", buffer);
        csBuffer.SetInt("OrginW", image.width);
        csBuffer.SetInt("OrginH", image.height);
        csBuffer.Dispatch(kernel, image.width, image.height, 1);
        buffer.GetData(total);
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                pixels[i, j].GetComponentInChildren<Image>().color =
                    new Color(total[i, j].x, total[i, j].y, total[i, j].z);
             
            }
        }
    }

    private void Update()
    {
        if (pixels != null)
            Tex();
    }
}