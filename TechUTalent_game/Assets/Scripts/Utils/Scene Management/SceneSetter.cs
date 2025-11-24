using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneSetter : Singleton<SceneSetter>
{
    [SerializeField] private string playerTag = "Player";
    public string NextDoor {  private get; set; }


    private void Start()
    {
        SceneManager.activeSceneChanged += SetupScene;
    }

    private void SetupScene(Scene arg0, Scene arg1)
    {
        var doors = FindObjectsByType<SceneDoor>(FindObjectsSortMode.None);
        var players = GameObject.FindGameObjectsWithTag(playerTag);
        foreach (var door in doors)
        {
            if (door.DoorId == NextDoor)
            {
                foreach (var player in players) player.transform.position = door.EnterPosition;
                return;
            }
        }
    }
}
