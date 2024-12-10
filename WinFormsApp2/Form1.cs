namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        private List<ProgressBar> horseProgressBars = new List<ProgressBar>();
        private Button startButton;
        private Label resultLabel;
        private Random random = new Random();
        private const int NumberOfHorses = 5;

        public Form1()
        {
            Text = "Емуляція кінних перегонів";
            Size = new Size(600, 600);

            startButton = new Button
            {
                Text = "Старт",
                Location = new Point(250, 20),
                Width = 100
            };
            startButton.Click += StartButton_Click;
            Controls.Add(startButton);

            for (int i = 0; i < NumberOfHorses; i++)
            {
                var progressBar = new ProgressBar
                {
                    Location = new Point(50, 60 + i * 50),
                    Width = 500,
                    Maximum = 100
                };
                horseProgressBars.Add(progressBar);
                Controls.Add(progressBar);

                var horseLabel = new Label
                {
                    Text = $"Кінь {i + 1}",
                    Location = new Point(10, 60 + i * 50),
                    Width = 40
                };
                Controls.Add(horseLabel);
            }

            resultLabel = new Label
            {
                Location = new Point(50, 60 + NumberOfHorses * 50),
                Width = 500,
                Height = 50,
                Text = "Результати будуть тут після фінішу.",
                AutoSize = true
            };
            Controls.Add(resultLabel);
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = false;
            resultLabel.Text = "Гонка розпочалася!";

            List<int> finishOrder = new List<int>();

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < horseProgressBars.Count; i++)
            {
                int horseIndex = i;
                tasks.Add(Task.Run(() => RunHorse(horseProgressBars[horseIndex], horseIndex, finishOrder)));
            }

            await Task.WhenAll(tasks);

            resultLabel.Text = "Результати гонки:\n";
            for (int i = 0; i < finishOrder.Count; i++)
            {
                resultLabel.Text += $"{i + 1}-е місце: Кінь {finishOrder[i] + 1}\n";
            }

            startButton.Enabled = true;
        }

        private void RunHorse(ProgressBar progressBar, int horseIndex, List<int> finishOrder)
        {
            while (progressBar.Value < progressBar.Maximum)
            {
                Thread.Sleep(random.Next(50, 200));
                Invoke(new Action(() =>
                {
                    progressBar.Value = Math.Min(progressBar.Value + random.Next(1, 5), progressBar.Maximum);
                }));
            }

            lock (finishOrder)
            {
                finishOrder.Add(horseIndex);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Application.EnableVisualStyles();
            Application.Run(new Form1());
        }
    }
}
