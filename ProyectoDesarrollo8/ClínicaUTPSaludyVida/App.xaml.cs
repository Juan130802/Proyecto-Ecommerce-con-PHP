using Clínica_UTP_Salud_y_Vida.Models;
using Clínica_UTP_Salud_y_Vida.Views;
using Timer = System.Timers.Timer;

namespace Clínica_UTP_Salud_y_Vida
{
    public partial class App : Application
    {
        private Timer _timer;

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new Bienvenida());
            StartDailyReportTimer();
        }

        private async Task GenerarReporteDiario()
        {
            var dbService = new LocalDbService();
            await dbService.GenerarReporteDiario();
        }

        private void StartDailyReportTimer()
        {
            var now = DateTime.Now;
            var targetTime = new DateTime(now.Year, now.Month, now.Day, 17, 40, 0);

            if (now > targetTime)
            {
                targetTime = targetTime.AddDays(1);
            }

            var timeToWait = targetTime - now;

            _timer = new Timer(timeToWait.TotalMilliseconds);
            _timer.Elapsed += async (sender, e) =>
            {
                _timer.Stop();
                await GenerarReporteDiario();
                StartDailyReportTimer();
            };
            _timer.AutoReset = false;
            _timer.Start();
        }
    }
}
