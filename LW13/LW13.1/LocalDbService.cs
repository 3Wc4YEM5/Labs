using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LW13._1
{
    public class LocalDbService
    {
        private const string DB_NAME = "DB.db";
        private readonly SQLiteAsyncConnection _connection;
        public LocalDbService() {
            //_connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection = new SQLiteAsyncConnection(Path.Combine("C:\\Users\\Georgiy\\source\\repos\\LW13.1\\LW13.1", DB_NAME));
            _connection.CreateTableAsync<Researcher>();
            _connection.CreateTableAsync<Project>();
            _connection.CreateTableAsync<Experiment>();
        }

        public async Task<List<Researcher>> GetResearchers()
        {
            return await _connection.Table<Researcher>().ToListAsync();
        }
        public async Task<Researcher> GetByIdRes(int id)
        {
            return await _connection.Table<Researcher>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateRes(Researcher researcher)
        {
            await _connection.InsertAsync(researcher);
        }
        
        public async Task UpdateRes(Researcher researcher)
        {
            await _connection.UpdateAsync(researcher);
        }

        public async Task DeleteRes(Researcher researcher)
        {
            await _connection.DeleteAsync(researcher);  
        }

        public async Task<List<Project>> GetProjects()
        {
            return await _connection.Table<Project>().ToListAsync();
        }
        public async Task<Project> GetByIdProj(int id)
        {
            return await _connection.Table<Project>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateProj(Project project)
        {
            await _connection.InsertAsync(project);
        }

        public async Task UpdateProj(Project project)
        {
            await _connection.UpdateAsync(project);
        }

        public async Task DeleteProj(Project project)
        {
            await _connection.DeleteAsync(project);
        }

        public async Task<List<Experiment>> GetExperiments()
        {
            return await _connection.Table<Experiment>().ToListAsync();
        }
        public async Task<Experiment> GetByIdEx(int id)
        {
            return await _connection.Table<Experiment>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateEx(Experiment experiment)
        {
            await _connection.InsertAsync(experiment);
        }

        public async Task UpdateEx(Experiment experiment)
        {
            await _connection.UpdateAsync(experiment);
        }

        public async Task DeleteEx(Experiment experiment)
        {
            await _connection.DeleteAsync(experiment);
        }

    }

}
