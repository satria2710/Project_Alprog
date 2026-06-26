using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;

namespace Project_Alprog
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LoginBtn_Clicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Peringatan", "Username dan Password tidak boleh kosong!", "OK");
                return;
            }

            if (username.ToLower() == "admin" && password == "admin123")
            {
                Preferences.Set("UserRole", "Admin");
                await Navigation.PushAsync(new CameraPage());
            }
            else if (username.ToLower() == "user" && password == "user123")
            {
                Preferences.Set("UserRole", "User");
                await Navigation.PushAsync(new CameraPage());
            }
            else
            {
                await DisplayAlert("Gagal", "Username atau Password salah!", "Coba Lagi");
            }
        }
    }
}