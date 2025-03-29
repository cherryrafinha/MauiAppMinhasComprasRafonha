using MauiAppMinhasComprasRafonha.Models;

namespace MauiAppMinhasComprasRafonha.Views;

public partial class EditarProduto : ContentPage
{
    public EditarProduto()
    {
        InitializeComponent();
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e) //toda vez que eu crio um novo manipulador ele cria automatico
    {
        try
        {
            Produto produto_anexado = BindingContext as Produto;
            Produto p = new Produto
            {
                Id = produto_anexado.Id,
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text),
                Categoria = picker_categoria.SelectedItem?.ToString()
            };

            await App.Db.Update(p);
            await DisplayAlert("Sucesso!", "Registro Atualizado", "OK");
            await Navigation.PopAsync(); //volta a tela de origem

        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK"); //await seria sendo aguardado
        }
    }
}