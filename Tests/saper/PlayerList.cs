using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

namespace Test.Saper
{
    public class PlayerList : NetworkBehaviour
    {
        public readonly SyncList<int> ConnectedClients = new SyncList<int>();
        public readonly SyncList<int> JoinedPlayers = new SyncList<int>();

        public readonly SyncDictionary<int, Player> Players = new SyncDictionary<int, Player>();

        public override void OnStartClient()
        {
            Players.Callback += OnPlayerListUpdate;
            ConnectedClients.Callback += OnConnectedListUpdate;
            JoinedPlayers.Callback += OnJoinedListUpdate;
        }

        void OnConnectedListUpdate(SyncList<int>.Operation op, int itemIndex, int oldItem, int newItem)
        {
            Debug.LogWarning("Connected list changed");
            Debug.LogWarning(op.ToString());
        }

        void OnJoinedListUpdate(SyncList<int>.Operation op, int itemIndex, int oldItem, int newItem)
        {
            Debug.LogWarning("Joined list changed");
            Debug.LogWarning(op.ToString());
        }

        void OnPlayerListUpdate(SyncIDictionary<int, Player>.Operation op, int key, Player item)
        {
            Debug.LogWarning("Players list changed");
            Debug.LogWarning(op.ToString());
        }

        [Server]
        public bool AddConnectedClient(NetworkConnection conn, Player player)
        {
            bool result = !Players.ContainsKey(conn.connectionId);
            if (result)
            {
                Players.Add(conn.connectionId, player);
                ConnectedClients.Add(conn.connectionId);
            }
            return result;
        }

        [Server]
        public bool MoveClientToActive(NetworkConnection conn)
        {
            bool result = Players.ContainsKey(conn.connectionId) &&
                ConnectedClients.Contains(conn.connectionId);

            if (result)
            {
                ConnectedClients.Remove(conn.connectionId);
                JoinedPlayers.Add(conn.connectionId);
            }
            return result;
        }

        [Server]
        public void RemoveClient(NetworkConnection conn)
        {
            if (Players.Remove(conn.connectionId))
            {
                if (!ConnectedClients.Remove(conn.connectionId)) JoinedPlayers.Remove(conn.connectionId);
            }
        }

        public Player GetPlayer(int connId)
        {
            Player player;
            Players.TryGetValue(connId, out player);
            return player;
        }
    }
}
