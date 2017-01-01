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

        private UnoGame game;

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

        private double discard_left1 = 350.0;

        private double discard_left2 = 450.0;

        private double[,] players_top_left = new double[4, 2] { { 0, 0 }, { 288, 832 }, { 51, 648 }, { 126, 87 } };

        private double player1_top = 288.0;

        private double player1_left = 832.0;

        private double player2_top = 51.0;

        private double player2_left = 648.0;

        private double player3_top = 126.0;

        private double player3_left = 87.0;


        private void img_MouseUp(object sender, MouseButtonEventArgs e)
        {

            double left = ran.Next((int)discard_left1, (int)discard_left2);
            double top = ran.Next((int)discard_top1, (int)discard_top2);
            int angle = ran.Next(-30, 30);

            Image i = (Image)e.Source;
            if (game.currentPlayer != 0 || !game.canPlay) { return; }
            if (!game.Play(0, imgs.IndexOf((Image)i))) { return; }
            color_show(game.lastColor);
            FloatInElement(top, left, i);
            i.RenderTransform = new RotateTransform(angle, i.ActualWidth / 2, i.ActualHeight / 2);
            imgs.Remove(i);
            i.MouseLeave -= img_MouseLeave;
            i.MouseEnter -= img_MouseEnter;
            i.MouseUp -= img_MouseUp;
            refresh();
            if (game.isReversed) { arrow.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Clockwise_arrow.svg.png")); }
            else { arrow.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Counterclockwise_arrow.svg.png")); }

            if (game.gameOver)
            {
                gameover();
            }
            if (!game.lastColor.Equals("Wild"))
            {
                game.Next();
                autoplay_show();
            }
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


        private void init_imgs(int n = 7)
        {
            try
            {
                List<Card> hand = game.GetHand(0);
                imgs = new List<Image>();
                for (int i = 0; i < n; i++)
                {
                    Image img_i = new Image();
                    img_i.Height = 100;
                    img_i.Width = 100;
                    img_i.SetValue(Canvas.TopProperty, hand_down_top);
                    img_i.SetValue(Canvas.LeftProperty, (double)(125 + (500 / n) * i));
                    img_i.Source = new BitmapImage(new Uri(hand[i].getResourceUri()));
                    img_i.MouseEnter += img_MouseEnter;
                    img_i.MouseLeave += img_MouseLeave;
                    img_i.MouseUp += img_MouseUp;

                    imgs.Add(img_i);
                    canvas.Children.Add(imgs[i]);
                }
                Image img_first = new Image();
                img_first.Height = 100;
                img_first.Width = 100;
                img_first.SetValue(Canvas.TopProperty, discard_top1);
                img_first.SetValue(Canvas.LeftProperty, discard_left1);
                img_first.Source = new BitmapImage(new Uri(game.discard.Last().getResourceUri()));

                canvas.Children.Add(img_first);

                arrow.Visibility = Visibility.Visible;

                numOfHand1.Visibility = Visibility.Visible;
                numOfHand2.Visibility = Visibility.Visible;
                numOfHand3.Visibility = Visibility.Visible;

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

        private void draw()
        {
            if (game.currentPlayer != 0) { return; }
            if (!game.canDraw) { return; }
            List<Card> cards = game.Draw(0);
            int n = cards.Count;
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

                    img_i.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/card_back_alt_large.png"));

                    canvas.Children.Add(img_i);

                    Image img_t = new Image();
                    img_t.Height = 100;
                    img_t.Width = 100;
                    img_t.SetValue(Canvas.TopProperty, hand_down_top);
                    img_t.SetValue(Canvas.LeftProperty, hand_donw_right);
                    img_t.Source = new BitmapImage(new Uri(cards[i].getResourceUri()));
                    img_t.Visibility = Visibility.Hidden;

                    img_t.MouseEnter += img_MouseEnter;
                    img_t.MouseLeave += img_MouseLeave;
                    img_t.MouseUp += img_MouseUp;
                    imgs.Add(img_t);
                    canvas.Children.Add(img_t);

                    FloatInElement(hand_down_top, hand_donw_right, img_i);
                }
            }
            catch (Exception ee) { }

            if (game.canPass) { pass_label.Visibility = Visibility.Visible; } else { game.Next(); autoplay_show(); }
        }

        private void play()
        { }

        private void othersPlay(int playerIndex, string cardURI, int count)
        {
            try
            {
                Image img = new Image();
                img.Height = 100;
                img.Width = 100;
                img.SetValue(Canvas.TopProperty, players_top_left[playerIndex, 0]);
                img.SetValue(Canvas.LeftProperty, players_top_left[playerIndex, 1]);
                img.Source = new BitmapImage(new Uri(cardURI));
                canvas.Children.Add(img);

                FloatInElement(ran.Next((int)discard_top1, (int)discard_top2), ran.Next((int)discard_left1, (int)discard_left2), img, count);
            }
            catch (Exception ee) { }
        }

        private void deck_MouseUp(object sender, MouseButtonEventArgs e)
        {
            draw();
        }

        private void challenge()
        { }
        private void chooseColor()
        { }
        private void gameover()
        {
            if (game.winner == 0)
            { win_label.Visibility = Visibility.Visible; }
            else
            { lose_label.Visibility = Visibility.Visible; }
        }
        private void restart()
        { }

        private void start_MouseUp(object sender, MouseButtonEventArgs e)
        {
            game = new UnoGame(4);

            start.Visibility = Visibility.Hidden;

            deck.Visibility = Visibility.Visible;
            player1.Visibility = Visibility.Visible;
            player2.Visibility = Visibility.Visible;
            player3.Visibility = Visibility.Visible;

            init_imgs();
        }

        public void FloatInElement(double top, double left, UIElement elem, int beginTime = 0)
        {
            Canvas.SetZIndex(elem, Canvas.GetZIndex(player1) - 1);

            try
            {
                DoubleAnimation floatY = new DoubleAnimation()
                {
                    BeginTime = new TimeSpan(0, 0, 0, beginTime, 0),
                    To = top,
                    Duration = new TimeSpan(0, 0, 0, 0, 300),
                };
                DoubleAnimation floatX = new DoubleAnimation()
                {
                    BeginTime = new TimeSpan(0, 0, 0, beginTime, 0),
                    To = left,
                    Duration = new TimeSpan(0, 0, 0, 0, 300),
                };

                floatX.Completed += (s, e) =>
                {
                    Canvas.SetZIndex(elem, ++numOfZindex);
                    if (floatY.To > 450.0)
                    {
                        canvas.Children.Remove(elem);
                        foreach (Image img in imgs)
                        {
                            img.Visibility = Visibility.Visible;

                        }

                        refresh();
                    }
                    else
                    {
                        int angle = ran.Next(-30, 30);
                        elem.RenderTransform = new RotateTransform(angle, ((Image)elem).ActualWidth / 2, ((Image)elem).ActualHeight / 2);
                    }
                    numOfHand1.Content = game.GetHandCount(1).ToString();
                    numOfHand2.Content = game.GetHandCount(2).ToString();
                    numOfHand3.Content = game.GetHandCount(3).ToString();
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

        private void pass_label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (game.currentPlayer != 0) { return; }
            if (!game.canPass) { return; }
            game.Pass(0);
            game.Next();
            pass_label.Visibility = Visibility.Hidden;
            autoplay_show();
        }

        private void autoplay_show()
        {
            pass_label.Visibility = Visibility.Hidden;
            int count = 0;
            while (game.currentPlayer != 0)
            {
                count++;
                Card card = game.AutoPlay();
                if (card != null)
                {
                    othersPlay(game.prevPlayer, card.getResourceUri(), count);

                }
                if (game.gameOver)
                {
                    gameover();
                    return;
                }


            }
            color_show(game.lastColor);
            if (game.isReversed) { arrow.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Clockwise_arrow.svg.png")); }
            else { arrow.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Counterclockwise_arrow.svg.png")); }

        }

        private void color_show(string color)
        {
            color_red.Visibility = Visibility.Hidden;
            color_yellow.Visibility = Visibility.Hidden;
            color_blue.Visibility = Visibility.Hidden;
            color_green.Visibility = Visibility.Hidden;


            if (color.Equals("Red"))
            { color_red.Visibility = Visibility.Visible; }
            else if (color.Equals("Green"))
            { color_green.Visibility = Visibility.Visible; }
            else if (color.Equals("Blue"))
            { color_blue.Visibility = Visibility.Visible; }
            else if (color.Equals("Yellow"))
            { color_yellow.Visibility = Visibility.Visible; }
            else if (color.Equals("Wild"))
            {
                color_red.Visibility = Visibility.Visible;
                color_yellow.Visibility = Visibility.Visible;
                color_blue.Visibility = Visibility.Visible;
                color_green.Visibility = Visibility.Visible;
            }
        }

        private void color_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (game.currentPlayer != 0) { return; }
            if (!game.lastColor.Equals("Wild")) { return; }


            Image c = (Image)e.Source;
            if (c.Equals(color_blue))
            {
                game.setWildColor(0, "Blue");
            }
            else if (c.Equals(color_red))
            {
                game.setWildColor(0, "Red");
            }
            else if (c.Equals(color_yellow))
            {
                game.setWildColor(0, "Yellow");
            }
            else if (c.Equals(color_green))
            {
                game.setWildColor(0, "Green");
            }

            game.Next();
            autoplay_show();
        }


    }
}
