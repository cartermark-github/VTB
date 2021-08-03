using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Controls;


namespace Prompter
{
    class ToolAdorner : Adorner
    {
        private static Geometry ascGeometry = Geometry.Parse("M 100 -4 L 103.5 -8 L 107 -4 Z");

        private static Geometry descGeometry = Geometry.Parse("M 100 -8 L 103.5 -4 L 107 -8 Z");


        private static Geometry boxGeometry = Geometry.Parse("M 0 0 L 0 -12 L 200 -12 L 200 0 Z");

        private bool MenuShow = true;

        public ToolAdorner(UIElement element)
               : base(element)
        {
            this.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(ToolAdorner_MouseDown);
            this.MouseEnter += new System.Windows.Input.MouseEventHandler(ToolAdorner_MouseEnter);
            this.Opacity = 0.5;
            this.ToolTip = "Close Menu";

        }

        private void ToolAdorner_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Cursor = System.Windows.Input.Cursors.Hand;
        }


        void ToolAdorner_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var mainWindow = (Application.Current.MainWindow as MainWindow);
            if (MenuShow == true)
            {
                mainWindow.MyGrid.RowDefinitions[1].Height = new GridLength(0);
                MenuShow = false;
                this.ToolTip = "Open Menu";
            }
            else
            {

                mainWindow.MyGrid.RowDefinitions[1].Height = new GridLength(32);
                MenuShow = true;
                this.ToolTip = "Close Menu";
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (AdornedElement.RenderSize.Width < 20)
                return;

            TranslateTransform transform = new TranslateTransform
                    (
                            (AdornedElement.RenderSize.Width / 2) - 100, 0  //(AdornedElement.RenderSize.Height)

                    );
            drawingContext.PushTransform(transform);

            Geometry geometry = ascGeometry;
            if (MenuShow == true) geometry = descGeometry;

            drawingContext.DrawGeometry(Brushes.LightBlue, null, boxGeometry);

            drawingContext.DrawGeometry(Brushes.Black, null, geometry);

            drawingContext.Pop();
        }

    }
}
