﻿using System;
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
        private PlayerManager pm = new PlayerManager();
        //private Ball ball = new Ball();

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

            PlayerAI.SetReporter(Log.Instance);

            court.Image = renderer.GetImage("res/court.png", new MyColor(), "court");
            court.CreateBall(renderer.GetImage("res/Ball.png", new MyColor(), "Ball"));
            pm.Initialize(renderer);
            //ball.TargetLocation = Court.RightGoalPos;
            //ball.CurrentState = Ball.State.Shooting;
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
