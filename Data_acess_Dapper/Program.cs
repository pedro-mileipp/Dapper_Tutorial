using System;
using BaltaDataAccess.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BaltaDataAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$; Trusted_Connection=False; TrustServerCertificate=True";

            using (var connection = new SqlConnection(connectionString))
            {   
                // ReadView(connection);
                // ExecuteScalar(connection);
                // ExecuteReadProcedure(connection);
                // ExecuteProcedure(connection);
                // CreateCategory(connection);
                // CreateManyCategory(connection);
                // UpdateCategory(connection);
                // DeleteCategory(connection);
                // ListCategories(connection);
                // Console.WriteLine();
                // GetCategory(connection);
                // OneToOne(connection);

            }
        }

        static void ListCategories(SqlConnection connection) // Imprimirá (listará) todas as categorias
        {
            var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]"); // Query SQL para trazer o Id e Title da tabela Category
            foreach (var item in categories) // cada registro (linha) da tabela será um elemento, para percorrer tudo é preciso iterar na lista.
            {
                Console.WriteLine($"{item.Id} - {item.Title}"); // irá imprimir o Id e o Title 
            }
        }

        static void GetCategory(SqlConnection connection) // Irá imprimir a categoria com base no Id passado para a função
        {
            var category = connection
                .QueryFirstOrDefault<Category>( // QueryFirstOrDefault para executar em cima de um único registro
                    "SELECT TOP 1 [Id], [Title] FROM [Category] WHERE [Id]=@id", // Query SQL para pegar o primeiro item que o Id seja igual ao Id passado como parâmetro
                    new
                    {
                        id = "bd9eb6e2-7cad-4eae-8e4c-d75bdbfd1498"
                    });
            Console.WriteLine($"{category.Id} - {category.Title}");

        }

        static void CreateCategory(SqlConnection connection) // Criará uma nova categoria
        {
            var category = new Category(); // Cada tabela é uma classe e cada registro uma instância
            // atribuindo os atributos da classe
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var insertSql = @"INSERT INTO [Category] VALUES (@Id, @Title, @Url, @Summary, @Order, @Description, @Featured)"; // Query SQL para inserir na tabela Category
    
            var rows = connection.Execute(insertSql, new // a variavel rows retorna o numero de registros inseridos ou atualizados
            {
                category.Id,
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            });
            Console.WriteLine($"{rows} linhas inseridas");
        }

        static void UpdateCategory(SqlConnection connection) // Função para atualizar (alterar) alguma categoria já existente
        {
            var updateQuery = "UPDATE [Category] SET [Title]=@title WHERE [Id]=@Id"; // Query SQL para atualizar os dados ONDE o Id seja igual ao @id
            var rows = connection.Execute(updateQuery, new // a variavel rows retorna o numero de registros inseridos ou atualizados
            {
                Id = new Guid("af3407aa-11ae-4621-a2ef-2028b85507c4"), // ou id = "af3407aa-11ae-4621-a2ef-2028b85507c4" passado como string tambem
                title = "Frontend"
            });

            Console.WriteLine($"{rows} registros atualizadas");
        }

        static void DeleteCategory(SqlConnection connection)
        {
            var deleteQuery = "DELETE [Category] WHERE [Id]=@id"; // Query SQL para deletar um registro ONDE o Id for igual ao @id
            var rows = connection.Execute(deleteQuery, new
            {
                id = new Guid("34f49860-f173-4fad-8a8d-88d1dee2eb26"), // Id da categortia que se deseja exluir. Também funcionaria se o parametro fosse passado como string: id = "af3407aa-11ae-4621-a2ef-2028b85507c4" 
            });

            Console.WriteLine($"{rows} registros excluídos");
        }

        static void CreateManyCategory(SqlConnection connection) // Adicionando mais de uma categoria no b.d
        {
            var category = new Category(); // instanciando a primeira categoria (todo registro será uma instância)
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var category2 = new Category(); // instanciando a segunda categoria (todo registro será uma instância)
            category2.Id = Guid.NewGuid();
            category2.Title = "Categoria Nova";
            category2.Url = "categoria-nova";
            category2.Description = "Categoria nova";
            category2.Order = 9;
            category2.Summary = "Categoria";
            category2.Featured = true;

            var insertSql = @"INSERT INTO [Category] VALUES( @Id, @Title, @Url, @Summary, @Order, @Description, @Featured)"; // Query SQL para inserir elementos na tabela

            var rows = connection.Execute(insertSql, new[]{ // aqui o new será um array
                new
                {
                    category.Id,
                    category.Title,
                    category.Url,
                    category.Summary,
                    category.Order,
                    category.Description,
                    category.Featured
                },
                new
                {
                    category2.Id,
                    category2.Title,
                    category2.Url,
                    category2.Summary,
                    category2.Order,
                    category2.Description,
                    category2.Featured
                }
            });
            Console.WriteLine($"{rows} linhas inseridas");
        }
    
        static void ExecuteProcedure(SqlConnection connection){
            var procedure = "[spDeleteStudent]"; // passando o nome da procedure, não precisa ser o comando SQL (nesse caso EXEC [spDeleteStudent] @StudentId), passa-se APENAS a procedure
            var pars = new { StudentId = "b7e12791-86f6-4dad-86ae-18709fd8bbb0"};
            
            // será retornado a quantidade de linhas afetadas em rows
            var rows = connection.Execute(procedure, // parâmetro 1, a procedure que desejamos que seja executada
            pars, // parâmetro 2, o parâmetro que a procedure recebe, nesse caso o Id do Student
            commandType: System.Data.CommandType.StoredProcedure); // parâmetro 3, o tipo de comando SQL que será realizado

            Console.WriteLine($"{rows} linhas afetadas");
        }
        
        static void ExecuteReadProcedure(SqlConnection connection)
        {
            var procedure = "[spGetCoursesByCategory]"; // passando o nome da procedure, não precisa ser o comando SQL (EXEC [spGetCoursesByCategory] '09ce0b7b-cfca-497b-92c0-3290ad9d5142'), passa-se APENAS a procedure
            var pars = new { CategoryId = "09ce0b7b-cfca-497b-92c0-3290ad9d5142" };
            var courses = connection.Query(
                procedure,
                pars,
                commandType: System.Data.CommandType.StoredProcedure);

            int i = 1;
            foreach (var item in courses) // cada curso da categoria será um item da lista
            {
                Console.WriteLine("{0}: {1}", i, item.Title);
                i++;
            }
            
        }
        
        static void ExecuteScalar(SqlConnection connection)
        {
            var category = new Category();
            category.Title = "Vue.Js";
            category.Url = "vuejs";
            category.Description = "Categoria destinada a serviços do Vue.Js";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var insertSql = @"
                INSERT INTO 
                    [Category] 
                OUTPUT inserted.[Id]
                VALUES(
                    NEWID(), 
                    @Title, 
                    @Url, 
                    @Summary, 
                    @Order, 
                    @Description, 
                    @Featured) 
                ";

            var id = connection.ExecuteScalar<Guid>(insertSql, new
            {
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            });
            Console.WriteLine($"A categoria inserida foi: {id}");
        }
    
        static void ReadView(SqlConnection connection)
        {
            var sql = "SELECT * FROM [vwCourses]";
            var courses = connection.Query(sql);

            int i = 1;
            foreach (var item in courses)
            {
                Console.WriteLine($"{i}: {item.Id} - {item.Title}");
                i++;
            }
        }

        static void OneToOne(SqlConnection connection)
        {
            var sql = @"
                SELECT 
                    * 
                FROM 
                    [CareerItem] 
                INNER JOIN 
                    [Course] ON [CareerItem].[CourseId] = [Course].[Id]";

            var items = connection.Query<CareerItem, Course, CareerItem>(
                sql,
                (careerItem, course) =>
                {
                    careerItem.Course = course;
                    return careerItem;
                }, splitOn: "Id");

            foreach (var item in items)
            {
                Console.WriteLine($"{item.Title} - Curso: {item.Course.Title}");
            }
        }

        static void OneToMany(SqlConnection connection){
        var sql = @"
                SELECT 
                    [Career].[Id],
                    [Career].[Title],
                    [CareerItem].[CareerId],
                    [CareerItem].[Title]
                FROM 
                    [Career] 
                INNER JOIN 
                    [CareerItem] ON [CareerItem].[CareerId] = [Career].[Id]
                ORDER BY
                    [Career].[Title]";

            var careers = new List<Career>();
            var items = connection.Query<Career, CareerItem, Career>(
                sql,
                (career, item) =>
                {
                    var car = careers.Where(x => x.Id == career.Id).FirstOrDefault();
                    if (car == null)
                    {
                        car = career;
                        car.Items.Add(item);
                        careers.Add(car);
                    }
                    else
                    {
                        car.Items.Add(item);
                    }

                    return career;
                }, splitOn: "CareerId");

            foreach (var career in careers)
            {
                Console.WriteLine($"{career.Title}");
                foreach (var item in career.Items)
                {
                    Console.WriteLine($" - {item.Title}");
                }
            }
       } 

        static void QueryMutiple(SqlConnection connection){
            var query = "SELECT * FROM [Category]; SELECT * FROM [Course]";

            using (var multi = connection.QueryMultiple(query))
            {
                var categories = multi.Read<Category>();
                var courses = multi.Read<Course>();

                foreach (var item in categories)
                {
                    Console.WriteLine(item.Title);
                }

                foreach (var item in courses)
                {
                    Console.WriteLine(item.Title);
                }
            }
        }

        static void SelectIn(SqlConnection connection)
        {
            var query = @"select * from Career where [Id] IN @Id";

            var items = connection.Query<Career>(query, new
            {
                Id = new[]{
                    "4327ac7e-963b-4893-9f31-9a3b28a4e72b",
                    "e6730d1c-6870-4df3-ae68-438624e04c72"
                }
            });

            foreach (var item in items)
            {
                Console.WriteLine(item.Title);
            }

        }

        static void Like(SqlConnection connection, string term)
        {
            var query = @"SELECT * FROM [Course] WHERE [Title] LIKE @exp";

            var items = connection.Query<Course>(query, new
            {
                exp = $"%{term}%"
            });

            foreach (var item in items)
            {
                Console.WriteLine(item.Title);
            }
        }

        static void Transaction(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Minha categoria que não";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var insertSql = @"INSERT INTO 
                    [Category] 
                VALUES(
                    @Id, 
                    @Title, 
                    @Url, 
                    @Summary, 
                    @Order, 
                    @Description, 
                    @Featured)";

            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                var rows = connection.Execute(insertSql, new
                {
                    category.Id,
                    category.Title,
                    category.Url,
                    category.Summary,
                    category.Order,
                    category.Description,
                    category.Featured
                }, transaction);

                transaction.Commit();
                // transaction.Rollback();

                Console.WriteLine($"{rows} linhas inseridas");
            }
        }
    }   

}