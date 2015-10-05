using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Graph.Core.Database.Repositories
{
	public class Repository : IRepository 
	{
		public IEnumerable<TEntity> Select<TEntity>(string where, object param = null)
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
				var id = connection.Query<dynamic>(entity.SaveSql, entity).Single().NewId;
				if (entity.Id != 0) {
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