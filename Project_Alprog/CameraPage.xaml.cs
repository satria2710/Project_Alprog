using Microsoft.Maui.Controls;
using System;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Camera.MAUI;
using Microsoft.Maui.ApplicationModel;
using OpenCvSharp;

namespace Project_Alprog
{
    public partial class CameraPage : ContentPage
    {
        private IDispatcherTimer? _frameTimer; // Tambah ? agar tidak error null
        private CascadeClassifier? _faceClassifier; // Tambah ? agar tidak error null
        private bool _isDetecting = false;

        public CameraPage()
        {
            InitializeComponent();
            CameraFeed.CamerasLoaded += CameraFeed_CamerasLoaded;
        }

        private async Task LoadCascadeModel()
        {
            string cascadeName = "haarcascade_frontalface_default.xml";
            string localPath = Path.Combine(FileSystem.CacheDirectory, cascadeName);

            if (!File.Exists(localPath))
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(cascadeName);
                using var newStream = File.Create(localPath);
                await stream.CopyToAsync(newStream);
            }

            _faceClassifier = new CascadeClassifier(localPath);
        }

        // Tambah tanda ? pada object sender
        private void CameraFeed_CamerasLoaded(object? sender, EventArgs e)
        {
            // PERBAIKAN 1: Gunakan Cameras.Count, bukan NumCameras
            if (CameraFeed.Cameras != null && CameraFeed.Cameras.Count > 0)
            {
                CameraFeed.Camera = CameraFeed.Cameras.First();
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    var result = await CameraFeed.StartCameraAsync();
                    if (result != CameraResult.Success)
                    {
                        await DisplayAlert("Error", "Gagal menyalakan kamera.", "OK");
                    }
                });
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
            }
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Akses Ditolak", "Aplikasi butuh kamera.", "OK");
                return;
            }

            await LoadCascadeModel();

            _frameTimer = Dispatcher.CreateTimer();
            _frameTimer.Interval = TimeSpan.FromMilliseconds(1000);
            _frameTimer.Tick += FrameTimer_Tick;
            _frameTimer.Start();
        }

        // Tambah tanda ? pada object sender
        private async void FrameTimer_Tick(object? sender, EventArgs e)
        {
            if (_isDetecting || _faceClassifier == null) return;
            _isDetecting = true;

            try
            {
                // PERBAIKAN 2: Hapus 'await' karena versi Camera.MAUI ini bersifat sinkron
                var snap = CameraFeed.GetSnapShot(Camera.MAUI.ImageFormat.PNG);

                if (snap is StreamImageSource streamImageSource)
                {
                    using var stream = await streamImageSource.Stream(CancellationToken.None);
                    if (stream != null)
                    {
                        using var ms = new MemoryStream();
                        await stream.CopyToAsync(ms);
                        byte[] imageBytes = ms.ToArray();

                        using Mat frame = Cv2.ImDecode(imageBytes, ImreadModes.Grayscale);

                        // PERBAIKAN 3: Gunakan OpenCvSharp.Rect secara eksplisit agar tidak ambigu
                        OpenCvSharp.Rect[] faces = _faceClassifier.DetectMultiScale(
                            frame,
                            scaleFactor: 1.1,
                            minNeighbors: 4,
                            minSize: new OpenCvSharp.Size(50, 50));

                        if (faces.Length > 0)
                        {
                            _frameTimer?.Stop();
                            await CameraFeed.StopCameraAsync();

                            await MainThread.InvokeOnMainThreadAsync(async () =>
                            {
                                await DisplayAlert("Verifikasi Berhasil", "Wajah Anda telah terdeteksi sistem.", "OK");
                                await Navigation.PushAsync(new DashboardPage());
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error OpenCV: " + ex.Message);
            }
            finally
            {
                _isDetecting = false;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _frameTimer?.Stop();
            CameraFeed.StopCameraAsync();
        }
    }
}