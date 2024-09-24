﻿using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Repositories;

public class SeverityRepository(ApplicationDbContext dbContext) : ISeverityRepository
{
    public async Task<IEnumerable<Severity>> GetSeveritiesAsync() => await dbContext.Severity.AsNoTracking().ToArrayAsync();
}
