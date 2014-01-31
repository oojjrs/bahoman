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
        private LogWindow logWindow = new LogWindow();

        // Note : 다음 요소들은 추후 UserManager가 생기면 그 아래로 이동할 요소들이다.
        private Game game = null;
        private DataManager dataManager = new DataManager();

        public MainForm()
        {
            InitializeComponent();
            this.ResizeRedraw = true;

            Log.Instance.Path = "$bball.log";
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
            dataManager.BuildData();
            this.StartNewGame();

            this.Width = Court.ImageWidth;
            this.Height = Court.ImageHeight;
            logWindow.Show(this);
            logWindow.Left = this.Right;
            logWindow.Top = this.Top;
            logWindow.Height = this.Height;
        }

        private void globalTimer_Tick(object sender, EventArgs e)
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

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F5)
                this.StartNewGame();
        }

        private void StartNewGame()
        {
            OutputManager.RemoveAll();
            game = new Game();
            if (game.Initialize(dataManager.AllTeams[0], dataManager.AllTeams[1]))
                globalTimer.Enabled = true;
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            logWindow.Height = this.Height;
        }

        private void MainForm_LocationChanged(object sender, EventArgs e)
        {
            logWindow.Left = this.Right;
            logWindow.Top = this.Top;
        }
    }
}
