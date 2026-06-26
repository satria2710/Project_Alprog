using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage; // Dibutuhkan untuk membaca Preferences

namespace Project_Alprog
{
    public partial class DashboardPage : ContentPage
    {
        public DashboardPage()
        {
            InitializeComponent();
        }

        // Fungsi ini akan otomatis berjalan sesaat sebelum halaman Dashboard terbuka di layar
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Membaca status role yang disimpan pada saat Login di Fase 3
            string userRole = Preferences.Get("UserRole", "Unknown");

            // Mengubah teks label agar sesuai dengan yang login
            UserRoleLabel.Text = $"Status Role: {userRole}";

            // Logika Penyesuaian Dinamis
            if (userRole == "Admin")
            {
                // Jika Admin, tampilkan menu Admin, sembunyikan menu User
                AdminSection.IsVisible = true;
                UserSection.IsVisible = false;
            }
            else if (userRole == "User")
            {
                // Jika User biasa, tampilkan menu User, sembunyikan menu Admin
                AdminSection.IsVisible = false;
                UserSection.IsVisible = true;
            }
            else
            {
                // Jaga-jaga jika terjadi error, sembunyikan keduanya
                AdminSection.IsVisible = false;
                UserSection.IsVisible = false;
            }
        }
    }
}