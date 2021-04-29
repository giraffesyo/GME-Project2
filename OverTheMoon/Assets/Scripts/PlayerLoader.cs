using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class Player
{
    public string Name { get; set; }
    
    public int Score { get; set; }
    
    // public override string ToString()
    // {
    //     string output = username + "\n";
    //     if (scores == null) scores = new Dictionary<string, int>();
    //     if (scores.Count == 0) return output + "Scores is Empty!\n";
    //     foreach (KeyValuePair<string, int> kvp in scores) output += $" - {kvp.Key}: {kvp.Value}\n";
    //     return output;
    // }
}

public class PlayerLoader : MonoBehaviour
{
    public static PlayerLoader Instance;
    
    private Player player;

    public string Username => player.Name;
    
    private async void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        Debug.Log("Starting fetch...");
        await GetOrCreatePlayer("test");
        Debug.Log("Finished fetching...");
    }

    private async Task GetOrCreatePlayer(string username)
    {
        // StartCoroutine(FetchPlayer(username));
        await SavePlayer(new Player() {Name = "test3", Score = 0});
    }

    private IEnumerator FetchPlayer(string username)
    {
        var req = UnityWebRequest.Get($"https://fsxg5i1hzl.execute-api.us-east-1.amazonaws.com/prod/user?name={Uri.EscapeDataString(username)}");
        yield return req.SendWebRequest();
        player = JsonConvert.DeserializeObject<Player>(req.downloadHandler.text);
        
        Debug.Log($"Successfully retrieved player info: {req.downloadHandler.text}");
    }

    private async Task SavePlayer(Player playerToSave)
    {
        if (playerToSave == null) return;
        
        var playerContent = JsonConvert.SerializeObject(playerToSave, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() { NamingStrategy = new CamelCaseNamingStrategy() } });
        // https://fsxg5i1hzl.execute-api.us-east-1.amazonaws.com/prod
        var req = WebRequest.Create(Uri.EscapeUriString("https://fsxg5i1hzl.execute-api.us-east-1.amazonaws.com/prod/user"));
        req.ContentType = "application/json; charset=utf-8";
        req.Method = "POST";
        using (var streamWriter = new StreamWriter(req.GetRequestStream())) {
            streamWriter.Write(playerContent);
            streamWriter.Flush();
        }

        var res = (HttpWebResponse) await req.GetResponseAsync();
        var readStream = new StreamReader(res.GetResponseStream());
        if (res.StatusCode == HttpStatusCode.OK)
        {
            
            string jsonResponse = readStream.ReadToEnd();
            Debug.Log($"Successfully saved player info {jsonResponse}");   
        }
    }
    
}
