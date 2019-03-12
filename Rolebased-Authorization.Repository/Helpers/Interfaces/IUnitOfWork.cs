using Rolebased_Authorization.Repository.Persistance;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rolebased_Authorization.Repository.Helpers.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationDbContext Context { get; }
        void Commit();
    }
}
