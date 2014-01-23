using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Core;
using Renderer;
using AI;

namespace bball
{
    public partial class MainForm : Form
    {
        private IRenderer renderer = Renderer.Container.GetInterface(Renderer.Type.Direct3D9);
        private Court court = Court.Instance;

        public MainForm()
        {
            InitializeComponent();
            this.ResizeRedraw = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                if (args[1] == "--stream")
                    renderer = Renderer.Container.GetInterface(Renderer.Type.Stream);
            }

            renderer.Initialize(this);
            renderer.SetReporter(Log.Instance);
            renderer.ResizeBackBuffer(Court.Width, Court.Height);
            ImageFactory.Renderer = renderer;

            court.Image = ImageFactory.Create("res/court.png");
            court.CreateBall(ImageFactory.Create("res/Ball.png"));

            var p1 = new Player();
            p1.PlayerLocation = CourtPos.Center;
            p1.Image = ImageFactory.Create("res/Player.png");
            p1.AI = PlayerAIFactory.Create(PlayerAIFactory.Type.ExpertSystem);
            p1.AI.SetReporter(Log.Instance);

            var p2 = new Player();
            p2.PlayerLocation = CourtPos.FromCoord(200, 0, 100);
            p2.Image = ImageFactory.Create("res/Player.png");
            p2.AI = PlayerAIFactory.Create(PlayerAIFactory.Type.ExpertSystem);
            p2.AI.SetReporter(Log.Instance);

            Team homeTeam = new Team(TeamType.Home);
            homeTeam.TeamState = TeamState.LooseBall;
            homeTeam.AddPlayer(p1);
            homeTeam.AddPlayer(p2);
        }

        private void GlobalTimer_Tick(object sender, EventArgs e)
        {
            OutputManager.UpdateAll();

            if (renderer.Clear(new MyColor(Color.Blue)))
            {
                if (renderer.BeginDraw())
                {
                    this.OnDraw(renderer);

                    if (renderer.EndDraw())
                        renderer.Flip();
                }
            }
        }
    }
}
