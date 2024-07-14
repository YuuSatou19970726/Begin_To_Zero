using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ApiExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetRequest("https://jsonplaceholder.typicode.com/todos/1"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Received: " + webRequest.downloadHandler.text);

                // Xử lý dữ liệu nhận được
                string jsonResult = webRequest.downloadHandler.text;
                Debug.Log("Received: " + jsonResult);

                // Chuyển đổi dữ liệu JSON sang data model
                Todo todo = JsonUtility.FromJson<Todo>(jsonResult);
                Debug.Log("Title: " + todo.title);
            }
        }
    }

    IEnumerator PostRequest(string uri, string json)
    {
        var request = new UnityWebRequest(uri, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Gửi yêu cầu và đợi phản hồi
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Received: " + request.downloadHandler.text);
        }
    }
}

[System.Serializable]
public class Todo
{
    public int userId;
    public int id;
    public string title;
    public bool completed;
}
