using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPlayer : Singleton<DataPlayer>
{
    public string dataPl
    {
        get => PlayerPrefs.GetString("data_player", "");
        set => PlayerPrefs.SetString("data_player", value);
    }

    public Player player;
    private PlayerController playerController;
    public void LoadData(PlayerController playerController)
    {
        this.playerController = playerController;
        if (dataPl.Length <= 0)
        {
            player = new Player(playerController.transform.position, playerController.animator.transform.localEulerAngles.y);

        }
        else
        {
            player = JsonUtility.FromJson<Player>(dataPl);
            GameManager.Instance.playerController.transform.position = player.pos;
            GameManager.Instance.playerController.animator.transform.localEulerAngles = new Vector3(0, player.rotationY, 0);
        }
    }

    public void AddCoin(int amount)
    {
        AudioManager.Instance.PlayOneShot("addCoin", 1f);
        player.coins += amount;
        Save();
        GameManager.Instance.uiController.GetPopup<TopUIPopup>().UpdateTxt();
    }

    public void AddExp(int amount)
    {
        player.exp += amount;
        Save();
        GameManager.Instance.uiController.GetPopup<TopUIPopup>().UpdateTxt();
    }
    public void Save()
    {
        player.pos = playerController.transform.position;
        player.rotationY = playerController.animator.transform.localEulerAngles.y;
        dataPl = JsonUtility.ToJson(player);
    }
}
