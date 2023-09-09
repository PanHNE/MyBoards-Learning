using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using MyBoards.Entities;

namespace MyBoards.Benchmark
{
    [MemoryDiagnoser]
    public class TrackingBenchmark
    {
        [Benchmark]
        public int WithTracking()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyBoardsContext>()
                .UseSqlServer("Server=DESKTOP-47QGLSK\\SQLEXPRESS;Database=MyBoardsDb;Trusted_Connection=True;TrustServerCertificate=true");

            var _dbContext = new MyBoardsContext(optionsBuilder.Options);

            var comments = _dbContext.Comments.ToList();

            return comments.Count();
        }

        [Benchmark]
        public int WithoutTracking()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyBoardsContext>()
                .UseSqlServer("Server=DESKTOP-47QGLSK\\SQLEXPRESS;Database=MyBoardsDb;Trusted_Connection=True;TrustServerCertificate=true");

            var _dbContext = new MyBoardsContext(optionsBuilder.Options);

            var comments = _dbContext.Comments.AsNoTracking().ToList();

            return comments.Count();
        }
    }
}
