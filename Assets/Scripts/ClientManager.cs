using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

/// <summary>
/// Manages client-server communication using UnityWebRequest for both POST and GET operations.
/// </summary>
public class ClientManager : MonoBehaviour
{
    public static ClientManager instance;

    /// <summary>
    /// Initializes the singleton instance and ensures it persists across scenes.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sends a POST request with the provided data to the specified API endpoint.
    /// </summary>
    /// <typeparam name="T">Type of the data to be sent.</typeparam>
    /// <param name="data">Data to be sent in the request body.</param>
    /// <param name="apiEndpoint">API endpoint to send the POST request to.</param>
    /// <param name="callback">Callback function invoked with the response.</param>
    /// <param name="token">Optional authorization token for the request.</param>
    /// <returns>Coroutine for handling the POST request.</returns>
    public IEnumerator PostData_Coroutine<T>(T data, string apiEndpoint, System.Action<string> callback, string token = "")
    {
        string json;

        if (data is string)
        {
            json = data as string; // If 'data' is already a string, use it as it is
        }
        else
        {
            json = JsonUtility.ToJson(data); // If 'data' is an object, convert it to JSON
        }

        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest(apiEndpoint, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            if (token != "")
            {
                request.SetRequestHeader("Authorization", "Bearer " + token);
            }
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string responseJson = request.downloadHandler.text;
                Debug.Log(responseJson);
                callback?.Invoke(responseJson);
            }
        }
    }

    /// <summary>
    /// Sends a GET request to the specified API endpoint.
    /// </summary>
    /// <param name="apiEndpoint">API endpoint to send the GET request to.</param>
    /// <param name="callback">Callback function invoked with the response.</param>
    /// <param name="token">Optional authorization token for the request.</param>
    /// <returns>Coroutine for handling the GET request.</returns>
    public IEnumerator GetData_Coroutine(string apiEndpoint, System.Action<string> callback, string token = "")
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiEndpoint))
        {
            request.downloadHandler = new DownloadHandlerBuffer();

            if (token != "")
            {
                request.SetRequestHeader("Authorization", "Bearer " + token);
            }

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string responseJson = request.downloadHandler.text;
                Debug.Log(responseJson);
                callback?.Invoke(responseJson);
            }
        }
    }
}
