using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct3D9;

using Color = SharpDX.Color;

namespace bball
{
    public partial class MainForm : Form
    {
        struct Vertex
        {
            public Vector4 Position;
            public SharpDX.Color Color;
        }

        private Device device = null;
        private VertexBuffer vertices = null;
        private VertexDeclaration vertexDecl = null;

        public MainForm()
        {
            InitializeComponent();
            this.ResizeRedraw = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            device = new Device(new Direct3D(), 0, DeviceType.Hardware, this.Handle, CreateFlags.HardwareVertexProcessing,
                new PresentParameters(this.Width, this.Height) { PresentationInterval = PresentInterval.One });
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Dispose();
            if (vertices == null || vertexDecl == null)
                return;

            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            device.BeginScene();

            device.SetStreamSource(0, vertices, 0, 20);
            device.VertexDeclaration = vertexDecl;
            device.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);

            device.EndScene();
            device.Present();
        }

        private void frameUpdateTimer_Tick(object sender, EventArgs e)
        {
            vertices = new VertexBuffer(device, 3 * 20, Usage.WriteOnly, VertexFormat.None, Pool.Managed);
            vertices.Lock(0, 0, LockFlags.None).WriteRange(new[] {
                new Vertex() { Color = Color.Red, Position = new Vector4(400.0f, 100.0f, 0.5f, 1.0f) },
                new Vertex() { Color = Color.Blue, Position = new Vector4(650.0f, 500.0f, 0.5f, 1.0f) },
                new Vertex() { Color = Color.Green, Position = new Vector4(150.0f, 500.0f, 0.5f, 1.0f) }
            });
            vertices.Unlock();

            var vertexElems = new[] {
        		new VertexElement(0, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.PositionTransformed, 0),
        		new VertexElement(0, 16, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
				VertexElement.VertexDeclarationEnd
        	};

            vertexDecl = new VertexDeclaration(device, vertexElems);
            this.Invalidate();
        }
    }
}
