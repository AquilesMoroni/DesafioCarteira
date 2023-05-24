using CarteiraDigital.Models;
using CarteiraDigital.ViewModel;
using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarteiraDigital.Repositorios
{
    public class MovimentosRepository
    {
        private ISession _session;

        public MovimentosRepository(ISession session) => _session = session;

        public async Task Add(MovimentoEntrada entrada)
        {
            NHibernate.ITransaction transaction = null;
            try
            {
                transaction = _session.BeginTransaction();
                await _session.SaveAsync(entrada);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await transaction?.RollbackAsync();
            }
            finally
            {
                transaction?.Dispose();
            }
        }

        public async Task Add(MovimentoSaida saida)
        {
            NHibernate.ITransaction transaction = null;
            try
            {
                transaction = _session.BeginTransaction();
                await _session.SaveAsync(saida);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await transaction?.RollbackAsync();
            }
            finally
            {
                transaction?.Dispose();
            }
        }

        public List<MovimentoViewModel> FindAll(int pessoaId)
        {
            string sql = @" SELECT 'Saída' AS Tipo, Descricao, Valor, DataSaida AS 'Data'
                            FROM MovimentoSaida
                            WHERE PessoaId = :idPessoa

                            UNION

                            SELECT 'Entrada' AS Tipo, Descricao, Valor, DataEntrada AS 'Data'
                            FROM MovimentoEntrada
                            WHERE PessoaId = :idPessoa

                            ORDER BY Data DESC";

            IQuery query = _session.CreateSQLQuery(sql)
                            .AddScalar("Tipo", NHibernateUtil.String)
                            .AddScalar("Descricao", NHibernateUtil.String)
                            .AddScalar("Valor", NHibernateUtil.Decimal)
                            .AddScalar("Data", NHibernateUtil.DateTime)
                            .SetParameter("idPessoa", pessoaId)
                            .SetResultTransformer(Transformers.AliasToBean<MovimentoViewModel>());

            List<MovimentoViewModel> movimentos = (List<MovimentoViewModel>)query.List<MovimentoViewModel>();
            return movimentos;
        }
    }
} 