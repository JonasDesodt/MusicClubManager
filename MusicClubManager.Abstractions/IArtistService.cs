﻿using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Filters;

namespace MusicClubManager.Abstractions
{
    public interface IArtistService : IService<ArtistRequest, ArtistResult, ArtistFilter> { }
}