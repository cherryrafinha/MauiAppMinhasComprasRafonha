using MauiAppMinhasComprasRafonha.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasComprasRafonha.Views;

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
    private void ToolbarItem_Clicked(object sender, EventArgs e) //no item clicado oq acontece
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto()); //fazendo a navegação entre as telas, a lista de produtos com a pagina novo produto

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

            lista.Clear();

            List<Produto> tmp = await App.Db.Search(q);
            lst_produtos.IsRefreshing = true;

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK"); // ele quer ser await por ser assincrono, diferente do outro, ele espera kkk
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }



    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);

        var relatorio = lista
       .GroupBy(p => p.Categoria)
       .ToDictionary(g => g.Key, g => g.Sum(p => p.Preco * p.Quantidade));

        // Formatar a mensagem
        string msg = $"Total Geral: {soma:C}\n\nTotal por Categoria:\n";
        foreach (var categoria in relatorio)
        {
            msg += $"{categoria.Key}: {categoria.Value:C}\n";
        }

        DisplayAlert("Total dos Produtos e por Categoria", msg, "OK");
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecionado = sender as MenuItem; //sempre que clicar vai mostrar qual foi selecionado
            Produto p = selecionado.BindingContext as Produto; // vai ser o item em questao que selecionou
            bool confirm = await DisplayAlert(
                "Tem certeza?", $"Remover {p.Descricao}?", "Sim", "Não"); //cria um alerta para ter certeza de remover com confirma e nao confirma
            if (confirm)
            {
                await App.Db.Delete(p.Id); //tira ele do banco de dados
                lista.Remove(p); //remove da lista
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
            Produto p = e.SelectedItem as Produto; //é uma rede objeto
            Navigation.PushAsync(new Views.EditarProduto //vai para a pagina de editar produto
            {
                BindingContext = p,
            });
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



}