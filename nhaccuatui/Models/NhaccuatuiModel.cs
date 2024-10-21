using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Collections; // Sử dụng Lớp ArrayList để lưu kết quả
using System.Data.SqlClient;// Sử dụng các lớp tương tác CSDL


namespace nhaccuatui.Models
{
    public class NhaccuatuiModel
    {
        private String connectionString = "workstation id=nhaccuatui.mssql.somee.com;packet size=4096;user id=tnpt003_SQLLogin_1;pwd=aj3vd8m6jx;data source=nhaccuatui.mssql.somee.com;persist security info=False;initial catalog=nhaccuatui;TrustServerCertificate=True";
        public ArrayList get(string sql)
        {
            ArrayList datalist = new ArrayList();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();

                using (SqlDataReader r = command.ExecuteReader())
                {
                    while (r.Read())
                    {
                        // Create an ArrayList to store the row data
                        ArrayList row = new ArrayList();
                        for (int i = 0; i < r.FieldCount; i++)
                        {
                            row.Add(r.GetValue(i).ToString());
                        }
                        datalist.Add(row); // Add the row to the datalist
                    }
                }
            }
            return datalist; // Return the filled datalist
        }

    }

}