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

using System.Windows.Media.Animation;

namespace Uno
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public int num { get; set; }

        public List<Image> imgs { get; set; }

        private int numOfZindex = 20;

        Random ran = new Random();

        private double deck_top = 407.0;

        private double deck_left = 55.0;

        private double hand_down_top = 550.0;

        private double hand_donw_right = 650.0;

        private double hand_up_top = 490.0;

        private double hand_up_right = 650.0;

        private double discard_top1 = 220.0;

        private double discard_top2 = 260.0;

        private double discard_left1 = 300.0;

        private double discard_left2 = 400.0;

        private double player1_top = 288.0;

        private double player1_left = 832.0;

        private double player2_top = 51.0;

        private double player2_left = 648.0;

        private double player3_top = 126.0;

        private double player3_left = 87.0;



        private void init_card()
        {
            try
            {
                //Image img = new Image();
                
                Image img2 = new Image();
                img2.Height = 100;
                img2.Width = 100;
                img2.SetValue(Canvas.TopProperty, 200.0);
                img2.SetValue(Canvas.LeftProperty, 100.0);
                img2.MouseUp += img_MouseUp;
                img2.MouseEnter += img_MouseEnter;
                img2.MouseLeave += img_MouseLeave;
                img2.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/blue_0_large.png"));
                
                canvas.Children.Add(img2);
            }
            catch (Exception ee) { }

            
        }

        private void img_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
            double left = ran.Next((int)discard_left1, (int)discard_left2);
            double top = ran.Next((int)discard_top1, (int)discard_top2);
            int angle = ran.Next(-30, 30);



            Image i = (Image)e.Source;
            FloatInElement(top, left, i);
            //i.SetValue(Canvas.TopProperty, top);
            //i.SetValue(Canvas.LeftProperty, left);
            i.RenderTransform = new RotateTransform(angle, i.ActualWidth / 2, i.ActualHeight / 2);
            Canvas.SetZIndex(i, ++numOfZindex);

            /*
            Image i = (Image)e.Source;
            Image img_tmp = new Image();
            img_tmp.Source = i.Source;
            img_tmp.Height = i.Height;
            img_tmp.Width = i.Width;
            img_tmp.SetValue(Canvas.TopProperty, 250.0);
            img_tmp.SetValue(Canvas.LeftProperty, left);
            img_tmp.RenderTransform = new RotateTransform(angle, img_tmp.ActualWidth / 2, img_tmp.ActualHeight / 2);
            */
            //canvas.Children.Add(i);
            imgs.Remove(i);
            i.MouseLeave -= img_MouseLeave;
            i.MouseEnter -= img_MouseEnter;
            i.MouseUp -= img_MouseUp;
            //canvas.Children.Remove(i);
            refresh();
            
        }

        private void img_MouseEnter(object sender, MouseEventArgs e)
        {
            Image i = (Image)e.Source;
            i.SetValue(Canvas.TopProperty, hand_up_top);
            
        }

        private void img_MouseLeave(object sender, MouseEventArgs e)
        {
            Image i = (Image)e.Source;
            i.SetValue(Canvas.TopProperty, hand_down_top);
        }



        private void init_imgs(int n = 20)
        {
            try
            {
                imgs = new List<Image>();
                for (int i = 0; i < n; i++)
                {
                    Image img_i = new Image();
                    img_i.Height = 100;
                    img_i  .Width = 100;
                    img_i.SetValue(Canvas.TopProperty, hand_down_top);
                    img_i.SetValue(Canvas.LeftProperty, (double)(125 + (500 / n) * i));
                    img_i.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/blue_0_large.png"));
                    img_i.MouseEnter += img_MouseEnter;
                    img_i.MouseLeave += img_MouseLeave;
                    img_i.MouseUp += img_MouseUp;

                    imgs.Add(img_i);
                    canvas.Children.Add(imgs[i]);
                }
            }
            catch (Exception ee) { }
        }

        private void refresh()
        {
            for (int i = 0; i < imgs.Count; i++)
            {

                imgs[i].SetValue(Canvas.LeftProperty, (double)(125 + (500 / imgs.Count) * i));

            }
        }

        private void draw(int n = 1)
        {
            try
            {
                int angle = ran.Next(-30, 30);
                for (int i = 0; i < n; i++)
                {
                    Image img_i = new Image();
                    img_i.Height = 100;
                    img_i.Width = 100;
                    img_i.SetValue(Canvas.TopProperty, deck_top);
                    img_i.SetValue(Canvas.LeftProperty, deck_left);
                    img_i.RenderTransform = new RotateTransform(angle, img_i.ActualWidth / 2, img_i.ActualHeight / 2);
                    //img_i.SetValue(Canvas.LeftProperty, (double)(125 + (500 / n) * i));

                    img_i.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/card_back_alt_large.png"));
                    //imgs.Add(img_i);
                    //refresh();
                    canvas.Children.Add(img_i);

                    Image img_t = new Image();
                    img_t.Height = 100;
                    img_t.Width = 100;
                    img_t.SetValue(Canvas.TopProperty, hand_down_top);
                    img_t.SetValue(Canvas.LeftProperty, hand_donw_right);
                    img_t.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/green_0_large.png"));
                    img_t.Visibility = Visibility.Hidden;
                    imgs.Add(img_t);
                    canvas.Children.Add(img_t);

                    //canvas.Children.Add(imgs[imgs.Count - 1]);
                    FloatInElement( hand_down_top, hand_donw_right, img_i);

                    
                    //imgs.Add(img_i);
                    //refresh();
                    

                    //img_i.MouseEnter += img_MouseEnter;
                    //img_i.MouseLeave += img_MouseLeave;
                    //img_i.MouseUp += img_MouseUp;

                }
            }
            catch (Exception ee) { }
            
        }

        private void play()
        { }

        private void othersPlay(int playerIndex)
        { }

        private void deck_MouseUp(object sender, MouseButtonEventArgs e)
        {
            draw();
        }

        private void challenge()
        { }
        private void chooseColor()
        { }
        private void gameover()
        { }
        private void restart()
        { }

        private void start_MouseUp(object sender, MouseButtonEventArgs e)
        {
            start.Visibility = Visibility.Hidden;
            //color_blue.Visibility = Visibility.Visible;
            //color_green.Visibility = Visibility.Visible;
            //color_red.Visibility = Visibility.Visible;
            //color_yellow.Visibility = Visibility.Visible;
            deck.Visibility = Visibility.Visible;
            player1.Visibility = Visibility.Visible;
            player2.Visibility = Visibility.Visible;
            player3.Visibility = Visibility.Visible;
            init_card();
            init_imgs();


        }

        public void FloatInElement(double top, double left, UIElement elem)
         {
             try
             {
                 DoubleAnimation floatY = new DoubleAnimation()
                 {
                     To = top,
                     Duration = new TimeSpan(0, 0, 0, 0, 300),
                 };
                 DoubleAnimation floatX = new DoubleAnimation()
                 {
                     To = left,
                     Duration = new TimeSpan(0, 0, 0, 0, 300),
                 };

                floatX.Completed += (s, e) =>
                {
                    if (floatY.To > 450.0)
                    {
                        canvas.Children.Remove(elem);
                        foreach(Image img in imgs)
                        {
                            img.Visibility = Visibility.Visible;
                            img.MouseEnter += img_MouseEnter;
                            img.MouseLeave += img_MouseLeave;
                            img.MouseUp += img_MouseUp;
                        }

                        refresh();
                    }
                };
                elem.BeginAnimation(Canvas.TopProperty, floatY);
                 elem.BeginAnimation(Canvas.LeftProperty, floatX);
                
            }
             catch (Exception)
             {
 
                 throw;
             }
            
         }

        private void changeBackground()
        {
            ImageBrush b = new ImageBrush();
            b.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/gameplay.png"));
            canvas.Background = b;
        }

       
    }
}
