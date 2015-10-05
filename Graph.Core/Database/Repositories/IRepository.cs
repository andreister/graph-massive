﻿using System.Collections.Generic;

namespace Graph.Core.Database.Repositories
{
	public interface IRepository
	{
		IEnumerable<TEntity> Select<TEntity>(string where, object param = null)
			where TEntity : Entity;
		
		void Save(Entity entity);
	}
}
