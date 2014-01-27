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
        private Game game = null;

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
            renderer.ResizeBackBuffer(Court.ImageWidth, Court.ImageHeight);
            ImageFactory.Renderer = renderer;

            // Note : 원래는 메뉴에서 게임 스타트를 눌러야 실행되는 부분이나 테스트를 위해 삽입
            game = new Game();
            game.Initialize();
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
