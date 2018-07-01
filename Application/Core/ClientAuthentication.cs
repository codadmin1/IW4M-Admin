﻿using SharedLibraryCore.Interfaces;
using SharedLibraryCore.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IW4MAdmin.Application.Core
{
    class ClientAuthentication : IClientAuthentication
    {
        private Queue<Player> ClientAuthenticationQueue;
        private Dictionary<long, Player> AuthenticatedClients;

        public ClientAuthentication()
        {
            ClientAuthenticationQueue = new Queue<Player>();
            AuthenticatedClients = new Dictionary<long, Player>();
        }

        public void AuthenticateClients(IList<Player> clients)
        {
            // we need to un-auth all the clients that have disconnected
            var clientNetworkIds = clients.Select(c => c.NetworkId);
            var clientsToRemove = AuthenticatedClients.Keys.Where(c => !clientNetworkIds.Contains(c));
            // remove them
            foreach (long Id in clientsToRemove.ToList())
            {
                AuthenticatedClients.Remove(Id);
            }

            // loop through the polled clients to see if they've been authenticated yet
            foreach (var client in clients)
            {
                // they've not been authenticated
                if (!AuthenticatedClients.TryGetValue(client.NetworkId, out Player value))
                {
                    // authenticate them
                    client.IsAuthenticated = true;
                    AuthenticatedClients.Add(client.NetworkId, client);
                }
                else
                {
                    // this update their ping
                    value.Ping = client.Ping;
                }
            }

            // empty out the queue of clients detected through log
            while (ClientAuthenticationQueue.Count > 0)
            {
                // grab each client that's connected via log
                var clientToAuthenticate = ClientAuthenticationQueue.Dequeue();
                // if they're not already authed, auth them
                if (!AuthenticatedClients.TryGetValue(clientToAuthenticate.NetworkId, out Player value))
                {
                    // authenticate them
                    clientToAuthenticate.IsAuthenticated = true;
                    AuthenticatedClients.Add(clientToAuthenticate.NetworkId, clientToAuthenticate);
                }
            }
        }

        public IList<Player> GetAuthenticatedClients()
        {
            return AuthenticatedClients.Values.ToList();
        }

        public void RequestClientAuthentication(Player client)
        {
            ClientAuthenticationQueue.Enqueue(client);
        }
    }
}
