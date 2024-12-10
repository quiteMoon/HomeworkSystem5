namespace WinFormsApp1
{
    public partial class Form1 : Form
    {

        private List<ProgressBar> progressBars = new List<ProgressBar>();
        private Button addBarsButton;
        private Button startButton;
        private NumericUpDown barCountInput;

        public Form1()
        {
            Text = "Танцюючі прогрес-бари";
            Size = new Size(600, 400);

            barCountInput = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 20,
                Value = 5,
                Location = new Point(20, 20),
                Width = 100
            };
            Controls.Add(barCountInput);

            addBarsButton = new Button
            {
                Text = "Додати бари",
                Location = new Point(150, 20),
                Width = 100
            };
            addBarsButton.Click += AddBarsButton_Click;
            Controls.Add(addBarsButton);

            startButton = new Button
            {
                Text = "Запустити",
                Location = new Point(270, 20),
                Width = 100,
                Enabled = false
            };
            startButton.Click += StartButton_Click;
            Controls.Add(startButton);
        }

        private void AddBarsButton_Click(object sender, EventArgs e)
        {
            foreach (var bar in progressBars)
                Controls.Remove(bar);
            progressBars.Clear();

            int barCount = (int)barCountInput.Value;
            for (int i = 0; i < barCount; i++)
            {
                var progressBar = new ProgressBar
                {
                    Location = new Point(20, 60 + i * 30),
                    Width = 500,
                    Maximum = 100
                };
                progressBars.Add(progressBar);
                Controls.Add(progressBar);
            }

            startButton.Enabled = true;
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = false;

            List<Task> tasks = new List<Task>();
            Random random = new Random();

            foreach (var progressBar in progressBars)
            {
                tasks.Add(Task.Run(() => FillProgressBar(progressBar, random)));
            }

            await Task.WhenAll(tasks);

            MessageBox.Show("Усі прогрес-бари завершені!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            startButton.Enabled = true;
        }

        private void FillProgressBar(ProgressBar progressBar, Random random)
        {
            while (progressBar.Value < progressBar.Maximum)
            {
                Thread.Sleep(random.Next(50, 200));
                Invoke(new Action(() =>
                {
                    progressBar.Value = Math.Min(progressBar.Value + random.Next(1, 10), progressBar.Maximum);
                }));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Application.EnableVisualStyles();
            Application.Run(new Form1());
        }
    }
}
