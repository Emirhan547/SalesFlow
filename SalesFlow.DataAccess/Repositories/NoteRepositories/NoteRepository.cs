using SalesFlow.DataAccess.Context;
using SalesFlow.DataAccess.Repositories.GenericRepositories;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.DataAccess.Repositories.NoteRepositories
{
    public class NoteRepository : GenericRepository<Note>, INoteRepository
    {
        public NoteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
