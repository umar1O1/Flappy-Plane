using System.Collections.Generic;

[System.Serializable]
public class PlayerScoreList
{
    public List<PlayerScoreData> topScoreList;

    //private string fileName = "TopScore.Json";

    public PlayerScoreList()
    {
        topScoreList = new List<PlayerScoreData>();
    }
    //public void AddNewScore(PlayerScoreData newScore)
    //{
    //    topScoreList.Add(newScore);
    //    UpdateList();
    //    SaveData();
    //}
    //public bool IsNewTopScore(int newTopScore)
    //{
    //    bool isTopScore = false;

    //    if (topScoreList.Count != 0)
    //    {
    //        if (topScoreList.Last().playerScore < newTopScore)
    //            isTopScore = true;
    //    }
    //    else
    //    {
    //        isTopScore = true;
    //    }

    //    return isTopScore;
    //}
    //void UpdateList() => topScoreList = topScoreList.OrderByDescending(scr => scr.playerScore).ToList();

    ////Save And Load
    //public void SaveData()
    //{
    //    string applicationPath = Path.Combine(Application.persistentDataPath, fileName);


    //    string newJson = JsonUtility.ToJson(this);
    //    Debug.Log(newJson);
    //    File.WriteAllText(applicationPath, newJson);

    //}

    //public void LoadData()
    //{
    //    string applicationPath = Path.Combine(Application.persistentDataPath, fileName);
    //    string loadedJsonDataString;


    //    if (!File.Exists(applicationPath))
    //    {
    //        File.Create(applicationPath);
    //    }
    //    else
    //    {

    //        loadedJsonDataString = File.ReadAllText(applicationPath);
    //        PlayerScoreList ps = JsonUtility.FromJson<PlayerScoreList>(loadedJsonDataString);
    //        topScoreList = new List<PlayerScoreData>(ps.topScoreList);

    //    }



    //}



}

