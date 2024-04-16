using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;


public class LeaderBoard : MonoBehaviour
{

    public PlayerScoreList scoreList = new();

    private string fileName = "TopScore.Json";



    IEnumerator Start()
    {
        yield return new WaitForSeconds(.2f);
        LoadData();
    }
    public void AddNewScore(PlayerScoreData newScore)
    {
        scoreList.topScoreList.Add(newScore);
        UpdateList();
        SaveData();
    }
    public bool IsNewTopScore(int newTopScore)
    {
        bool isTopScore = false;

        if (scoreList.topScoreList.Count != 0)
        {
            if (scoreList.topScoreList.Last().playerScore < newTopScore)
                isTopScore = true;
        }
        else
        {
            isTopScore = true;
        }

        return isTopScore;
    }
    void UpdateList() => scoreList.topScoreList = scoreList.topScoreList.OrderByDescending(scr => scr.playerScore).ToList();

    //Save And Load
    public void SaveData()
    {
        string applicationPath = Path.Combine(Application.persistentDataPath, fileName);


        string newJson = JsonUtility.ToJson(scoreList);
        Debug.Log(newJson);
        File.WriteAllText(applicationPath, newJson);

    }

    public void LoadData()
    {
        string applicationPath = Path.Combine(Application.persistentDataPath, fileName);
        string loadedJsonDataString;


        if (!File.Exists(applicationPath))
        {
            File.Create(applicationPath).Dispose();
        }
        else
        {

            loadedJsonDataString = File.ReadAllText(applicationPath);
            scoreList = JsonUtility.FromJson<PlayerScoreList>(loadedJsonDataString);


        }



    }
}


