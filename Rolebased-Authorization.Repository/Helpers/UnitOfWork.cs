using Rolebased_Authorization.Repository.Helpers.Interfaces;
using Rolebased_Authorization.Repository.Persistance;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rolebased_Authorization.Repository.Helpers
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext Context { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            Context = context;
        }
        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();

        }
    }
}
