using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Flappybird
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private Random r = new Random();
        private int score = 0;

        private bool inGame = false;

        string selectedLevel;

        private List<String> levels = new List<string> { "normál", "esős", "ködös" };
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Tick += Timer_Tick;
            
            this.KeyDown += MainWindow_KeyDown;
            LevelsList.ItemsSource = levels;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space && inGame)
            { 
                double aktualisPoz = Canvas.GetTop(Bird);
                double sebesseg = -70;
                double ujPoz = aktualisPoz + sebesseg;
                if (ujPoz > -20)
                {
                    Canvas.SetTop(Bird, ujPoz);
                    
                }
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            BirdGravity();
            Pipe_Move();
            Pipe_Generate();
            Death();

        }

        private void Death()
        {
            if (((Canvas.GetTop(Bird) < Canvas.GetTop(PipeTop) + PipeTop.Height || Canvas.GetTop(Bird) + Bird.Height < Canvas.GetLeft(PipeBottom)) && Canvas.GetLeft(Bird) + Bird.Width > Canvas.GetLeft(PipeTop) && Canvas.GetLeft(Bird) < Canvas.GetLeft(PipeTop) + PipeTop.Width)
                || Canvas.GetTop(Bird) >= 404)
            {
                inGame = false;
                timer.Stop();
                MessageBox.Show("Game Over!");
                StartBtn.IsEnabled = true;

                Canvas.SetTop(Bird, 200);
                Canvas.SetLeft(PipeBottom, 800);
                Canvas.SetLeft(PipeTop, 800);
                score = 0;
                Levels.Opacity = 1;
                LevelsList.Opacity = 1;
            }

        }

        private void Pipe_Generate()
        {
            int hatarvonal = -50;
            if (Canvas.GetLeft(PipeBottom) == hatarvonal)
            {
                int alsoCsatornaPoz = r.Next(234, 350);
                int felsoCsatornaPoz = alsoCsatornaPoz - 370;
                Canvas.SetTop(PipeBottom, alsoCsatornaPoz);
                Canvas.SetTop(PipeTop, felsoCsatornaPoz);
                Canvas.SetLeft(PipeBottom, 800);
                Canvas.SetLeft(PipeTop, 800);
            }
        }

        private void Pipe_Move()
        {
            double pipeSpeed = 5;
            double aktualisTopPoz = Canvas.GetLeft(PipeTop);
            double aktualisBottomPoz = Canvas.GetLeft(PipeBottom);
            Canvas.SetLeft(PipeTop, aktualisTopPoz - pipeSpeed);
            Canvas.SetLeft(PipeBottom, aktualisBottomPoz - pipeSpeed);


            if (Canvas.GetLeft(PipeTop) + PipeTop.Width < Canvas.GetLeft(Bird) - Bird.Width - 65)
            {
                score += 1;
                ScoreText.Text = "Score: " + score;
            }
        }

        private void BirdGravity()
        {
            double aktualisPoz = Canvas.GetTop(Bird);
            double sebesseg = 4;
            double ujPoz = aktualisPoz + sebesseg;
            Canvas.SetTop(Bird, ujPoz);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (LevelsList.SelectedItem != null)
            {
                selectedLevel = LevelsList.SelectedItem.ToString();
                // You can implement level-specific logic here based on the selected level
                MessageBox.Show($"Selected Level: {selectedLevel}");
                inGame = true;
                score = 0;
                ScoreText.Text = "Score: 0";
                timer.Start();
                StartBtn.IsEnabled = false;
                Levels.Opacity = 0;
                LevelsList.Opacity = 0;

                if (selectedLevel == "esős")
                {
                    GameCanvas.Background = new SolidColorBrush(Color.FromRgb(100, 100, 100)); // Gray background for rainy level
                    Rain();
                }

                else if (selectedLevel == "ködös")
                    {
                       
                    }
            }
            else
            {
                MessageBox.Show("Kérlek válassz egy pályát");
            }


        }

        private void Rain()
        {
            if (new Random().Next(0, 100) < 2)// what is this? this is a random chance to spawn a rain effect
            {
                Rectangle rain = new Rectangle
                {
                    Width = 5,
                    Height = 15,
                    Fill = Brushes.Blue
                };
                Canvas.SetLeft(rain, new Random().Next(0, (int)GameCanvas.ActualWidth));
                Canvas.SetTop(rain, 0);
                GameCanvas.Children.Add(rain);
                DispatcherTimer rainTimer = new DispatcherTimer();
                rainTimer.Interval = TimeSpan.FromMilliseconds(20);
                rainTimer.Tick += (s, args) =>
                {
                    double rainTop = Canvas.GetTop(rain);
                    Canvas.SetTop(rain, rainTop + 5);
                    if (rainTop > GameCanvas.ActualHeight)
                    {
                        GameCanvas.Children.Remove(rain);
                        rainTimer.Stop();
                    }
                };
                rainTimer.Start();
            }
        }
    }
}