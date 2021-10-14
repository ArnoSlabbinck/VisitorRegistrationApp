﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;

namespace VisitorRegistrationApp.Data.Repository
{
    public class EmployeeRepository : BaseRepository<Employee, ApplicationDbContext>, IEmployeeRespository
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly ILogger<Employee> logger;
        public EmployeeRepository(ApplicationDbContext applicationDbContext, ILogger<Employee> logger) : base(applicationDbContext, logger)
        {
            this.applicationDbContext = applicationDbContext;
            this.logger = logger;
        }

        public async Task<Employee> GetEmployeeByName(string name)
        {
            return applicationDbContext.Employees.Where(e => e.Name == name).FirstOrDefault();
        }

        public IQueryable<Employee> GetEmployeesWithCompanies()
        {
            return applicationDbContext.Employees.Include(c => c.Company);
        }

        public async Task<Employee> GetEmployeeWithCompanyAndImage(int CompanyId)
        {
            return await applicationDbContext.Employees.Include(c => c.Company)
                .ThenInclude(i => i.Picture)
                .Where(x => x.Company.Id == CompanyId).FirstAsync();
        }
    }

    public interface IEmployeeRespository : IRepository<Employee>
    {
        IQueryable<Employee> GetEmployeesWithCompanies();

        Task<Employee> GetEmployeeByName(string name);

        Task<Employee> GetEmployeeWithCompanyAndImage(int CompanyId);
    }
}
