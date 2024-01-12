using UnityEngine;

[CreateAssetMenu(fileName = "ApiConfig", menuName = "Config/API Configuration")]
public class ApiConfig : ScriptableObject
{
    public string apiEndpoint;
    public string authToken;
}