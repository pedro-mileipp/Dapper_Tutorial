using System;
using Microsoft.Data.SqlClient;
using Dapper;
using Teste_Dapper.Models;
namespace Teste_Dapper{
    class Program{
        static void Main(){
            Console.WriteLine();
            const string connectionString = "Server=localhost,1433;Database=TestDapper;User ID=sa;Password=1q2w3e4r@#$; Trusted_Connection=False; TrustServerCertificate=True";

            using (var connection = new SqlConnection(connectionString)){
                
                // DeleteProduct(connection);
                // CreateProduct(connection);
                // UpdateProductQtd(connection);
                ListProducts(connection);
                Console.WriteLine();
                // GetProduct(connection);
            };
        }

        static void GetProduct(SqlConnection connection){
            var product = connection.QueryFirstOrDefault("SELECT TOP 1 [Id], [Nome], [Fabricante], [Quantidade], [VlUnitario], [Tipo] FROM [Products] WHERE Id = @id", new {
                id = new Guid("b2498e9b-16aa-488e-8e05-caa578924e12")
            });

            Console.WriteLine($"{product.Id} - {product.Nome} - {product.Fabricante} - {product.Quantidade} - {product.VlUnitario} - {product.Tipo}");

        }

        static void ListProducts(SqlConnection connection){
            var records = connection.Query<Products>("SELECT [Id], [Nome], [Fabricante], [Quantidade], [VlUnitario], [Tipo] FROM [Products]");
            foreach (var item in records)
            {
                System.Console.WriteLine();
                Console.WriteLine($"{item.Id} - {item.Nome} - {item.Fabricante} - {item.Quantidade} - {item.VlUnitario} - {item.Tipo}");
            }
        }

        static void CreateProduct(SqlConnection connection){
            var prod = new Products();
            prod.Id = Guid.NewGuid();
            prod.Nome = "G15 Gamer";
            prod.Fabricante = "Dell";
            prod.Quantidade = 35;
            prod.VlUnitario = 4500;
            prod.Tipo = "Notebook";

            var insertSQL = "INSERT INTO [Products] VALUES(@Id, @Nome, @Fabricante, @Quantidade, @VlUnitario, @Tipo)";

            var rows = connection.Execute(insertSQL, new {
            prod.Id,
            prod.Nome,
            prod.Fabricante,    
            prod.Quantidade,
            prod.VlUnitario,
            prod.Tipo
            });

            Console.WriteLine($"{rows} linhas inseridas");  
        }

        static void UpdateProductQtd(SqlConnection connection){
            var updateQuery = "UPDATE [Products] SET Quantidade = @Quantidade WHERE Id = @id";
            var rows = connection.Execute(updateQuery, new {
                id = new Guid("988b8479-bdf3-4d19-b570-34b7b6d182de"),
                quantidade = 50
            });
            Console.WriteLine($"{rows} registros atualizado");
        }

        static void DeleteProduct(SqlConnection connection){
            var deleteQuery = "DELETE FROM Products WHERE Id = @id";
            var rows = connection.Execute(deleteQuery, new{
                id = new Guid("8de3237a-8355-4d0e-9da7-063eb2030a99")
            });
            Console.WriteLine($"{rows} registros excluídos");
        }

    }
}