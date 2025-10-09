// DatabaseService.cs
using MySql.Data.MySqlClient;
using System;
using System.Windows; // 用於 MessageBox

namespace Deskpet
{
    public class DatabaseService
    {
        // ====== 1️⃣ 設定 MySQL 連線字串 ======
        private readonly string connectionString = "Server=localhost;Database=PetProject;Uid=root;Pwd=;";
        // 如果你有設定 root 密碼，改成 Pwd=你的密碼;

        // ====== 2️⃣ 初始化資料庫（檢查/建立表格）======
        public void InitializeDatabase()
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS PetStatus (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        Mood INT NOT NULL,
                        Hunger INT NOT NULL,
                        LastUpdate DATETIME NOT NULL
                    );
                    CREATE TABLE IF NOT EXISTS UserData (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        Username VARCHAR(50) NOT NULL,
                        Reminder VARCHAR(255),
                        TodoList TEXT
                    );";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"資料庫初始化失敗: {ex.Message}");
            }
        }

        // ====== 3️⃣ 讀取最新的桌寵狀態 ======
        public PetStatus LoadPetStatus()
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT Mood, Hunger, LastUpdate FROM PetStatus ORDER BY Id DESC LIMIT 1;";
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new PetStatus
                                {
                                    Mood = reader.GetInt32("Mood"),
                                    Hunger = reader.GetInt32("Hunger"),
                                    LastUpdate = reader.GetDateTime("LastUpdate")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"讀取資料失敗: {ex.Message}");
            }
            // 如果沒有任何資料，回傳一個預設的狀態
            return new PetStatus { Mood = 100, Hunger = 100, LastUpdate = DateTime.Now };
        }

        // ====== 4️⃣ 儲存桌寵狀態 ======
        public void SavePetStatus(PetStatus status)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO PetStatus (Mood, Hunger, LastUpdate) VALUES (@mood, @hunger, @time);";
                        cmd.Parameters.AddWithValue("@mood", status.Mood);
                        cmd.Parameters.AddWithValue("@hunger", status.Hunger);
                        cmd.Parameters.AddWithValue("@time", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"儲存資料失敗: {ex.Message}");
            }
        }
    }
}