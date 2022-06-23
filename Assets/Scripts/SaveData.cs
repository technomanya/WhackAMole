using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class SaveData : MonoBehaviour
{

    [SerializeField] private List<HitPositionData> hitPositionDataList;
    [SerializeField] private HitTimeStampData hitTimeStampData;
    private string allData;
    public void SavePointData(string _pointData)
    {
        var pointData = new PointData();
        pointData.point = _pointData;
        var pointDataJson = JsonUtility.ToJson(pointData);
        allData += pointDataJson + "\n";
        //System.IO.File.WriteAllText(Application.persistentDataPath + "/WhackAMoleData.json", potionDataJson);
    }
    public void SavePositionData(Vector3 position)
    {
        var hitPositionData = new HitPositionData
        {
            positionX = position.x,
            positionY = position.y,
            positionZ = position.z
        };
        
        var positionDataJson = JsonUtility.ToJson(hitPositionData);
        allData += positionDataJson + "\n";
        //System.IO.File.WriteAllText(Application.persistentDataPath + "/WhackAMoleData.json", positionDataJson);
    }
    
    public void SaveTimeData(string timeData)
    {
        hitTimeStampData = new HitTimeStampData();
        hitTimeStampData.hitTime = timeData;
        var timeDataJson = JsonUtility.ToJson(this.hitTimeStampData);
        allData += timeDataJson + "\n";
        //System.IO.File.WriteAllText(Application.persistentDataPath + "/WhackAMoleData.json", timeDataJson);
    }

    public void SaveAllData()
    {
        System.IO.File.WriteAllText(Application.persistentDataPath + "/WhackAMoleData.json", allData);
    }
}
[System.Serializable]
public class PointData
{
    public string point;
}

[System.Serializable]
public class HitPositionData
{
    public float positionX;
    public float positionY;
    public float positionZ;
}

[System.Serializable]
public class HitTimeStampData
{
    public string hitTime;
}
