using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class ApiExample : MonoBehaviour
{
    private const float API_CHECK_MAXTIME = 5;
    private float apiCheckCountdown = API_CHECK_MAXTIME;
    public GameObject prefab;

    void Start()
    {
        CheckRandom();
    }
    void Update()
    {
        apiCheckCountdown -= Time.deltaTime;
        if (apiCheckCountdown <= 0)
        {
            CheckRandom();
            apiCheckCountdown = API_CHECK_MAXTIME;
        }
    }


    public void CheckRandom()
    {
        // Assumes you're using "kubectl proxy", and no authentication is required.
        KubeApiClient client = KubeApiClient.Create("http://localhost:8001");

        PodListV1 pods = await client.PodsV1().List(
            labelSelector: "k8s-app=my-app"
        );        // Load from the default kubeconfig on the machine.
        var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();


        // Use the config object to create a client.
        var client = new Kubernetes(config);

        //var r = await GetRandomFromApi();


        //for (int i = 0; i < r.random; i++)
        //{
        //    Instantiate(prefab);
        //    await Task.Delay(500);
        //}

    }
    private async Task<RandomInfo> GetRandomFromApi()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://csrng.net/csrng/csrng.php?min=0&max=100");
        HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd().Replace("[","").Replace("]","");
        RandomInfo info = JsonUtility.FromJson<RandomInfo>(jsonResponse);
        return info;
    }
}
