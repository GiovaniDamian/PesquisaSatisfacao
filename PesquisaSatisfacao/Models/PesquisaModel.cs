using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PesquisaSatisfacao.Models
{
    public class PesquisaModel
    {
        public string Texto { get; set; }
        public int Nota { get; set; }
        public string Usuario { get; set; }
        public DateTime DataRealizada { get; set; }


        public static bool Incluir(string texto, int nota, string usuario, DateTime data)
        {
            var repositorio = Repositorio.NovaInstancia();

            var sql = @"UPDATE pesquisa_satisfacao 
                        set texto = :texto,
                            nota = :nota,
                            data = :data
                        where usuario = :usuario";

            var parametros = repositorio.NovosParametros();
            parametros.Add(":texto", texto);
            parametros.Add(":nota", nota);
            parametros.Add(":usuario", usuario);
            parametros.Add(":data", data);

            var resultado = repositorio.ExecutarSql(sql, parametros);

            return resultado;
        }

        public static bool EncontrarPesquisaDuplicada(string usuario)
        {
            var repositorio = Repositorio.NovaInstancia();
            var parametros = repositorio.NovosParametros();

            var sql = @"SELECT data from pesquisa_satisfacao where usuario = :usuario";

            parametros.Add(":usuario", usuario);

            var resultado = repositorio.ExecutarSqlComRetorno(sql, parametros);

            if (resultado.Tables[0].Rows.Count > 0)
            {
                if (resultado.Tables[0].Rows[0].ItemArray[0].ToString() != "")
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IncluirDataModal(string usuario, DateTime data)
        {
            var repositorio = Repositorio.NovaInstancia();

            var sql = @"INSERT INTO pesquisa_satisfacao ( usuario, datamodal)
                        VALUES (:usuario, :data)";

            var parametros = repositorio.NovosParametros();

            parametros.Add(":usuario", usuario);
            parametros.Add(":data", data);

            var resultado = repositorio.ExecutarSql(sql, parametros);

            return resultado;
        }

        public static bool AtualizarDataModal(string usuario, DateTime data)
        {
            var repositorio = Repositorio.NovaInstancia();

            var sql = @"UPDATE pesquisa_satisfacao set datamodal = :data where usuario = :usuario";

            var parametros = repositorio.NovosParametros();

            parametros.Add(":usuario", usuario);
            parametros.Add(":data", data);

            var resultado = repositorio.ExecutarSql(sql, parametros);

            return resultado;
        }

        public static string PesquisarDataModal(string usuario)
        {
            var repositorio = Repositorio.NovaInstancia();
            var parametros = repositorio.NovosParametros();

            var sql = @"SELECT datamodal from pesquisa_satisfacao where usuario = :usuario";

            parametros.Add(":usuario", usuario);
            var resultado = repositorio.ExecutarSqlComRetorno(sql, parametros);


            if (resultado.Tables[0].Rows.Count > 0)
            {
                return resultado.Tables[0].Rows[0].ItemArray[0].ToString();
            }
            return "False";
        }
    }
}