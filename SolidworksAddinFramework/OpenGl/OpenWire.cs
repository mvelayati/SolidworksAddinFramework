using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using OpenTK.Graphics.OpenGL;
using SolidWorks.Interop.sldworks;

namespace SolidworksAddinFramework.OpenGl
{
    public abstract class Wire : IRenderable
    {
        private IList<double[]> _Points;
        private readonly float _Thickness;
        private readonly PrimitiveType _Mode;

        protected Wire(IEnumerable<double[]> points, float thickness, PrimitiveType mode)
        {
            _Points = points.ToList();
            _Thickness = thickness;
            _Mode = mode;
        }

        public void Render(DateTime time)
        {
            using (ModernOpenGl.Begin(_Mode))
            using (ModernOpenGl.SetColor(this.Color, ShadingModel.Smooth))
            using (ModernOpenGl.SetLineWidth(_Thickness))
            {
                _Points.ForEach(GL.Vertex3);
            }
        }

        public Color Color { get; set; } = System.Drawing.Color.Blue;

        public void ApplyTransform(IMathTransform transform)
        {
            _Points = _Points
                .Select(p =>
                {
                    DenseMatrix rotation;
                    DenseVector translation;
                    transform.ExtractTransform(out rotation, out translation);
                    var pv = rotation*p + translation;
                    return pv.Values;
                }).ToList();
        }
    }

    public class OpenWire : Wire
    {
        public OpenWire(IEnumerable<double[]> points, float thickness)
            : base(points, thickness, PrimitiveType.LineStrip)
        { }
    }

    public class ClosedWire : Wire
    {
        public ClosedWire(IEnumerable<double[]> points, float thickness)
            : base(points, thickness, PrimitiveType.LineLoop)
        { }
    }
}