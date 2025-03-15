using MauiAppMinhasComprasRafonha.Helpers;

namespace MauiAppMinhasComprasRafonha
{
    public partial class App : Application
    {
        static SQLiteDatabaseHelper _db; // tornando o banco de dados do sqlite disponivel

        public static SQLiteDatabaseHelper Db
        {
            get
            {
                if (_db == null)
                {
                    string path = Path.Combine( // o caminho até o banco de dados b3
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData),
                        "banco_sqlite_compras.db3");

                    _db = new SQLiteDatabaseHelper(path);
                }

                return _db;
            }
        }

        public App()
        {
            InitializeComponent();

            //MainPage = new AppShell();
            MainPage = new NavigationPage(new Views.ListaProduto());
        }
    }
}
