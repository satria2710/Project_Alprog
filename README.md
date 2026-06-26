# 🌍 EEWS Command Center - Access Control System

> Prototipe sistem keamanan siber dan *dashboard* kontrol untuk stasiun pemantauan Jaringan Peringatan Dini Gempa Bumi (*Earthquake Early Warning System* - EEWS).

## 📋 Deskripsi Proyek
Sistem kendali infrastruktur kritis memerlukan validasi akses yang cepat, tangguh, dan anti-lag. Proyek ini adalah aplikasi lintas platform (*cross-platform*) yang mendemonstrasikan implementasi keamanan biometrik wajah tanpa bergantung pada komputasi *cloud* (*On-Device Edge Computing*). Tujuannya adalah untuk mencegah tingginya latensi dan menekan risiko *false alarm* pada saat bencana alam gempa bumi yang berpotensi memutus jaringan internet luar.

## ✨ Fitur Utama Inovasi
* **On-Device Facial Verification (OpenCV):** Menggunakan algoritma *Haar Cascade* yang berjalan 100% secara lokal untuk mendeteksi wajah operator secara *real-time* tanpa membebani pembacaan sensor seismik.
* **Role-Based Adaptive Dashboard (.NET MAUI):** Antarmuka GUI cerdas yang langsung berubah sesuai wewenang dalam waktu < 3 detik. *Field Operator* akan melihat menu instruksi aksi darurat, sedangkan *Admin* mendapatkan kendali penuh.
* **Zero Database Architecture:** Mengandalkan integrasi `Microsoft.Maui.Storage.Preferences` untuk manajemen sesi yang super ringan tanpa perlu instalasi *database SQL* yang berat.

## 🛠️ Stack Teknologi
| Komponen | Teknologi yang Digunakan |
| :--- | :--- |
| **Framework UI** | .NET MAUI (C# / XAML) |
| **Computer Vision** | OpenCV (Haar Cascade Classifier) |
| **Local Storage** | MAUI Preferences |
| **Target OS** | Windows, Android (Cross-platform) |

## 🚀 Panduan Instalasi & Menjalankan
1. *Clone repository* ini ke mesin lokal Anda:
   ```bash
   git clone [https://github.com/satria2710/Project_Alprog.git](https://github.com/satria2710/Project_Alprog.git)
