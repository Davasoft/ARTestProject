using System.Collections;
using UnityEngine;

/// <summary>
/// This class broadcasts messages using the ClientManager to retrieve data from an API endpoint.
/// </summary>
public class MessageBroadcaster : MonoBehaviour
{
    // Reference to the configuration for the API
    public ApiConfig apiConfig;

    // Time to wait between API calls in seconds
    private float waitTime = 5f;

    // WaitForSeconds object for optimizing the wait time in the coroutine
    private WaitForSeconds waitInterval;

    /// <summary>
    /// Initializes the script and starts the coroutine for repeated API calls.
    /// </summary>
    private void Start()
    {
        // Initialize the WaitForSeconds object for optimisation
        waitInterval = new WaitForSeconds(waitTime);

        // Start the coroutine for repeated API calls
        StartCoroutine(RepeatAPICall());
    }

    /// <summary>
    /// Coroutine that update current wind data
    /// </summary>
    /// <returns></returns>
    IEnumerator RepeatAPICall()
    {
        while (true)
        {
            // Call the API using the ClientManager
            StartCoroutine(ClientManager.instance.GetData_Coroutine(apiConfig.apiEndpoint, DisplayInformation, apiConfig.authToken));

            // Wait for the specified time before the next API call
            yield return waitInterval;
        }
    }

    /// <summary>
    /// Displays information from the OpenWeather retrieved JSON data.
    /// </summary>
    /// <param name="json">The JSON data retrieved from the API.</param>
    void DisplayInformation(string json)
    {
        // Deserialize JSON data into WeatherData object
        WeatherData weatherData = JsonUtility.FromJson<WeatherData>(json);

        // Access and log wind speed
        if (weatherData != null && weatherData.wind != null)
        {
            // Invoke an event to display the wind speed
            Signatures.DisplayMessage.Invoke("Wind Speed: " + weatherData.wind.speed);
        }
    }
}