﻿using Infrastructure.Contracts.Request;
using Infrastructure.Contracts.Response;
using System;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IClusterBusiness
    {
        Task<ClusterItemResponse[]> ListClusterAsync();
        Task<bool> CreateClusterAsync(ClusterCreateRequest request);
        Task<(bool found, bool update)> UpdateClusterAsync(Guid id, ClusterUpdateRequest request);
        Task<(bool found, bool update)> DeleteClusterAsync(Guid id);
        Task<(bool found, bool restart)> RestartClusterMasterAsync(Guid id);
        Task<(bool found, bool ready, KubeconfigDownloadResponse file)> DownloadKubeconfigAsync(Guid id);
    }
}
