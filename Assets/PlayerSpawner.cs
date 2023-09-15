using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private int numberOfPlayersAtStart = 1;
    [SerializeField] private AssetReference playerReference;
    private List<GameObject> players = new List<GameObject>();
    [SerializeField] private List<AddressableAssetLoader> addressableAssetLoaders = new List<AddressableAssetLoader>();
    [SerializeField] private Canvas sharedCanvas;
    [SerializeField] private GameObject canvasHorizontalLayout;
    [SerializeField] private GameObject targetBasket;

    void Start()
    {
        for (int i = 0; i < numberOfPlayersAtStart; i++)
        {
            SpawnPlayer();
        }
    }

    [ContextMenu("SpawnBall")]
    void SpawnPlayer()
    {
        AddressableAssetLoader addressableAssetLoader = new AddressableAssetLoader(playerReference);
        addressableAssetLoader.LoadAsync(OnPlayerLoaded);
        addressableAssetLoaders.Add(addressableAssetLoader);
    }

    public void OnPlayerLoaded(GameObject player)
    {
        player.name = "Player " + players.Count;
        player.GetComponent<PlayerTargetBasket>().SetTargetBasket(targetBasket);
        player.transform.position =
            this.transform.position + transform.TransformDirection(Vector3.left) * players.Count;
        players.Add(player);
        var playerCanvas = player.GetComponentsInChildren<Canvas>();
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

    private void OnPlayerLeft(PlayerInput leftingPlayer)
    {
        var playerIndex = players.IndexOf(leftingPlayer.gameObject);
        addressableAssetLoaders[playerIndex].ReleaseCompletely();
    }

    private void OnDestroy()
    {
        foreach (var addressableAssetLoader in addressableAssetLoaders)
        {
            addressableAssetLoader.ReleaseCompletely();
        }
    }
}