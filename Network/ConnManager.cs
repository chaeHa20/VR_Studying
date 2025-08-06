using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct NetworkInputData : INetworkInput
{
    public Vector3 direction;
    public float roatation;

    public NetworkInputData(Vector3 direction, float rotation)
    {
        this.direction = direction;
        this.roatation = rotation;
    }
}


public class ConnManager : MonoSingleton<ConnManager>, INetworkRunnerCallbacks
{
    [SerializeField] NetworkPrefabRef m_playerPrefab;

    private NetworkRunner m_runner;
    private Dictionary<PlayerRef, NetworkObject> m_spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    private async void Start()
    {
        m_runner = gameObject.AddComponent<NetworkRunner>();
        m_runner.ProvideInput = true;

        initScene(out SceneRef scene);

        await m_runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.AutoHostOrClient,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
        });
    }

    private void initScene(out SceneRef scene)
    {
        scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            Vector3 spawnPosition = UnityEngine.Random.insideUnitSphere * 5.0f;
            spawnPosition.y = 0.0f;

            NetworkObject networkPlayerObject = runner.Spawn(m_playerPrefab, spawnPosition, Quaternion.identity, player);
            m_spawnedCharacters.Add(player, networkPlayerObject);
        }
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (m_spawnedCharacters.TryGetValue(player, out NetworkObject networkPlayerObject))
        {
            runner.Despawn(networkPlayerObject);
            m_spawnedCharacters.Remove(player);
        }
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        float h = ARVRInputManager.instance.getControllerAxis("Horizonbtal", eController.LTouch);
        float v = ARVRInputManager.instance.getControllerAxis("Vertical", eController.LTouch);
        var dir = new Vector3(h, 0.0f, v);

        var rot = ARVRInputManager.instance.getControllerAxis("Mouse X", eController.RTouch);

        NetworkInputData data = new NetworkInputData(dir,rot);

        input.Set(data);
    }

    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
}
