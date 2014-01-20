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
            Team homeTeam = new Team(TeamType.Home);
            players.Add(new Player(CourtPos.Center, homeTeam, r));
            return true;
        }
    }
}
