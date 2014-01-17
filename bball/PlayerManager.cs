using System;
using System.Collections.Generic;
using System.Drawing;

using Core;
using Renderer;

namespace bball
{
    class PlayerManager
    {
        List<Player> players = new List<Player>();

        public bool Initialize(IRenderer r)
        {
            Team homeTeam = new Team(TeamType.Home);
            players.Add(new Player(0, 0, homeTeam, r));
            return true;
        }
    }
}
