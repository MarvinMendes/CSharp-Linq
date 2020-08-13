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


                //contando todas as músicas pertencentes a um determinado artista


                var queryCount = from f in context.Faixas where f.Album.Artista.Nome.Contains("U2") select f;


                var quantidade = queryCount.Count();

                Console.WriteLine("\n Existem {0} músicas do U2 no banco de dados.\n", quantidade);

                queryCount = queryCount.OrderBy(f => f.Nome);

                foreach (var item in queryCount)
                {
                    Console.WriteLine("Músicas do U2 {0}", item.Nome);
                }

                //clientes
                Console.WriteLine("\n");

                var clientes = from c in context.Clientes select c;

                clientes = clientes.Where(c => c.Pais.Contains("Brazil"));

                foreach (var item in clientes)
                {
                    Console.WriteLine("Clientes {0}, - {1}, - {2}", item.PrimeiroNome, item.Sobrenome, item.Pais);                                       
                }


                //manipulando notas fiscais
                var notasFiscais = from c in context.Clientes join nf in context.NotaFiscals on c.ClienteId
                                   equals nf.ClienteId 
                                   select new { client = c.PrimeiroNome, totalPedido = nf.Total };


                notasFiscais = notasFiscais.Where(nf => nf.totalPedido > 10);

                foreach (var item in notasFiscais)
                {
                    Console.WriteLine("Nome Cliente: {0} - Total gasto: {1}", item.client, item.totalPedido);

                }

                //buscando o total gasto com um determinado artista

                Console.WriteLine("\n");
                var faixasEmItemNota = from inf in context.ItemNotaFiscals
                                 where inf.Faixa.Album.Artista.Nome.Contains("U2")
                                 select inf;
                                
                //{ totalArtista = inf.PrecoUnitario * inf.Quantidade }
                foreach (var item in faixasEmItemNota)
                {
                    Console.WriteLine("Nome da música comprada: " + item.Faixa.Nome + " valor da música: "   + item.Faixa.PrecoUnitario);
                }

                //navegando pelas propriedades

                var totalGasto = from inf in context.ItemNotaFiscals
                                 where inf.Faixa.Album.Artista.Nome.Contains("Marvin")
                                 select new { totalDasFaixas = inf.PrecoUnitario * inf.Quantidade };

                // Console.WriteLine("\n Total gasto");
                // foreach (var item in totalGasto)
                //{
                //  Console.WriteLine("{0} ", item.totalDasFaixas);
                //}

                var artista = from f in context.Faixas where f.Album.Artista.Nome.Contains("Marvin") select f;

                foreach (var item in artista)
                {
                    Console.WriteLine("Músicas do artista: Marvin Gaye  {0}", item.Nome);
                }

                Console.WriteLine("\nTotal gasto com as músicas do artista: {0}\n", totalGasto.Sum(f => f.totalDasFaixas));

                //buscando album mais vendidos de um artista

                var albunsVendidos = from it in context.ItemNotaFiscals
                                      where it.Faixa.Album.Artista.Nome.Contains("U2")
                                      group it by it.Faixa.Album into agrupadoIt
                                      select new { tituloAlbum = agrupadoIt.Key.Titulo,
                                        totalAlbum = agrupadoIt.Sum(t => t.Quantidade * t.PrecoUnitario)};


                albunsVendidos = albunsVendidos.OrderByDescending(p => p.totalAlbum);

                foreach (var item in albunsVendidos)
                {
                    Console.WriteLine("\nTítulo do álbum {0}, Gasto com o Álbum {1}", item.tituloAlbum, item.totalAlbum);                        
                }
                // Console.WriteLine("Total de músicas: {0}", total);
                //Console.WriteLine("Total da soma: {0}", quantidadeVendidas);


                Console.WriteLine("\n");


                var musicasVendidas = from it in context.ItemNotaFiscals
                                      where it.Faixa.Album.Artista.Nome.Contains("U2")
                                      group it by it.Faixa into agupadoFaixa
                                      select new { nome = agupadoFaixa.Key.Nome, unitario = agupadoFaixa.Key.PrecoUnitario, 
                                          totalMusicas = agupadoFaixa.Sum(p =>  p.Quantidade) };

                musicasVendidas = musicasVendidas.OrderByDescending(q => q.totalMusicas);

                foreach (var item in musicasVendidas)
                {
                    Console.WriteLine("\nMusica vendida {0} - Preço unitário: {1} - Quantidade de vezes que foi comprada: {2}" , item.nome, item.unitario, item.totalMusicas);
                }
                Console.WriteLine("\n");

                //trazendo as musicas vendidas de um artista, a quantidade de venda de cada uma e o somatório final de todas as músicas vendidas deste artista


                var MusicasVendidas = from it in context.ItemNotaFiscals
                                           where it.Faixa.Album.Artista.Nome.Contains("U2")
                                           group it by it.Faixa into agrupadoFaixas
                                           select new { nomeArtista = agrupadoFaixas.Key.Album.Artista.Nome,
                                               nomeMusica = agrupadoFaixas.Key.Nome,
                                               quantidade = agrupadoFaixas.Sum(quant => quant.Quantidade),
                                               precoUnit = agrupadoFaixas.Key.PrecoUnitario };


                var totalDeMusicasVendidas = MusicasVendidas.Sum(quant => quant.quantidade);
                var valorTotalEmMusicas = totalDeMusicasVendidas * 0.99;

                Console.WriteLine("Pegando total gasto em músicas do artista - Total de músicas vendidas: {0} - Total em dinheiro das músicas vendidas: R$ {1}", totalDeMusicasVendidas, valorTotalEmMusicas);
                foreach (var item in MusicasVendidas)
                {                    
                    Console.WriteLine("Nome do artista: {0} - Nome da música: {1} - Total de vendas: {2}", item.nomeArtista, item.nomeMusica, item.quantidade );
                }

            }





            Console.ReadKey();
        }
    }
}
