using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfColorFontDialog;

namespace Prompter
{
    /// <summary>
    /// Interaction logic for ucSlides.xaml
    /// </summary>
    public partial class ucSlides : UserControl
    {
        
        public ucSlides()
        {
            InitializeComponent();

            
        }

        private void CloseMenu_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (Application.Current.MainWindow as MainWindow);
            if (mainWindow != null)
            {
                mainWindow.Close();
                
                
            }
        }


        public void NewFdSet()
        {
            rtbStyle.Foreground = Brushes.Black;
            rtbStyle.FontSize = 14;
            rtbStyle.FontWeight = FontWeights.Normal;
            rtbStyle.FontFamily = new FontFamily("Arial");

            
            rtbSend.Foreground = Brushes.Black;
            rtbSend.FontSize = 14;
            rtbSend.FontWeight = FontWeights.Normal;
            rtbSend.FontFamily = new FontFamily("Arial");
            for (int x = 0; x < Docs.RtbCnt; x++)
            {

                FlowDocument p = new FlowDocument();
                Docs.Fd[x] = p;
                Docs.Fd[x].Blocks.Clear();
            }
            Docs.PageIdx = 0;

        }

        public void FontStudioMenu_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (Application.Current.MainWindow as MainWindow);

            ColorFontDialog cfd = new ColorFontDialog(true, true);
            cfd.Title = "Font Studio";
            cfd.Owner = mainWindow;
            cfd.ShowInTaskbar = false;

            cfd.Font = FontInfo.GetControlFont(rtbSend);


            TextRange tr = new TextRange(Docs.Fd[Docs.PageIdx].ContentStart, Docs.Fd[Docs.PageIdx].ContentEnd);
            


            try
            {
                object value;
                value = tr.GetPropertyValue(TextElement.FontSizeProperty);
                cfd.Font.Size = (double)((value == DependencyProperty.UnsetValue) ? null : value);
                value = tr.GetPropertyValue(TextElement.FontFamilyProperty);
                cfd.Font.Family = (FontFamily)((value == DependencyProperty.UnsetValue) ? null : value);
                value = tr.GetPropertyValue(TextElement.FontStyleProperty);
                cfd.Font.Style = (FontStyle)((value == DependencyProperty.UnsetValue) ? null : value);
                value = tr.GetPropertyValue(TextElement.ForegroundProperty);
                cfd.Font.BrushColor = (SolidColorBrush)((value == DependencyProperty.UnsetValue) ? null : value);



                if (cfd.Font.Family == null || cfd.Font.Style == null)
                {
                    MessageBox.Show("Not able to get fonts from slide.");
                    return;

                }


                if (cfd.ShowDialog() == true)
                {
                    FontInfo font = cfd.Font;
                    if (font != null)
                    {
                        FontInfo.ApplyFont(rtbStyle, font);
                        FontInfo.ApplyFont(rtbSend, font);

                       

                        for (int x = 0; x < Docs.RtbCnt; x++)
                        {
                            TextRange textRange = new TextRange(Docs.Fd[x].ContentStart, Docs.Fd[x].ContentEnd);
                            textRange.ApplyPropertyValue(TextElement.ForegroundProperty, font.BrushColor);
                            textRange.ApplyPropertyValue(TextElement.FontFamilyProperty, font.Family);
                            textRange.ApplyPropertyValue(TextElement.FontSizeProperty, font.Size);
                            textRange.ApplyPropertyValue(TextElement.FontStyleProperty, font.Style);




                            //fd[x].FontFamily = font.Family;
                            //fd[x].FontSize = font.Size;
                            //fd[x].FontStyle = font.Style;
                            //fd[x].Foreground = font.BrushColor;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was a problem setting fonts on this slide, try a different slide.");
            }
        }

        private void rtbSend_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {

            rtbSend.ContextMenu.Items.Clear();

            SpellingError SpellErrors;
            SpellErrors = rtbSend.GetSpellingError(rtbSend.CaretPosition);

            int cmdIndex = 0;
            if (SpellErrors != null)
            {
                foreach (string str in SpellErrors.Suggestions)
                {
                    MenuItem mi = new MenuItem();
                    mi.Header = str;
                    mi.FontWeight = FontWeights.Bold;
                    mi.Command = EditingCommands.CorrectSpellingError;
                    mi.CommandParameter = str;
                    mi.CommandTarget = rtbSend;
                    rtbSend.ContextMenu.Items.Add(mi);
                    cmdIndex++;
                }
                rtbSend.ContextMenu.Items.Add(new Separator());
            }


            MenuItem mic = new MenuItem();
            mic.Header = "Font Studio";
            mic.Click += FontStudioMenu_Click;
            rtbSend.ContextMenu.Items.Add(mic);

            rtbSend.ContextMenu.Items.Add(new Separator());

           
            mic = new MenuItem();
            mic.Header = "Undo";
            mic.Command = ApplicationCommands.Undo;
            mic.CommandTarget = rtbSend;
            rtbSend.ContextMenu.Items.Add(mic);

            mic = new MenuItem();
            mic.Header = "Copy";
            mic.Command = ApplicationCommands.Copy;
            mic.CommandTarget = rtbSend;
            rtbSend.ContextMenu.Items.Add(mic);

            mic = new MenuItem();
            mic.Header = "Cut";
            mic.Command = ApplicationCommands.Cut;
            mic.CommandTarget = rtbSend;
            rtbSend.ContextMenu.Items.Add(mic);

            mic = new MenuItem();
            mic.Header = "Paste";
            mic.Command = ApplicationCommands.Paste;
            mic.CommandTarget = rtbSend;
            rtbSend.ContextMenu.Items.Add(mic);

          
            rtbSend.ContextMenu.Items.Add(new Separator());

            mic = new MenuItem();
            mic.Header = "Toggle Bold";
            mic.Command = EditingCommands.ToggleBold;
            mic.CommandTarget = rtbSend;
            rtbSend.ContextMenu.Items.Add(mic);

            mic = new MenuItem();
            mic.Header = "Toggle Bullets";
            mic.Command = EditingCommands.ToggleBullets;
            mic.CommandTarget = rtbSend;
            rtbSend.ContextMenu.Items.Add(mic);

            mic = new MenuItem();
            mic.Header = "Toggle Numbering";
            mic.Command = EditingCommands.ToggleNumbering;
            mic.CommandTarget = rtbSend;
            rtbSend.ContextMenu.Items.Add(mic);

            mic = new MenuItem();
            mic.Header = "Toggle Underline";
            mic.Command = EditingCommands.ToggleUnderline;
            mic.CommandTarget = rtbSend;
            rtbSend.ContextMenu.Items.Add(mic);

            rtbSend.ContextMenu.Items.Add(new Separator());

            mic = new MenuItem();
            mic.Header = "Align Left";
            mic.Command = EditingCommands.AlignLeft;
            mic.CommandTarget = rtbSend;
            rtbSend.ContextMenu.Items.Add(mic);

            mic = new MenuItem();
            mic.Header = "Align Right";
            mic.Command = EditingCommands.AlignRight;
            mic.CommandTarget = rtbSend;
            rtbSend.ContextMenu.Items.Add(mic);

            mic = new MenuItem();
            mic.Header = "Align Center";
            mic.Command = EditingCommands.AlignCenter;
            mic.CommandTarget = rtbSend;
            rtbSend.ContextMenu.Items.Add(mic);

            mic = new MenuItem();
            mic.Header = "Align Justify";
            mic.Command = EditingCommands.AlignJustify;
            mic.CommandTarget = rtbSend;
            rtbSend.ContextMenu.Items.Add(mic);

        }
    }
}
