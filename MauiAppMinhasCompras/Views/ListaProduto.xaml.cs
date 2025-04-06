using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();

        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        try
        {
            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());

        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {


            string q = e.NewTextValue;

            lst_produtos.IsRefreshing = true;

            lista.Clear();

            List<Produto> tmp = await App.Db.Search(q);

            tmp.ForEach(i => lista.Add(i));

        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }

        finally
        {
            lst_produtos.IsRefreshing = false;
        }

    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);

        string msg = $"O total é {soma:C}";

        DisplayAlert("Total dos Produtos", msg, "OK");
    }

    private async void MenuItem_Clicked3(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecionado = sender as MenuItem;

            Produto p = selecionado.BindingContext as Produto;

            bool confirm = await DisplayAlert(
                "Tem Certeza?", $"Remover {p.Descricao}?", "Sim", "Não");
            if (confirm)
            {
                await App.Db.Delete(p.ID);
                lista.Remove(p);

            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto
            {

                BindingContext = p,
            });

        }


        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }

    }

    private void SomarPorCategoria_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (pickerCategoria.SelectedItem == null)
            {
                DisplayAlert("Erro", "Por favor, selecione uma categoria primeiro.", "OK");
                return;
            }

            string categoriaSelecionada = pickerCategoria.SelectedItem.ToString();

            var produtosFiltrados = lista.Where(i => i.Categoria == categoriaSelecionada).ToList();

            double soma = produtosFiltrados.Sum(i => i.Total);

            string msg = produtosFiltrados.Count > 0
                ? $"O total da categoria {categoriaSelecionada} é {soma:C}"
                : "Nenhum produto encontrado nesta categoria.";

            DisplayAlert("Total por Categoria", msg, "OK");
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {
            lista.Clear();
            List<Produto> tmp = await App.Db.GetAll();
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private async void PickerCategoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string categoriaSelecionada = pickerCategoria.SelectedItem.ToString();

            lista.Clear();

            List<Produto> produtosFiltrados = await App.Db.GetProdutosPorCategoria(categoriaSelecionada);
            produtosFiltrados.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
    }


}