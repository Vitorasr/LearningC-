using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Files.SystemMonitor;

namespace MonitorDeRecursos
{
    public class MainForm : Form
    {
        private readonly SystemMonitor _monitor;
        private Label _cpuUsageLabel;
        private Label _cpuTempLabel;
        private Label _gpuUsageLabel;
        private Label _gpuTempLabel;
        private Label _memoryLabel;
        private Label _diskLabel;
        private Label _networkLabel;
        private Label _processesLabel;
        private System.Windows.Forms.Timer _updateTimer;

        public MainForm()
        {
            _monitor = new SystemMonitor();
            InitializeComponents();
            StartUpdateTimer();
        }

        private void InitializeComponents()
        {
            Text = "Monitor de Recursos";
            Size = new Size(600, 400);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = true;
            AutoScroll = true;
            Icon = new Icon("appicon.ico");

            Font boldFont = new Font("Segoe UI", 9, FontStyle.Bold);
            Font regularFont = new Font("Segoe UI", 9, FontStyle.Regular);

            var cpuUsageTitleLabel = new Label { Location = new Point(10, 10), Size = new Size(100, 20), Text = "Uso de CPU:", Font = boldFont };
            _cpuUsageLabel = new Label { Location = new Point(110, 10), Size = new Size(480, 20), Text = "Carregando...", Font = regularFont };

            var cpuTempTitleLabel = new Label { Location = new Point(10, 40), Size = new Size(150, 20), Text = "Temperatura da CPU:", Font = boldFont };
            _cpuTempLabel = new Label { Location = new Point(160, 40), Size = new Size(430, 60), Text = "Carregando...", Font = regularFont };

            var gpuUsageTitleLabel = new Label { Location = new Point(10, 110), Size = new Size(100, 20), Text = "Uso da GPU:", Font = boldFont };
            _gpuUsageLabel = new Label { Location = new Point(110, 110), Size = new Size(480, 20), Text = "Carregando...", Font = regularFont };

            var gpuTempTitleLabel = new Label { Location = new Point(10, 140), Size = new Size(150, 20), Text = "Temperatura da GPU:", Font = boldFont };
            _gpuTempLabel = new Label { Location = new Point(160, 140), Size = new Size(430, 20), Text = "Carregando...", Font = regularFont };

            // Título e valor para Memória
            var memoryTitleLabel = new Label { Location = new Point(10, 170), Size = new Size(100, 20), Text = "Memória:", Font = boldFont };
            _memoryLabel = new Label { Location = new Point(110, 170), Size = new Size(480, 20), Text = "Carregando...", Font = regularFont };

            // Título e valor para Armazenamento
            var diskTitleLabel = new Label { Location = new Point(10, 200), Size = new Size(120, 20), Text = "Armazenamento:", Font = boldFont };
            _diskLabel = new Label { Location = new Point(130, 200), Size = new Size(460, 60), Text = "Carregando...", Font = regularFont };

            // Título e valor para Redes
            var networkTitleLabel = new Label { Location = new Point(10, 270), Size = new Size(100, 20), Text = "Redes:", Font = boldFont };
            _networkLabel = new Label { Location = new Point(110, 270), Size = new Size(480, 60), Text = "Carregando...", Font = regularFont };

            // Título e valor para Processos
            var processesTitleLabel = new Label { Location = new Point(10, 340), Size = new Size(100, 20), Text = "Processos:", Font = boldFont };
            _processesLabel = new Label { Location = new Point(110, 340), Size = new Size(480, 200), Text = "Carregando...", Font = regularFont };

            // Adicionar todos os rótulos ao formulário
            Controls.AddRange(new Control[] {
                cpuUsageTitleLabel, _cpuUsageLabel,
                cpuTempTitleLabel, _cpuTempLabel,
                gpuUsageTitleLabel, _gpuUsageLabel,
                gpuTempTitleLabel, _gpuTempLabel,
                memoryTitleLabel, _memoryLabel,
                diskTitleLabel, _diskLabel,
                networkTitleLabel, _networkLabel,
                processesTitleLabel, _processesLabel
            });

            FormClosing += (s, e) => _updateTimer.Stop();
        }
        private void StartUpdateTimer()
        {
            _updateTimer = new System.Windows.Forms.Timer { Interval = 10000 };
            _updateTimer.Tick += (s, e) => UpdateData();
            _updateTimer.Start();
            UpdateData();
        }

        private void UpdateData()
        {
            try
            {
                _cpuUsageLabel.Text = $"{_monitor.GetCurrentCpuUsage():F2}%";

                using (var cpuTempWriter = new StringWriter())
                {
                    Console.SetOut(cpuTempWriter);
                    _monitor.PrintCpuTemperature();
                    string cpuTempText = cpuTempWriter.ToString().Trim();
                    string[] lines = cpuTempText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                    cpuTempText = string.Join(Environment.NewLine, lines.Take(3));
                    _cpuTempLabel.Text = cpuTempText;
                    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
                }
                using (var gpuUsageWriter = new StringWriter())
                {
                    Console.SetOut(gpuUsageWriter);
                    _monitor.PrintGpuUsage();
                    _gpuUsageLabel.Text = gpuUsageWriter.ToString().Trim();
                    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
                }
                using (var gpuTempWriter = new StringWriter())
                {
                    Console.SetOut(gpuTempWriter);
                    _monitor.PrintGpuTemperature();
                    _gpuTempLabel.Text = gpuTempWriter.ToString().Trim();
                    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
                }

                var (totalMem, freeMem) = _monitor.GetMemoryUsage();
                float usedMem = totalMem - freeMem;
                float memPercent = totalMem > 0 ? (float)usedMem / totalMem * 100 : 0;
                _memoryLabel.Text = $"Memória: {usedMem:F2} MB usados de {totalMem:F2} MB ({memPercent:F2}%)";

                using (var diskWriter = new StringWriter())
                {
                    Console.SetOut(diskWriter);
                    _monitor.PrintDiskUsage();
                    _diskLabel.Text = diskWriter.ToString().Trim();
                    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
                }

                using (var networkWriter = new StringWriter())
                {
                    Console.SetOut(networkWriter);
                    _monitor.PrintNetworkUsage();
                    _networkLabel.Text = networkWriter.ToString().Trim();
                    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
                }

                using (var processesWriter = new StringWriter())
                {
                    Console.SetOut(processesWriter);
                    _monitor.PrintTopProcesses();
                    _processesLabel.Text = processesWriter.ToString().Trim();
                    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}