using CW_Fantasy_App.Data;
using CW_Fantasy_App.Services;
using Entities.FootballModels;
using Services.DTOs;
using Services;
using Services.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class FootballDataService
    {
        private readonly HttpClient _httpClient;
        private readonly FootballDataOptions _options;
        private readonly ApplicationDbContext _context;

        public FootballDataService(
            HttpClient httpClient,
            IOptions<FootballDataOptions> options,
            ApplicationDbContext context)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _context = context;
        }

        public async Task GetTeamAsync(int apiId, CancellationToken ct = default)
        {
            var url = _options.BaseURL + _options.Endpoints.TeamDetails.Replace("{id}", apiId.ToString());

            if (!_httpClient.DefaultRequestHeaders.Contains(_options.CustomKeyApiName))
                _httpClient.DefaultRequestHeaders.Add(
                    _options.CustomKeyApiName, _options.ApiKey);

            var json = await _httpClient.GetStringAsync(url, ct);

            var dto = JsonSerializer.Deserialize<TeamApiResponse>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (dto == null) return;

            var club = await _context.Clubs
                .FirstOrDefaultAsync(c => c.ApiId == dto.Id, ct);

            if (club == null)
            {
                club = new Club
                {
                    ApiId = dto.Id,
                    Name = dto.Name,
                    CrestUrl = dto.Crest
                };
                _context.Clubs.Add(club);
            }

            if (!_context.Coaches.Any(c => c.ApiId == dto.Coach.Id))
            {
                _context.Coaches.Add(new Coach
                {
                    ApiId = dto.Coach.Id,
                    Name = dto.Coach.Name,
                    Nationality = dto.Coach.Nationality,
                    DateOfBirth = dto.Coach.DateOfBirth,
                    Club = club
                });
            }

            foreach (var p in dto.Squad)
            {
                if (!_context.Players.Any(x => x.ApiId == p.Id))
                {
                    _context.Players.Add(new Player
                    {
                        ApiId = p.Id,
                        Name = p.Name,
                        Position = p.Position,
                        Nationality = p.Nationality,
                        DateOfBirth = p.DateOfBirth,
                        Club = club
                    });
                }
            }

            await _context.SaveChangesAsync(ct);
        }
    }

}
