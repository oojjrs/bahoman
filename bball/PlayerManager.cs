using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Core;
using Renderer;

namespace bball
{
    class PlayerManager
    {
        List<Player> players = new List<Player>();

        public bool Initialize(IRenderer r)
        {
            players.Add(new Player(0, 0, TeamType.Home, r));
            return true;
        }

        public void OnDraw(IRenderer r)
        {
            foreach (var p in players)
                p.OnDraw(r);
        }
    }
}
