using System;
using System.Collections.Generic;
using System.Drawing;

using AI;
using Core;
using Renderer;

namespace bball
{
    class PlayerManager
    {
        List<Player> players = new List<Player>();

        public bool Initialize(IRenderer r)
        {
            players.Add(new Player(CourtPos.Center, TeamType.Home, r));
            players.Add(new Player(CourtPos.FromCoord(220,0,30), TeamType.Home, r));
            return true;
        }
    }
}