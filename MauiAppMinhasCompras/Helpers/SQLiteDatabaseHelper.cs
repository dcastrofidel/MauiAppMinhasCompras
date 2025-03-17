using MauiAppMinhasCompras.Models;
using SQLite;
namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        //comunicação da classe helper como banco de dados mantendo a persistência
        //de dados
        //O modificador readonly significa que o campo só pode ser atribuído no momento da
        //declaração ou dentro do construtor da classe.Isso ajuda a garantir que a conexão do
        //banco de dados não seja substituída acidentalmente
        readonly SQLiteAsyncConnection _conn;
        
        //Inicialização do banco de dados e criação da tabela Produtos assim que a instancia
        //Helpers é criada
        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
            //criação da tabela (caso ja exista o sql não faz nada, comando Wait serve para 
            //esperar a criação do banco de dados só dando continuidade quando ele estiver configurado
        }
        public Task<int> Insert(Produto p) //inserção de um registro do tipo produto no banco de dados
        {
            return _conn.InsertAsync(p);
        }
        public Task<List<Produto>> Update(Produto p) //atualizando a tabela e inserindo novos
            //valores para as colunas
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";
            return _conn.QueryAsync<Produto>(
            sql, p.Descricao, p.Quantidade, p.Preco, p.Id
            );
        }
        public Task<int> Delete(int id) //remoção dos registros na tabela com base na id fornecida 
            //como argumento, realiza a exclusão sem bloquear a thread principal
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);//expressão lambida (retornar todos os itens com a id que determinei)
            //Apenas registros onde o campo Id do produto (i.Id) corresponde ao valor fornecido no argumento (id) serão
            //excluídos.

        }
        public Task<List<Produto>> GetAll()//getall utilizado para efetuar as consultas na tabela
        {
            return _conn.Table<Produto>().ToListAsync();
            //Representa uma consulta que acessa a tabela Produto no banco de dados. É equivalente a "SELECT * FROM
            //Produto" em SQL.

        }
        public Task<List<Produto>> Search(string q)//metodo para efetuar a busca na tabela utilizando uma string
        //parametro é a letra Q 
        {
            //seleção da tabela produto e aplicação do filtro para buscar na coluna descrição
            //com registros que tenham a string Q, os caracteres "%" indicam que a letra pode entar antes 
            //ou depois de qualquer string, assim como no Work Bench
            string sql = "SELECT * FROM Produto WHERE descricao LIKE '%" + q + "%'";
            return _conn.QueryAsync<Produto>(sql);
        }
    }
}

//Task retorna as operações geralmente com 1 para sucedido e 0 para falha
// em task a maioria das operações funciona de forma assincrona, ou seja em paralelo com o codigo em funcionamento
//sqlite salva as informações como arquivos de texto, não tem tanta segurança, posso usar o sqlite 
//como api juntamente com o SGBD duvida
//leitura e escrita em um arquivo, diferente de uma conexão via host