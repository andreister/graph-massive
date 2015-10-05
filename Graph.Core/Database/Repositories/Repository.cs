using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Graph.Core.Database.Repositories
{
	public class Repository : IRepository 
	{
		public IEnumerable<TEntity> Select<TEntity>(string where = null, object param = null)
			where TEntity : Entity
		{
			var tableName = GetTableName<TEntity>();
			using (var connection = OpenConnection()) {
				var filter = string.IsNullOrWhiteSpace(where) ? "" : " WHERE " + where;
				return connection.Query<TEntity>("SELECT * FROM " + tableName + filter, param);
			}
		}

		public void Save(Entity entity)
		{
			using (var connection = OpenConnection()) {
				var id = (int)connection.Query<dynamic>(entity.SaveSql, entity).Select(x => (long)x.NewId).Single();
				if (entity.Id == 0) {
					entity.Id = id;
				}
			}
		}

		protected void Execute(string adHocQuery, object param = null)
		{
			using (var connection = OpenConnection()) {
				connection.Execute(adHocQuery, param);
			}
		}

		protected IEnumerable<TResult> SelectAdHoc<TResult>(string adHocQuery, object param = null)
		{
			using (var connection = OpenConnection()) {
				return connection.Query<TResult>(adHocQuery, param);
			}
		}

		private string GetTableName<TEntity>()
		{
			var attribute = typeof(TEntity).GetCustomAttributes(typeof(TableNameAttribute), false).Cast<TableNameAttribute>().Single();
			return attribute.TableName;
		}

		private SqlConnection OpenConnection()
		{
			var hardcodedConnectionString = "Data Source=.;Initial Catalog=Graph;Integrated Security=SSPI";
			return new SqlConnection(hardcodedConnectionString);
		}
	}
}