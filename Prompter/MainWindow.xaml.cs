using System;
using System.Collections.Generic;
using System.IO;
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
using WpfPageTransitions;

namespace Prompter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        System.Windows.Threading.DispatcherTimer dTimer = new System.Windows.Threading.DispatcherTimer();
        ucSlides Slides = new ucSlides();


       

        public void UpdateSlide()
        {
            PageNum.Content = $"Slide# {Docs.PageIdx + 1}";

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Switch))
                {
                    (window as Switch).lbPageIdx.Content = $"Slide# {Docs.PageIdx + 1}";
                }
            }

            ptControl.ShowPage(Slides);

           
            dTimer.Start();
        }


       


        public MainWindow()
        {
            InitializeComponent();

            Settings settings = new Settings();

            settings.LoadSettings();

            string[] dTimes = new string[] { "400","450","500", "550", "600","650","700","750","800","850","900" };
            cmdDelay.ItemsSource = dTimes;
            cmdDelay.SelectedIndex = settings.DelayIdx;

            Docs.Initialize();

            Slides.NewFdSet();

            Slides.rtbSend.Document = Docs.Fd[0];

            
            dTimer.Tick += dTimer_Tick;
            //dTimer.Interval = new TimeSpan(0, 0, 1);
            //dTimer.Interval = TimeSpan.FromMilliseconds(700);

            cmbTrans.ItemsSource = Enum.GetNames(typeof(PageTransitionType));
            cmbTrans.SelectedIndex = settings.TransIdx;

            UpdateSlide();


            


        }

        private void dTimer_Tick(object sender, EventArgs e)
        {

            Slides.rtbSend.Document = Docs.Fd[Docs.PageIdx];
            
            dTimer.Stop();
        }


        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

       

      

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Docs.PageIdx++;
            UpdateSlide();
            
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Docs.PageIdx--;
            UpdateSlide();
           
            
        }

        private void SaveFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".xaml";
            dlg.Filter = "Xaml documents (.xaml)|*.xaml";
            dlg.Title = "Video Text Box";

            Nullable<bool> result = dlg.ShowDialog();

            if(result == true)
            {

                using (StreamWriter outputFile = new StreamWriter(dlg.FileName))
                {
                    
                    for (int x = 0; x < Docs.RtbCnt; x++)
                    {
                        outputFile.WriteLine(GetContent(Docs.Fd[x]));
                    }
                }
                


                //TextRange t = new TextRange(rtbSend.Document.ContentStart, rtbSend.Document.ContentEnd);
                //FileStream file = new FileStream(dlg.FileName, FileMode.Create);
                //t.Save(file, System.Windows.DataFormats.XamlPackage);
                //file.Close();



                    //foreach (RichTextBox rtb in tools.FindVisualChildren<RichTextBox>(MyGrid))
                    //{
                    //    string fnFull;
                    //    string fnDir = System.IO.Path.GetDirectoryName(dlg.FileName);
                    //    string[] fn = dlg.SafeFileName.ToString().Split('.');
                    //    string fnDirGrp = $@"{fnDir}\{fn[0]}";

                    //    if (Directory.Exists(fnDirGrp) == false){
                    //        Directory.CreateDirectory(fnDirGrp);
                    //    }

                    //    if (rtb.Tag.ToString() == "0")
                    //    {
                    //        fnFull = $@"{fnDir}\{fn[0]}.{fn[1]}";
                    //    }
                    //    else
                    //    {
                    //        fnFull = $@"{fnDirGrp}\{fn[0]}-{rtb.Tag}.{fn[1]}";
                    //    }




                    //    //MessageBox.Show(rtb.Tag.ToString());
                    //}
                    //string[] fn = dlg.SafeFileName.ToString().Split('.');
                    //MessageBox.Show($"{dlg.InitialDirectory}{fn[0]}-x.{fn[1]}");
                    //MessageBox.Show(System.IO.Path.GetDirectoryName(dlg.FileName));
            }
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".xaml";
            dlg.Filter = "Xaml documents (.xaml)|*.xaml";
            dlg.Title = "Video Text Box";

            

            Nullable<bool> result = dlg.ShowDialog();

            if(result == true)
            {
                Docs.Fd[Docs.PageIdx].Blocks.Clear();
                Slides.NewFdSet();


                using (StreamReader inputFile = new StreamReader(dlg.FileName))
                {
                    
                    for (int x = 0; x < Docs.RtbCnt; x++)
                    {
                        LoadDataIntoFlowdocument(inputFile.ReadLine(), Docs.Fd[x]);
                        
                    }
                }

                

                

                
                TextRange tr = new TextRange(Docs.Fd[Docs.PageIdx].ContentStart, Docs.Fd[Docs.PageIdx].ContentEnd);
                
                object valueFontSize = tr.GetPropertyValue(TextElement.FontSizeProperty);
                object valueFontFamily = tr.GetPropertyValue(TextElement.FontFamilyProperty);
                object valueFontStyle = tr.GetPropertyValue(TextElement.FontStyleProperty);
                object valueForeground = tr.GetPropertyValue(TextElement.ForegroundProperty);
               



                for (int x = 0; x < Docs.RtbCnt; x++)
                {
                    Docs.Fd[x].FontSize = (double)((valueFontSize == DependencyProperty.UnsetValue) ? null : valueFontSize);
                    Docs.Fd[x].FontFamily = (FontFamily)((valueFontFamily == DependencyProperty.UnsetValue) ? null : valueFontFamily);
                    Docs.Fd[x].FontStyle = (FontStyle)((valueFontStyle == DependencyProperty.UnsetValue) ? null : valueFontStyle);
                    Docs.Fd[x].Foreground = (SolidColorBrush)((valueForeground == DependencyProperty.UnsetValue) ? null : valueForeground);
                }

                Docs.PageIdx = 0;
                UpdateSlide();

                //TextRange t = new TextRange(rtbSend.Document.ContentStart, rtbSend.Document.ContentEnd);
                //FileStream file = new FileStream(dlg.FileName, FileMode.Open);
                //t.Load(file, System.Windows.DataFormats.XamlPackage);
                //file.Close();

            }

        }


        public string GetContent(FlowDocument document)
        {
            MemoryStream ms = new MemoryStream();
            TextRange tRange = new TextRange(document.ContentStart, document.ContentEnd);
            tRange.Save(ms, DataFormats.XamlPackage);
            //tRange.Save(ms, DataFormats.Rtf);

            ms.Position = 0;

            string base64 = Convert.ToBase64String(ms.ToArray());

            return base64;

        }


        private void LoadDataIntoFlowdocument(string data, FlowDocument fdx)
        {
            byte[] content = Convert.FromBase64String(data);

            MemoryStream ms = new MemoryStream(content);

            ms.Position = 0;
            TextRange textRange = new TextRange(fdx.ContentStart, fdx.ContentEnd);
            textRange.Load(ms, DataFormats.XamlPackage);
            //textRange.Load(ms, DataFormats.Rtf);
        }

        private void NewFile(object sender, RoutedEventArgs e)
        {
            
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Start a New Project?", "Video Text Box", System.Windows.Forms.MessageBoxButtons.OKCancel);
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                Docs.Fd[Docs.PageIdx].Blocks.Clear();
                Slides.NewFdSet();
                UpdateSlide();
                
            }
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Switch SwitchWindow = new Switch();
            //SwitchWindow.Owner = this;
            //SwitchWindow.Show();

            ToolAdorner tbAdorner = null;
            tbAdorner = new ToolAdorner(dpMenu);
            AdornerLayer.GetAdornerLayer(dpMenu).Add(tbAdorner);

           
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MyGrid.RowDefinitions[1].Height = new GridLength(1);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Close Video Text Box?", "Video Text Box", System.Windows.Forms.MessageBoxButtons.OKCancel);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Settings settings = new Settings();
                settings.DelayIdx = cmdDelay.SelectedIndex;
                settings.TransIdx = cmbTrans.SelectedIndex;
                settings.SaveSettings();
                this.Close();
            }
        }

        private void cmbTrans_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           ptControl.TransitionType = (PageTransitionType)Enum.Parse(typeof(PageTransitionType), cmbTrans.SelectedItem.ToString(), true);
        }

        private void cmdDelay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Int32 delayTime = Int32.Parse(cmdDelay.SelectedItem.ToString());
            dTimer.Interval = TimeSpan.FromMilliseconds(delayTime);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Switch RemoteWindow = new Switch();
            RemoteWindow.Owner = this;
            RemoteWindow.Show();

        }
    }
}
