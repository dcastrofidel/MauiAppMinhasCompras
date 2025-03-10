using MauiAppMinhasCompras.Helpers;

namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        static SQLiteDatabaseHelper _db;

        public static SQLiteDatabaseHelper Db
        {//Instanciamento da classe do SQLite database helper para acessar o Crud, as API,pacotes Nuget e metodos, tornando a classe(SQLiteDataBaseHelper) dispónivel para acesso em todo o app, fazendo assim alterações em qualquer parte dele
            get
            {
                if(_db == null)
                {
                    string path = Path.Combine(
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

            
            MainPage = new NavigationPage(new Views.ListaProduto());
        }
    }
}
