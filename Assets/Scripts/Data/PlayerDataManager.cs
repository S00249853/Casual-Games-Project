using UnityEngine;
using Unity.Services.CloudCode;
using Unity.Services.CloudCode.GeneratedBindings;
using System;
public class PlayerDataManager : MonoBehaviour
{
    private PlayerDataServiceBindings MyModuleBindings;
    public LoginManager LoginManager;
    public string PlayerName;
    void Start()
    {
        LoginManager.PlayerSignedIn += InitializePlayer;

        MyModuleBindings = new PlayerDataServiceBindings(CloudCodeService.Instance);
    }

    private async void InitializePlayer()
    {
        try
        {
            var resultFromCloud = await MyModuleBindings.SayHello(PlayerName);
            Debug.Log($"{resultFromCloud}");
        }

        catch(CloudCodeException ex)
        {
            Debug.LogException(ex);
        }
    }

    public async void SaveNewPlayerName()
    {
        if (!IsPlayerNameValid(PlayerName))
        {
            Debug.LogWarning("Name must be 4-16 characters");
            return;
        }
        try
        {
            PlayerName = await MyModuleBindings.HandleNewPlayerNameEntry(PlayerName);
            Debug.Log($"Saved new player name in the cloud: {PlayerName}");
        }

        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
    private bool IsPlayerNameValid(string name)
    {
        if (name.Length is < 4 or > 16)
        {
            return false;
        }



        return true;
    }

    private void OnDisable()
    {
        LoginManager.PlayerSignedIn -= InitializePlayer;
    }
}
