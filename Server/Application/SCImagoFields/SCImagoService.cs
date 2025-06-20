﻿using Application.Shared.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.SCImagoFields
{
    public class SCImagoFieldService : GenericCachedService<SCImagoFieldDto, SCImagoField>, ISCImagoFieldService
    {
        protected override TimeSpan defaultCacheTime => TimeSpan.FromHours(24);

        public SCImagoFieldService(
            IUnitOfWork unitOfWork,
            IGenericMapper<SCImagoFieldDto, SCImagoField> mapper,
            IDistributedCache cache,
            ILogger<SCImagoFieldService> logger
            )
            : base(unitOfWork, mapper, cache, logger)
        {
        }
    }
}
