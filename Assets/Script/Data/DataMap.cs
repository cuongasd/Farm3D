using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMap : Singleton<DataMap>
{
    private string dataMap
    {
        get => PlayerPrefs.GetString("data_map", "");
        set => PlayerPrefs.SetString("data_map", value);
    }

    public string strDataAvailable
    {
        get => PlayerPrefs.GetString("data_available", "");
        set => PlayerPrefs.SetString("data_available", value);
    }

    public DataGround dataGround;
    public DataAvailable dataAvailable;
    public List<GroundCtrl> groundCtrls;
    public List<VacantLand> vacantLands;
    public List<AvailableCtrl> availableCtrls;
    public void LoadData()
    {
        int x = 0;
        if (dataMap.Length > 0)
        {
            dataGround = JsonUtility.FromJson<DataGround>(dataMap);
        }
        else
        {
            dataGround.grounds = new List<Ground>();
            for (int i = 0; i < groundCtrls.Count; i++)
            {
                Ground g = new Ground(x, groundCtrls[i].cropsController, null, groundCtrls[i].empty);
                dataGround.grounds.Add(g);
                x++;
            }

            for (int i = 0; i < vacantLands.Count; i++)
            {
                Ground g = new Ground(x, null, vacantLands[i].animalCtrl, groundCtrls[i].empty);
                dataGround.grounds.Add(g);
                x++;
            }
            Save();
        }

        x = 0;

        while (x < dataGround.grounds.Count)
        {
            for (int i = 0; i < groundCtrls.Count; i++)
            {
                groundCtrls[i].SetInfoGround(dataGround.grounds[x], x);
                x++;
            }

            for (int i = 0; i < vacantLands.Count; i++)
            {
                vacantLands[i].SetInfoVacantLand(dataGround.grounds[x], x);
                x++;
            }
        }

        if (strDataAvailable.Length <= 0)
        {
            dataAvailable = new DataAvailable();
            for (int i = 0; i < availableCtrls.Count; i++)
            {
                Available available = new Available(i + 1);
                dataAvailable.availables.Add(available);
            }
            SaveAvailable();
        }
        else
        {
            dataAvailable = JsonUtility.FromJson<DataAvailable>(strDataAvailable);
        }

        for (int i = 0; i < availableCtrls.Count; i++)
        {
            availableCtrls[i].SetInfo(dataAvailable.availables[i]);
        }
    }
    public void Save()
    {
        string data = JsonUtility.ToJson(dataGround);
        dataMap = data;
    }

    public void SaveAvailable()
    {
        string data = JsonUtility.ToJson(dataAvailable);
        strDataAvailable = data;
    }

    public void SetAvailable(Available available)
    {
        dataAvailable.availables[available.id - 1].isExist = available.isExist;
        SaveAvailable();
    }
    public void SaveGround()
    {
        foreach (var i in groundCtrls)
        {
            SetGround(i);
        }

        foreach (var i in vacantLands)
        {
            SetVacantLand(i);
        }
        Save();
    }

    public void SetGround(GroundCtrl groundCtrl)
    {
        foreach (Ground i in dataGround.grounds)
        {
            if (i.id == groundCtrl.id)
            {
                i.empty = groundCtrl.empty;
                if (groundCtrl.cropsController != null)
                {
                    i.idCrops = groundCtrl.cropsController.id;
                    i.time = groundCtrl.cropsController.curHarvestTime;
                }
                break;
            }
        }
        Save();
    }

    public void SetVacantLand(VacantLand vacantLand)
    {
        foreach (Ground i in dataGround.grounds)
        {
            if (i.id == vacantLand.id)
            {
                i.empty = vacantLand.empty;
                if (vacantLand.animalCtrl != null)
                {
                    i.idAnimals = vacantLand.animalCtrl.id;
                    i.time = vacantLand.animalCtrl.curHarvestTime;
                }
                break;
            }
        }
        Save();
    }
}
