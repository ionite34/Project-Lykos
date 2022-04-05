namespace Project_Lykos
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            // Check FonixData.cdf exists
            if (!DependencyCheck.CheckFonixData())
            {
                DependencyDialog dialog = new();
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    Application.Exit();
                    return;
                }
            }
            Application.Run(new MainWindow());
        }
    }
}