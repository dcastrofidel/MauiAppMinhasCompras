using SQLite;
using System.Globalization;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        string _descricao;
        double _quantidade;
        double _preco;

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Descricao {

            get => _descricao; 
            
            set
            {
                if (value == null) 
                {
                    throw new Exception("Por Favor, preencha a descrição");
                }

                _descricao = value;

            }
                
                
        }



        public double Quantidade {

            get => _quantidade;

            set
            {
                if (value == 0)
                {
                    throw new Exception("Por Favor, preencha a quantidade");
                }

                _quantidade = value;

            }


        }


        public double Preco  {

            get => _preco;

            set
            {
                if (value == 0)
                {
                    throw new Exception("Por Favor, preencha o preço");
                }

                _preco = value;

            }


        }


        public double Total { get => Quantidade * Preco; }
    }
}