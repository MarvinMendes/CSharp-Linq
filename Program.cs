using LinqToEntities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToEntities
{
    class Program
    {
        static void Main(string[] args)
        {
             using (var context = new AluraTunesEntities1()) {
                var query = from g in context.Generos select g;

               // foreach (var genero in query)
                //{
                  //  Console.WriteLine("\t "+ genero.Nome + "\t" + genero.GeneroId);
                //}

                var queryJoin = from g in context.Generos join f in context.Faixas on g.GeneroId equals f.GeneroId select new { g, f };

               queryJoin = queryJoin.Take(20);

              //  foreach (var item in queryJoin)
               // {
                    //Console.WriteLine("\t Gênero ID {0}, Nome do Gênero: {1} \t", item.g.GeneroId, item.g.Nome);
               //     Console.WriteLine("\t Gênero ID: {0}, Nome do Gênero: {1} Faixa ID: {2}, Nome da Faixa: {3}", item.g.GeneroId, item.g.Nome, item.f.FaixaId, item.f.Nome); ;
              //  }



                var queries = from a in context.Artistas 
                              join ab in context.Albums on a.ArtistaId equals ab.ArtistaId 
                              join f in context.Faixas on ab.AlbumId equals f.AlbumId 
                              select new { a, ab, f};

                queries = queries.Take(10);

                foreach (var item in queries)
                {                    
                    //Console.WriteLine("\t {0}, {1}", item.a.Nome, item.a.Albums);
                    
                    Console.WriteLine("\t Artista: {0}, Álbum: {1}, Músicas: {2}", item.a.Nome, item.ab.Titulo, item.f.Nome);

                }


                Console.WriteLine("\n Busca com métodos");
                var queryOrder = from a in context.Albums join f in context.Faixas on a.AlbumId equals f.AlbumId select new { album = a.Titulo, faixas = f.Nome };

                queryOrder = queryOrder.Where(a => a.album.Contains("How To Dismantle An Atomic Bomb"));

                queryOrder = queryOrder.OrderBy(f => f.faixas);

                foreach (var item in queryOrder)
                {
                    Console.WriteLine("Álbum: " + item.album + " Faixas: " + item.faixas);
                }


            }



            Console.ReadKey();
        }
    }
}
