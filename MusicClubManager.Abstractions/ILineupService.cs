﻿using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Transfer;

namespace MusicClubManager.Abstractions
{
    public interface ILineupService : IService<LineupRequest, LineupResult, LineupFilter> 
    {
        Task<ServiceResult<LineupResult>> Previous(int id);

        Task<ServiceResult<LineupResult>> Next(int id);
    }
}