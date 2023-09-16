using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerSpawner : MonoBehaviour, ISpawner
{
    [Header("Config")]
    [SerializeField] private Canvas sharedCanvas;
    [SerializeField] private GameObject targetBasket;
    [SerializeField] private AssetReference playerReference;
    [SerializeField] private int numberOfPlayersAtStart = 1;
    private List<GameObject> players = new List<GameObject>();
    [SerializeField] private List<AddressableAssetLoader> addressableAssetLoaders = new List<AddressableAssetLoader>();
    [SerializeField] private GameObject canvasHorizontalLayout;

    void Start()
    {
        for (int i = 0; i < numberOfPlayersAtStart; i++)
        {
            Spawn();
        }
    }


    public void OnPlayerLoaded(GameObject player)
    {
        SetupNewPlayer(player);
    }

    void SetupNewPlayer(GameObject newPlayer)
    {
        newPlayer.GetComponent<PlayerName>().SetPlayerName("Player " + players.Count);
        newPlayer.GetComponent<PlayerTargetBasket>().SetTargetBasket(targetBasket);
        newPlayer.GetComponent<SpawnedBy>().SetSpawner(this);
        newPlayer.transform.position =
            this.transform.position +  Vector3.right * players.Count;
        players.Add(newPlayer);
        var playerCanvas = newPlayer.GetComponentsInChildren<Canvas>();
        foreach (var canvas in playerCanvas)
        {
            canvas.transform.parent = canvasHorizontalLayout.transform;
        }
    }

    [ContextMenu(nameof(LeftLastPlayer))]
    public void LeftLastPlayer()
    {
        OnPlayerLeft(players.Last().GetComponent<PlayerInput>());
    }

    private void OnPlayerLeft(PlayerInput leavingPlayer)
    {
        var playerIndex = players.IndexOf(leavingPlayer.gameObject);
        addressableAssetLoaders[playerIndex].ReleaseCompletely();
    }

    private void OnDestroy()
    {
        foreach (var addressableAssetLoader in addressableAssetLoaders)
        {
            addressableAssetLoader.ReleaseCompletely();
        }
    }

    public void Spawn()
    {
        AddressableAssetLoader addressableAssetLoader = new AddressableAssetLoader(playerReference);
        addressableAssetLoader.LoadAsync(OnPlayerLoaded);
        addressableAssetLoaders.Add(addressableAssetLoader);
    }

    public Vector3 GetSpawnPosition()
    {
        return
            transform.position;
    }
}