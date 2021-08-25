using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RGB
{
    public int r;
    public int g;
    public int b;

    public RGB(int r, int g, int b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
    }

    public RGB()
    {
        this.r = 0;
        this.g = 0;
        this.b = 0;
    }
}

public class Main : MonoBehaviour
{
    public GameObject pixel;
    public Button send;
    private GameObject[,] pixels = new GameObject[8, 32];
    public TexSampling texSampling;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                var cell = Instantiate(pixel, pixel.transform.parent);
                cell.SetActive(true);
                pixels[i, j] = cell;
            }
        }

        send.onClick.AddListener(delegate { SendOnClicked(); });
        texSampling.Init(pixels);
    }

    private void SendOnClicked()
    {
        string json = SerializedData();
        StartCoroutine(PostJson("http://47.242.233.207:5000/LED", json));
    }

    public static IEnumerator PostJson(string url, string postData)
    {
        Debug.Log("请求中。。。Url:" + url);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(postData));
        request.timeout = 5;
        request.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if (request.isDone && !request.isHttpError)
        {
            DownloadHandler downloadHandler_TXT = request.downloadHandler;
            Debug.Log("请求完成");
        }
        else
        {
            Debug.Log(request.error);
        }
    }

    private string SerializedData()
    {
        StringBuilder sb = new StringBuilder();
        StringWriter sw = new StringWriter(sb);

        using (JsonWriter writer = new JsonTextWriter(sw))
        {
            writer.Formatting = Formatting.Indented;

            writer.WriteStartObject();
            writer.WritePropertyName("Data");
            writer.WriteStartArray();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    writer.WriteStartObject();
                    if (i % 2 == 0)
                    {
                        var data = pixels[i, j].GetComponent<PixelConfig>().GetData();
                        writer.WritePropertyName("r");
                        writer.WriteValue(data.r);
                        writer.WritePropertyName("g");
                        writer.WriteValue(data.g);
                        writer.WritePropertyName("b");
                        writer.WriteValue(data.b);
                    }
                    else
                    {
                        var data = pixels[i, 31 - j].GetComponent<PixelConfig>().GetData();
                        writer.WritePropertyName("r");
                        writer.WriteValue(data.r);
                        writer.WritePropertyName("g");
                        writer.WriteValue(data.g);
                        writer.WritePropertyName("b");
                        writer.WriteValue(data.b);
                    }

                    writer.WriteEndObject();
                }
            }

            writer.WriteEnd();
            writer.WriteEndObject();
        }

        Debug.Log(sb.ToString());
        return sb.ToString();
    }
}