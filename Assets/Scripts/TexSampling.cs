using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TexSampling : MonoBehaviour
{
    public Texture2D image;
    public ComputeShader csBuffer;
    ComputeBuffer buffer;
    // Start is called before the first frame update
    void Start()
    {

        Vector3[] total = new Vector3[256];
        buffer = new ComputeBuffer(256, 12);
        int kernel = csBuffer.FindKernel("CSMain");
        csBuffer.SetTexture(kernel, "Orgin", image);
        csBuffer.SetBuffer(kernel, "Result", buffer);
        csBuffer.Dispatch(kernel, image.width, image.height, 1);
        buffer.GetData(total);
        for (int i = 0; i < total.Length; i++)
        {
            //Debug.Log(total[i]);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
