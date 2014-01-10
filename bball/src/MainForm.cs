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

namespace bball
{
    public partial class MainForm : Form
    {
        private IRenderer renderer = Renderer.Container.GetInterface(Renderer.Type.Direct3D9);

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
            renderer.ResizeBackBuffer(this.Width, this.Height);
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Dispose();

            if (renderer.Clear(new MyColor(Color.LightGray)))
            {
                if (renderer.BeginDraw())
                {
                    this.OnDraw(renderer);

                    if (renderer.EndDraw())
                        renderer.Flip();
                }
            }
        }

        private void frameUpdateTimer_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            renderer.ResizeBackBuffer(this.Width, this.Height);
        }
    }
}
