﻿using Core.Constants;
using Core.Ultilities.Seeders;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Infrastructure.Persistence.Seeders
{
    public class JsonDataSeeder<T> : IDataSeeder where T : class
    {
        // =====================================
        // === Fields & Props
        // =====================================

        private readonly IFileReader _fileReader;
        private string _absoluteFilePathJson = default!;
        private readonly ApplicationDbContext _dbContext;

        // =====================================
        // === Constructors
        // =====================================

        public JsonDataSeeder(IFileReader fileReader, ApplicationDbContext dbContext)
        {
            _fileReader = fileReader;
            _dbContext = dbContext;
        }

        // =====================================
        // === Methods
        // =====================================

        /// <summary>
        /// Add file before parse to json object
        /// </summary>
        /// <param name="relativeFilePath"></param>
        /// <returns></returns>
        public JsonDataSeeder<T> AddRelativeFilePath(string relativeFilePath)
        {
            _absoluteFilePathJson = Path.Combine(AppCts.AbsoluteProjectPath, relativeFilePath);
            return this;
        }

        /// <summary>
        /// Parse Json string To particula object
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<IEnumerable<T>> ParseJsonToObject()
        {
            try
            {
                var json = await _fileReader.ReadFileAsync(_absoluteFilePathJson);
                var settings = new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                };

                var data = JsonConvert.DeserializeObject<IEnumerable<T>>(json, settings) ?? Enumerable.Empty<T>();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Seed particular 1 entity from json file
        /// </summary>
        /// <returns></returns>
        public async Task SeedAsync()
        {
            // check file exists first
            if (string.IsNullOrEmpty(_absoluteFilePathJson)) throw new FileNotFoundException("Does not have file");

            // Seed data based on entity
            if (await _dbContext.Database.CanConnectAsync())
            {
                if (!await _dbContext.Set<T>().AnyAsync())
                {
                    await _dbContext.Set<T>().AddRangeAsync(await ParseJsonToObject());
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}